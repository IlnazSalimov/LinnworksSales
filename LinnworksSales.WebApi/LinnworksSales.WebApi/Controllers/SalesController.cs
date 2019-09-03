using LinnworksSales.Data.Data.Repository.Interfaces;
using LinnworksSales.Data.Enums;
using LinnworksSales.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LinnworksSales.Data.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private IRegionRepository RegionRepository { get; set; }

        private ICountryRepository CountryRepository { get; set; }

        private IItemTypeRepository ItemTypeRepository { get; set; }

        private ISaleRepository SaleRepository { get; set; }

        private ILogger Logger { get; set; }
        /// <summary>
        /// Provide access to object mapper
        /// </summary>
        public ICommonMapper Mapper { get; }

        public SalesController(ISaleRepository orderRepository, IRegionRepository regionRepository,
            ICountryRepository countryRepository, IItemTypeRepository itemTypeRepository, ICommonMapper mapper, 
            ILogger<SalesController> logger)
        {
            SaleRepository = orderRepository;
            RegionRepository = regionRepository;
            CountryRepository = countryRepository;
            ItemTypeRepository = itemTypeRepository;
            Mapper = mapper;
            Logger = logger;
        }

        /// <summary>
        /// Action that only support the HTTP GET method wich return all sales. 
        /// </summary>
        /// <returns>Return JSON array with StatucCode 200</returns>
        [HttpGet]
        public IActionResult Get(int? page, int count, string sortColumn, string direction = "asc", string country = "")
        {
            Logger.LogDebug("Hi");
            PageEntitiesContainer<Sale> pageEntities = new PageEntitiesContainer<Sale>(
                SaleRepository.GetAll().Include(s => s.Country).Include(s => s.ItemType), SaleRepository);

            pageEntities.RegisterFilter(new Filter(typeof(Country), country));

            switch (sortColumn)
            {
                case "orderDate":
                    pageEntities.OrderBy(s => s.OrderDate, direction);
                    break;
            }

            pageEntities.SetPage(page, count > 0 ? count : 50);

            return Ok(Mapper.Map<PageEntitiesContainerDto<SaleDto>>(pageEntities));
        }

        /// <summary>
        /// Action that only support the HTTP GET method which return sale by id. 
        /// </summary>
        /// <returns>400 Bad Request if instructor by id doesn`t exist or 201 Created if success</returns>
        [HttpGet("{id}", Name = "GetSale")]
        public async Task<IActionResult> Get(int id)
        {
            Sale dbOrder = await SaleRepository.GetAsync(id);

            if (dbOrder == null)
            {
                string message = $"Can not find sale with ID:{id}";
                Logger.LogDebug(message);
                return BadRequest(message);
            }

            return Ok(dbOrder);
        }

        /// <summary>
        /// Action that only support the HTTP POST method, which create a new sale. 
        /// </summary>
        /// <param name="sale">New instructor</param>
        /// <returns>400 Bad Request if argument is null or 201 Created if success</returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]SaleDto sale)
        {
            if (sale == null)
            {
                return BadRequest();
            }

            Sale saleToSave = Mapper.Map<Sale>(sale);
            if (!await SaleRepository.SaveAsync(saleToSave))
            {
                string message = $"Creating a sale failed on save.";
                Logger.LogWarning(message);
                return BadRequest(message);
            }

            SaleDto orderToReturn = Mapper.Map<SaleDto>(saleToSave);
            return CreatedAtRoute("GetOrder", new { id = orderToReturn.Id }, orderToReturn);
        }

        /// <summary>
        /// Action that only support the HTTP PUT method, which update sale. 
        /// </summary>
        /// <param name="id">Sale ID to be updated</param>
        /// <param name="sale">Sale to update</param>
        /// <returns>400 Bad Request if sale by id doesn`t exist or 204 No Content if success</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody]SalePutDto sale)
        {
            Sale dbSale = await SaleRepository.GetAsync(id);

            if (sale == null)
            {
                return BadRequest();
            }

            Mapper.Map(sale, dbSale);

            if (!SaleRepository.Update(dbSale))
            {
                string message = $"Updating a sale {id} failed on save.";
                Logger.LogWarning(message);
                return BadRequest(message);
            }

            return NoContent();
        }

        /// <summary>
        /// Action that only support the HTTP DELETE method, which delete sale. 
        /// </summary>
        /// <param name="id">Instructor ID to be deleted</param>
        /// <returns>400 Bad Request if instructor by id doesn`t exist or 204 No Content if success</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            Sale order = await SaleRepository.GetAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            if (!SaleRepository.Delete(order))
            {
                string message = $"Deleting a sale {id} failed on save.";
                Logger.LogWarning(message);
                return BadRequest(message);
            }
            return NoContent();
        }

        /// <summary>
        /// Action that only support the HTTP DELET method, which bulk delete sales. 
        /// </summary>
        /// <param name="id">Sales ID array to be deleted</param>
        /// <returns>400 Bad Request if sale by id doesn`t exist or 204 No Content if success</returns>
        [HttpDelete()]
        public async Task<ActionResult> Delete([FromQuery]int[] ids)
        {
            IQueryable<Sale> sales = SaleRepository.GetAll().Where(s => ids.Contains(s.Id));
            await SaleRepository.BulkDeleteAsync(sales);
            return NoContent();
        }
    }
}
