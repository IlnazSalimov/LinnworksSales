using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LinnworksSales.Data.Data.Repository.Interfaces;
using LinnworksSales.Data.Enums;
using LinnworksSales.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Z.BulkOperations;

namespace LinnworksSales.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImportController : ControllerBase
    {
        public IRegionRepository RegionRepository { get; set; }

        public ICountryRepository CountryRepository { get; set; }

        public IItemTypeRepository ItemTypeRepository { get; set; }

        public ISaleRepository SaleRepository { get; set; }

        public ILogger Logger { get; set; }

        public ImportController(IRegionRepository regionRepository, ICountryRepository countryRepository, 
            IItemTypeRepository itemTypeRepository, ISaleRepository saleRepository, ILogger<ImportController> logger)
        {
            RegionRepository = regionRepository;
            CountryRepository = countryRepository;
            ItemTypeRepository = itemTypeRepository;
            SaleRepository = saleRepository;
            Logger = Logger;
        }

        [HttpPost("sales")]
        public async Task<IActionResult> ImportSales(List<IFormFile> files)
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

            while (!streamReader.EndOfStream)
            {
                data = streamReader.ReadLine();
                try
                {
                    salesDenormal.Add(new SaleDenormalizedModel(data.Split(seperators, StringSplitOptions.RemoveEmptyEntries)));
                }
                catch (IndexOutOfRangeException e)
                {
                    //Logger.LogError("One line in the import file have incorrect format.");
                    // Continue import
                    continue;
                }
            }

            return salesDenormal;
        }

        private async Task NormalizeAndSave(List<SaleDenormalizedModel> salesDenormal)
        {
            List<Region> regions = salesDenormal.Select(s => s.Region).Distinct().Select(s => new Region() { Name = s }).ToList();
            await RegionRepository.BulkMergeAsync(regions, options => options.ColumnPrimaryKeyExpression = r => r.Name);

            List<Country> countries = salesDenormal.GroupBy(s => s.Country).
                Select(s => new Country()
                {
                    Name = s.Key,
                    Region = regions.FirstOrDefault(r => r.Name == s.FirstOrDefault()?.Region)
                }).ToList();
            await CountryRepository.BulkMergeAsync(countries, options => options.ColumnPrimaryKeyExpression = c => c.Name);

            List<ItemType> itemTypes = salesDenormal.Select(s => s.ItemType).Distinct().Select(s => new ItemType() { Name = s }).ToList();
            await ItemTypeRepository.BulkMergeAsync(itemTypes, options => options.ColumnPrimaryKeyExpression = i => i.Name);

            List<Sale> sales = salesDenormal.Select(s => new Sale()
            {
                Country = countries.FirstOrDefault(r => r.Name == s.Country),
                ItemType = itemTypes.FirstOrDefault(t => t.Name == s.ItemType),
                SalesChanel = Enum.Parse<SalesChanel>(s.SalesChanel),
                OrderDate = DateTime.Parse(s.OrderDate),
                OrderPriority = Enum.Parse<OrderPriority>(s.OrderPriority),
                ShipDate = DateTime.Parse(s.ShipDate),
                UnitCost = decimal.Parse(s.UnitCost),
                UnitPrice = decimal.Parse(s.UnitPrice),
                UnitsSold = int.Parse(s.UnitsSold),
                OrderId = int.Parse(s.OrderId)
            }).ToList();
            await SaleRepository.BulkMergeAsync(sales, options => options.ColumnPrimaryKeyExpression = s => s.OrderId);
        }
    }
}