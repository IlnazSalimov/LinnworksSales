using System;
using System.Text;
using AutoMapper;
using LinnworksSales.Data.Models;
using LinnworksSales.Data.AutoMapper;
using LinnworksSales.Data.Data.Models;
using LinnworksSales.Data.Data.Repository.Interfaces;

namespace LinnworksSales.Data.Models
{
    public class CommonMapper : ICommonMapper
    {
        public IMapper Mapper { get; set; }

        public ICountryRepository CountryRepository { get; set; }
        public IItemTypeRepository ItemTypeRepository { get; set; }

        public CommonMapper(ICountryRepository CountryRepository, IItemTypeRepository ItemTypeRepository)
        {
            MapperConfiguration config = GetConfiguration();
            Mapper = config.CreateMapper();
            this.CountryRepository = CountryRepository;
            this.ItemTypeRepository = ItemTypeRepository;
        }

        public object Map(object source, Type sourceType, Type destinationType)
        {
            return Mapper.Map(source, sourceType, destinationType);
        }

        public object Map(object source, object destination)
        {
            return Mapper.Map(source, destination);
        }

        public T Map<T>(object source) where T : class
        {
            return (T)Mapper.Map(source, source.GetType(), typeof(T));
        }


        private MapperConfiguration GetConfiguration()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Country, CountryDto>();
                cfg.CreateMap<CountryDto, Country>();
                cfg.CreateMap<ItemType, ItemTypeDto>();
                cfg.CreateMap<ItemTypeDto, ItemType>();
                cfg.CreateMap<Sale, SaleDto>().
                ForMember(destinationMember => destinationMember.OrderDate,
                        expression => expression.MapFrom(m => m.OrderDate.ToShortDateString())).
                    ForMember(destinationMember => destinationMember.ShipDate,
                        expression => expression.MapFrom(m => m.ShipDate.ToShortDateString()));
                cfg.CreateMap<SalePutDto, Sale>().
                    ForMember(destinationMember => destinationMember.Country,
                        expression => expression.MapFrom(m => CountryRepository.Get(m.CountryId))).
                    ForMember(destinationMember => destinationMember.ItemType,
                        expression => expression.MapFrom(m => ItemTypeRepository.Get(m.ItemTypeId)));

                cfg.CreateMap(typeof(PageEntitiesContainer<>), typeof(PageEntitiesContainerDto<>)).ConvertUsing(typeof(PageEntitiesContainerConverter<,>));
            });
        }
    }
}
