using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApplication5.Data;
using WebApplication5.IRepository;
using WebApplication5.Models;

namespace WebApplication5.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HotelController : ControllerBase
{
    // private readonly ILogger _logger;
    private readonly ILogger<HotelController> _logger;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public HotelController(IUnitOfWork unitOfWork, ILogger<HotelController> logger, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetHotels(RequestParams model)
    {
        var hotels = await _unitOfWork.Hotels.GetAll(model);
        var result = _mapper.Map<IList<HotelDto>>(hotels);

        return Ok(result);
    }

    [HttpGet("{id:int}", Name = "GetHotel")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    //[Authorize]
    public async Task<IActionResult> GetHotel(int id)
    {
        var hotel = await _unitOfWork.Hotels.Get(x => x.Id == id, x => x.Include(y => y.Country));
        var result = _mapper.Map<HotelDto>(hotel);

        return Ok(result);
    }


    [HttpPost]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> CreateHotel([FromBody] CreateHotelDto hotelDto)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogError($"Invalid post attempt in  {nameof(CreateHotel)}");
            return BadRequest(ModelState);
        }


        var hotel = _mapper.Map<Hotel>(hotelDto);
        await _unitOfWork.Hotels.Insert(hotel);
        await _unitOfWork.Save();
        return CreatedAtRoute("GetHotel", new {id = hotel.Id}, hotel);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> UpdateHotel([FromBody] UpdateHotelDto hotelDto)
    {
        if (!ModelState.IsValid && hotelDto.Id < 1)
        {
            _logger.LogError($"Invalid Put attempt in  {nameof(UpdateHotel)}");
            return BadRequest(ModelState);
        }

        var hotel = await _unitOfWork.Hotels.Get(x => x.Id == hotelDto.Id);
        if (hotel == null)
        {
            _logger.LogError($"Submitted data is invalid in  {nameof(UpdateHotel)}");
            return BadRequest("Submitted data is invalid");
        }

        _mapper.Map(hotelDto, hotel);
        _unitOfWork.Hotels.Update(hotel);
        await _unitOfWork.Save();
        return CreatedAtRoute("GetHotel", new {id = hotel.Id}, hotel);
    }

    [HttpDelete("{id:int}", Name = "DeleteHotel")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> DeleteHotel(int id)
    {
        if (id < 1)
        {
            _logger.LogError($"Invalid Delete attempt in  {nameof(DeleteHotel)}");
            return BadRequest(ModelState);
        }

        var hotel = await _unitOfWork.Hotels.Get(x => x.Id == id);
        if (hotel != null)
        {
            await _unitOfWork.Hotels.Delete(id);
            await _unitOfWork.Save();
        }

        return NoContent();
    }
}