using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinnworksSales.WebApi.Data.Repository.Interfaces;
using LinnworksSales.Data.Models;
using Microsoft.AspNetCore.Mvc;
using LinnworksSales.WebApi.Models;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace LinnworksSales.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        /// <summary>
        /// Provide access to instructors in database
        /// </summary>
        public ISaleRepository OrderRepository { get; set; }
        /// <summary>
        /// Provide access to object mapper
        /// </summary>
        public ICommonMapper Mapper { get; }

        public SalesController(ISaleRepository orderRepository, ICommonMapper mapper)
        {
            OrderRepository = orderRepository;
            Mapper = mapper;
        }

        /// <summary>
        /// Action that only support the HTTP GET method wich return all instructors. 
        /// </summary>
        /// <returns>Return JSON array with StatucCode 200</returns>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(OrderRepository.GetAll());
        }

        /// <summary>
        /// Action that only support the HTTP GET method which return instructor by id. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>400 Bad Request if instructor by id doesn`t exist or 201 Created if success</returns>
        [HttpGet("{id}", Name = "GetOrder")]
        public async Task<IActionResult> Get(int id)
        {
            Sale dbOrder = await OrderRepository.GetAsync(id);

            if (dbOrder == null)
            {
                return BadRequest();
            }

            return Ok(dbOrder);
        }

        /// <summary>
        /// Action that only support the HTTP POST method, which create a new instructor. 
        /// </summary>
        /// <param name="instructor">New instructor</param>
        /// <returns>400 Bad Request if argument is null or 201 Created if success</returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]SaleDto instructor)
        {
            if (instructor == null)
            {
                return BadRequest();
            }

            Sale orderToSave = Mapper.Map<Sale>(instructor);
            if (!await OrderRepository.SaveAsync(orderToSave))
            {
                throw new Exception("Creating a instructor failed on save.");
            }

            SaleDto orderToReturn = Mapper.Map<SaleDto>(orderToSave);
            return CreatedAtRoute("GetOrder", new { id = orderToReturn.Id }, orderToReturn);
        }

        /// <summary>
        /// Action that only support the HTTP PUT method, which update instructor. 
        /// </summary>
        /// <param name="id">Instructor ID to be updated</param>
        /// <param name="order">Instructor to update</param>
        /// <returns>400 Bad Request if instructor by id doesn`t exist or 204 No Content if success</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody]SaleDto order)
        {
            Sale dbOrder = await OrderRepository.GetAsync(id);

            if (order == null)
            {
                return BadRequest();
            }

            Mapper.Map(order, dbOrder);

            if (!OrderRepository.Update(dbOrder))
            {
                throw new Exception($"Updating a instructor {id} failed on save.");
            }

            return NoContent();
        }

        /// <summary>
        /// Action that only support the HTTP PUT method, which update instructor. 
        /// </summary>
        /// <param name="id">Instructor ID to be deleted</param>
        /// <returns>400 Bad Request if instructor by id doesn`t exist or 204 No Content if success</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            Sale order = await OrderRepository.GetAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            if (!OrderRepository.Delete(order))
            {
                throw new Exception($"Deleting a instructor {id} failed on save.");
            }
            return NoContent();
        }

        [HttpPost("Import")]
        public async Task<IActionResult> Import(List<IFormFile> files)
        {
            string[] read;
            char[] seperators = { ',' };
            long size = files.Sum(f => f.Length);
            List<SaleDenormalizedModel> sales = new List<SaleDenormalizedModel>();

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    using (var stream = formFile.OpenReadStream())
                    using (var streamReader = new StreamReader(stream))
                    {
                        string data = streamReader.ReadLine();

                        while (!streamReader.EndOfStream)
                        {
                            data = streamReader.ReadLine();
                            read = data.Split(seperators, StringSplitOptions.RemoveEmptyEntries);
                            try
                            {
                                sales.Add(new SaleDenormalizedModel
                                {
                                    Region = read[0],
                                    Country = read[1],
                                    ItemType = read[2],
                                    SalesChanel = read[3],
                                    OrderPriority = read[4],
                                    OrderDate = read[5],
                                    OrderId = read[6],
                                    ShipDate = read[7],
                                    UnitsSold = read[8],
                                    UnitPrice = read[9],
                                    UnitCost = read[10]
                                });
                            }
                            catch(IndexOutOfRangeException e)
                            {
                                continue;
                            }
                        }
                    }
                }
            }

            // process uploaded files
            // Don't rely on or trust the FileName property without validation.

            return Ok(new { count = files.Count, size });
        }
    }
}
