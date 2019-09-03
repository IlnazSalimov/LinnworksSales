using LinnworksSales.Data.Enums;
using LinnworksSales.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LinnworksSales.Data.Repository.Interfaces;
using LinnworksSales.WebApi.Models;

namespace LinnworksSales.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImportController : ControllerBase
    {
        private readonly IRegionRepository regionRepository;

        private readonly ICountryRepository countryRepository;

        private readonly IItemTypeRepository itemTypeRepository;

        private readonly ISaleRepository saleRepository;

        private readonly ILogger logger;

        public ImportController(IRegionRepository regionRepository, ICountryRepository countryRepository,
            IItemTypeRepository itemTypeRepository, ISaleRepository saleRepository, ILogger<ImportController> logger)
        {
            this.regionRepository = regionRepository;
            this.countryRepository = countryRepository;
            this.itemTypeRepository = itemTypeRepository;
            this.saleRepository = saleRepository;
            this.logger = logger;
        }

        [HttpPost("sales")]
        public async Task<IActionResult> ImportSales([FromForm(Name = "files")]List<IFormFile> files)
        {
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    using (var stream = formFile.OpenReadStream())
                    using (var streamReader = new StreamReader(stream))
                    {
                        await NormalizeAndSave(ParseSales(streamReader));
                    }
                }
            }

            return Ok(new { count = files.Count, size = files.Sum(f => f.Length) });
        }

        private List<SaleDenormalizedModel> ParseSales(StreamReader streamReader)
        {
            char[] seperators = { ',' };
            List<SaleDenormalizedModel> salesDenormal = new List<SaleDenormalizedModel>();
            string data = streamReader.ReadLine();

            int currentLine = 0;
            while (!streamReader.EndOfStream)
            {
                currentLine++;
                data = streamReader.ReadLine();
                
                try
                {
                    if (data == null)
                    {
                        logger.LogError($"Import file have incorrect format in {currentLine} line.");
                        continue; // Continue import
                    }
                    salesDenormal.Add(new SaleDenormalizedModel(data.Split(seperators, StringSplitOptions.RemoveEmptyEntries)));
                }
                catch (IndexOutOfRangeException e)
                {
                    logger.LogError(e, $"Import file have incorrect format in {currentLine} line.");
                    continue; // Continue import
                }
            }

            return salesDenormal;
        }

        private async Task NormalizeAndSave(List<SaleDenormalizedModel> salesDenormal)
        {
            List<Region> regions = salesDenormal.Select(s => s.Region).Distinct().Select(s => new Region() { Name = s }).ToList();
            await regionRepository.BulkMergeAsync(regions, options => options.ColumnPrimaryKeyExpression = r => r.Name);

            List<Country> countries = salesDenormal.GroupBy(s => s.Country).
                Select(s => new Country()
                {
                    Name = s.Key,
                    Region = regions.FirstOrDefault(r => r.Name == s.FirstOrDefault()?.Region)
                }).ToList();
            await countryRepository.BulkMergeAsync(countries, options => options.ColumnPrimaryKeyExpression = c => c.Name);

            List<ItemType> itemTypes = salesDenormal.Select(s => s.ItemType).Distinct().Select(s => new ItemType() { Name = s }).ToList();
            await itemTypeRepository.BulkMergeAsync(itemTypes, options => options.ColumnPrimaryKeyExpression = i => i.Name);

            List<Sale> sales = salesDenormal.Select(s => new Sale()
            {
                Country = countries.FirstOrDefault(r => r.Name == s.Country),
                ItemType = itemTypes.FirstOrDefault(t => t.Name == s.ItemType),
                SalesChanel = Enum.Parse<SalesChanel>(s.SalesChanel),
                OrderDate = DateTime.ParseExact(s.OrderDate, "M/d/yyyy", CultureInfo.InvariantCulture),
                OrderPriority = Enum.Parse<OrderPriority>(s.OrderPriority),
                ShipDate = DateTime.ParseExact(s.ShipDate, "M/d/yyyy", CultureInfo.InvariantCulture),
                UnitCost = decimal.Parse(s.UnitCost, CultureInfo.InvariantCulture),
                UnitPrice = decimal.Parse(s.UnitPrice, CultureInfo.InvariantCulture),
                UnitsSold = int.Parse(s.UnitsSold),
                OrderId = int.Parse(s.OrderId)
            }).ToList();
            await saleRepository.BulkMergeAsync(sales, options => options.ColumnPrimaryKeyExpression = s => s.OrderId);
        }
    }
}