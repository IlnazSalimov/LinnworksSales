using System;
using AutoMapper;
using LinnworksSales.Data.Models;
using LinnworksSales.Data.Models.Dto;
using LinnworksSales.Data.Repository.Interfaces;

namespace LinnworksSales.WebApi.AutoMapper
{
    public class CommonMapper : ICommonMapper
    {
        private readonly IMapper mapper;
        private readonly ICountryRepository countryRepository;
        private readonly IItemTypeRepository itemTypeRepository;

        public CommonMapper(ICountryRepository countryRepository, IItemTypeRepository itemTypeRepository)
        {
            MapperConfiguration config = GetConfiguration();
            mapper = config.CreateMapper();
            this.countryRepository = countryRepository;
            this.itemTypeRepository = itemTypeRepository;
        }

        public object Map(object source, Type sourceType, Type destinationType)
        {
            return mapper.Map(source, sourceType, destinationType);
        }

        public object Map(object source, object destination)
        {
            return mapper.Map(source, destination);
        }

        public T Map<T>(object source) where T : class
        {
            return (T)mapper.Map(source, source.GetType(), typeof(T));
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
                        expression => expression.MapFrom(m => countryRepository.Get(m.CountryId))).
                    ForMember(destinationMember => destinationMember.ItemType,
                        expression => expression.MapFrom(m => itemTypeRepository.Get(m.ItemTypeId)));

                cfg.CreateMap(typeof(PageEntitiesContainer<>), typeof(PageEntitiesContainerDto<>)).ConvertUsing(typeof(PageEntitiesContainerConverter<,>));
            });
        }
    }
}
