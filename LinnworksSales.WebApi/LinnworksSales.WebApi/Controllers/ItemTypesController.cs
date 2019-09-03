using LinnworksSales.Data.Data.Repository.Interfaces;
using LinnworksSales.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace LinnworksSales.Data.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemTypesController : ControllerBase
    {
        public IItemTypeRepository ItemTypeRepository { get; set; }
        /// <summary>
        /// Provide access to object mapper
        /// </summary>
        public ICommonMapper Mapper { get; }

        public ItemTypesController(IItemTypeRepository itemTypeRepository, ICommonMapper mapper)
        {
            ItemTypeRepository = itemTypeRepository;
            Mapper = mapper;
        }

        /// <summary>
        /// Action that only support the HTTP GET method wich return all item types. 
        /// </summary>
        /// <returns>Return JSON array with StatucCode 200</returns>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(Mapper.Map<List<ItemTypeDto>>(ItemTypeRepository.GetAll()));
        }
    }
}
