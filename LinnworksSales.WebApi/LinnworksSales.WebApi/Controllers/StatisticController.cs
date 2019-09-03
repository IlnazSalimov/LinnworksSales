using System.Linq;
using LinnworksSales.Data.Models;
using LinnworksSales.Data.Repository.Interfaces;
using LinnworksSales.WebApi.AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace LinnworksSales.WebApi.Controllers
{
    [Route("api/stat")]
    [ApiController]
    public class StatisticController : ControllerBase
    {
        private readonly ISaleRepository saleRepository;

        /// <summary>
        /// Provide access to object mapper
        /// </summary>
        private readonly ICommonMapper mapper;

        public StatisticController(ISaleRepository saleRepository, ICommonMapper mapper)
        {
            this.saleRepository = saleRepository;
            this.mapper = mapper;
        }

        /// <summary>
        /// Action that only support the HTTP GET method wich return count of orders have been sold in the specific
        /// country for a specific year. 
        /// </summary>
        /// <returns>Return JSON array with StatucCode 200</returns>
        [HttpGet("getsoldorders")]
        public IActionResult GetSoldOrders(int country, int year)
        {
            return Ok(saleRepository.GetAll().Count(s => s.Country.Id == country && s.OrderDate.Year == year));
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
            return Ok(saleRepository.GetAll().
                Where(s => s.Country.Id == country && s.OrderDate.Year == year).
                Sum(s => s.UnitsSold * s.UnitPrice - s.UnitsSold * s.UnitCost));
        }
    }
}
