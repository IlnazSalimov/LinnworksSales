using LinnworksSales.Data.Data.Repository.Interfaces;
using LinnworksSales.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace LinnworksSales.Data.Controllers
{
    [Route("api/stat")]
    [ApiController]
    public class StatisticController : ControllerBase
    {
        public ISaleRepository SaleRepository { get; set; }
        /// <summary>
        /// Provide access to object mapper
        /// </summary>
        public ICommonMapper Mapper { get; }

        public StatisticController(ISaleRepository saleRepository, ICommonMapper mapper)
        {
            SaleRepository = saleRepository;
            Mapper = mapper;
        }

        /// <summary>
        /// Action that only support the HTTP GET method wich return count of orders have been sold in the specific
        /// country for a specific year. 
        /// </summary>
        /// <returns>Return JSON array with StatucCode 200</returns>
        [HttpGet("getsoldorders")]
        public IActionResult GetSoldOrders(int country, int year)
        {
            return Ok(SaleRepository.GetAll().Count(s => s.Country.Id == country && s.OrderDate.Year == year));
        }

        /// <summary>
        /// Action that only support the HTTP GET method which return profit has been earned in the specific country for a specific year
        /// </summary>
        /// <param name="country"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        [HttpGet("getprofit")]
        public IActionResult GetProfit(int country, int year)
        {
            return Ok(SaleRepository.GetAll().
                Where(s => s.Country.Id == country && s.OrderDate.Year == year).
                Sum(s => s.UnitsSold * s.UnitPrice - s.UnitsSold * s.UnitCost));
        }
    }
}
