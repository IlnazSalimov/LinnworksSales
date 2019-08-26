using System;
using System.Text;
using AutoMapper;
using LinnworksSales.Data.Models;
using LinnworksSales.WebApi.Data.Models;

namespace LinnworksSales.WebApi.Models
{
    public class CommonMapper : ICommonMapper
    {
        public IMapper Mapper { get; set; }

        public CommonMapper()
        {
            MapperConfiguration config = GetConfiguration();
            Mapper = config.CreateMapper();
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
                cfg.CreateMap<Sale, SaleDto>().
                    ForMember(destinationMember => destinationMember.Country,
                        expression => expression.MapFrom(m => m.Country.Name)).
                    ForMember(destinationMember => destinationMember.ItemType,
                        expression => expression.MapFrom(m => m.ItemType.Name));
            });
        }
    }
}
