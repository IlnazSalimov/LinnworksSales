using LinnworksSales.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using LinnworksSales.Data.Models.Dto;
using LinnworksSales.Data.Repository.Interfaces;
using LinnworksSales.WebApi.AutoMapper;

namespace LinnworksSales.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly ICountryRepository countryRepository;

        /// <summary>
        /// Provide access to object mapper
        /// </summary>
        private readonly ICommonMapper mapper;

        public CountriesController(ICountryRepository countryRepository, ICommonMapper mapper)
        {
            this.countryRepository = countryRepository;
            this.mapper = mapper;
        }

        /// <summary>
        /// Action that only support the HTTP GET method wich return all countries. 
        /// </summary>
        /// <returns>Return JSON array with StatucCode 200</returns>
        [HttpGet]
        public IActionResult Get()
        { 
            return Ok(mapper.Map<List<CountryDto>>(countryRepository.GetAll()));
        }
    }
}
