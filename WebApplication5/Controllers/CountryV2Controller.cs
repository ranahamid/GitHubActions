using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using WebApplication5.Data;
using WebApplication5.IRepository;
using WebApplication5.Models;

namespace WebApplication5.Controllers
{
    [ApiVersion("2.0",Deprecated = true)]
    [Route("api/country")]
    [ApiController]
    public class CountryV2Controller : ControllerBase
    {
        //private readonly IUnitOfWork _unitOfWork;
        //private readonly ILogger<CountryController> _logger;
        //private readonly IMapper _mapper;
        private readonly DatabaseContext _database;
        public CountryV2Controller(DatabaseContext database)
        {
            _database = database;
        }

        //public CountryV2Controller(IUnitOfWork unitOfWork, ILogger<CountryController> logger, IMapper mapper)
        //{
        //    _unitOfWork = unitOfWork;
        //    _logger = logger;
        //    _mapper = mapper;
        //}

        [HttpGet]
         
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public  IActionResult GetCountries( )
        { 
            return  Ok(_database.Countries);

        }
        //[HttpGet("{id:int}", Name = "GetCountry")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<IActionResult> GetCountry(int id)
        //{

        //    var country = await _unitOfWork.Countries.Get(x => x.Id == id, new List<string> { "Hotels" });
        //    var result = _mapper.Map<CountryDto>(country);

        //    return Ok(result);

        //}

        //[HttpPost]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Authorize]
        //public async Task<IActionResult> CreateCountry([FromBody] CreateCountryDto countryDto)
        //{

        //    if (!ModelState.IsValid)
        //    {
        //        _logger.LogError($"Invalid post attempt in  {nameof(CreateCountry)}");
        //        return BadRequest(ModelState);
        //    }
        //    var country = _mapper.Map<Country>(countryDto);
        //    await _unitOfWork.Countries.Insert(country);
        //    await _unitOfWork.Save();
        //    return CreatedAtRoute("GetCountry", new { id = country.Id }, country);

        //}


        //[HttpPut]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Authorize]
        //public async Task<IActionResult> UpdateCountry([FromBody] UpdateCountryDto countryDto)
        //{

        //    if (!ModelState.IsValid)
        //    {
        //        _logger.LogError($"Invalid put attempt in  {nameof(UpdateCountry)}");
        //        return BadRequest(ModelState);
        //    }
        //    var country = await _unitOfWork.Countries.Get(x => x.Id == countryDto.Id);
        //    if (country == null)
        //    {
        //        _logger.LogError($"Invalid country update in  {nameof(UpdateCountry)}");
        //        return BadRequest("Invalid country update");
        //    }
        //    _mapper.Map(countryDto, country);
        //    _unitOfWork.Countries.Update(country);
        //    await _unitOfWork.Save();
        //    return CreatedAtRoute("GetCountry", new { id = country.Id }, country);

        //}

        //[HttpDelete("{id:int}", Name = "DeleteCountry")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //// [Authorize]
        //public async Task<IActionResult> DeleteCountry(int id)
        //{
        //    if (id < 1)
        //    {
        //        _logger.LogError($"Invalid Delete attempt in  {nameof(DeleteCountry)}");
        //        return BadRequest(ModelState);
        //    }
        //    var hotel = await _unitOfWork.Countries.Get(x => x.Id == id);
        //    if (hotel != null)
        //    {
        //        await _unitOfWork.Countries.Delete(id);
        //        await _unitOfWork.Save();
        //    }
        //    return NoContent();

        //}
    }
}
