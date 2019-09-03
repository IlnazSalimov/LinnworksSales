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
    public class ItemTypesController : ControllerBase
    {
        private readonly IItemTypeRepository itemTypeRepository;
        /// <summary>
        /// Provide access to object mapper
        /// </summary>
        public ICommonMapper Mapper { get; }

        public ItemTypesController(IItemTypeRepository itemTypeRepository, ICommonMapper mapper)
        {
            this.itemTypeRepository = itemTypeRepository;
            Mapper = mapper;
        }

        /// <summary>
        /// Action that only support the HTTP GET method wich return all item types. 
        /// </summary>
        /// <returns>Return JSON array with StatucCode 200</returns>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(Mapper.Map<List<ItemTypeDto>>(itemTypeRepository.GetAll()));
        }
    }
}
