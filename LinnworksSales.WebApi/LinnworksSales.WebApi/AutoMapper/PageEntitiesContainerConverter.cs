using AutoMapper;
using LinnworksSales.Data.Models;
using LinnworksSales.Data.Models.Dto;
using LinnworksSales.Data.Models.Entity;

namespace LinnworksSales.WebApi.AutoMapper
{
    public class PageEntitiesContainerConverter<TSource, TDest> : ITypeConverter<PageEntitiesContainer<TSource>, PageEntitiesContainerDto<TDest>> where TSource: class, IEntity
    {
        public PageEntitiesContainerDto<TDest> Convert(PageEntitiesContainer<TSource> source, PageEntitiesContainerDto<TDest> destination, ResolutionContext context)
        {
            // you now have the known types of TSource and TDest
            // you're probably creating the dest collection
            destination = destination ?? new PageEntitiesContainerDto<TDest>();
            // You're probably mapping the contents
            foreach (var sourceItem in source.OnePageItems)
            {
                destination.OnePageItems.Add(context.Mapper.Map<TSource, TDest>(sourceItem));
            }
            destination.TotalItemsCount = source.TotalItemsCount;
            //then returning that collection
            return destination;
        }
    }
}
