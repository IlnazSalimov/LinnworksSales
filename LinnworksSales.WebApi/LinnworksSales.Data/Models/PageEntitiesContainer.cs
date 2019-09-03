using LinnworksSales.Data.Models.Entity;
using LinnworksSales.Data.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LinnworksSales.Data.Models
{
    public class PageEntitiesContainer<TEntity> where TEntity : class, IEntity
    {
        private readonly IFilterableRepository<TEntity> filterByRepository;
        private readonly IList<Filter> _lazyFilters = new List<Filter>();
        private IQueryable<TEntity> entities;
        private IEnumerable<TEntity> onePageItems;
        private int totalItemsCount;

        public PageEntitiesContainer(IQueryable<TEntity> entities, IFilterableRepository<TEntity> filterByRepository)
        {
            this.entities = entities;
            this.filterByRepository = filterByRepository;
        }

        public int TotalItemsCount
        {
            get
            {
                if (onePageItems != null)
                {
                    return totalItemsCount;
                }

                return QueryableItems.Count();
            }
        }

        public IQueryable<TEntity> QueryableItems
        {
            get
            {
                if (_lazyFilters.Count != 0 && filterByRepository != null)
                {
                    entities = filterByRepository.FilterBy(entities, _lazyFilters.ToArray());
                    _lazyFilters.Clear();
                }

                return entities;
            }
        }

        public IEnumerable<TEntity> OnePageItems
        {
            get
            {
                if (onePageItems == null)
                {
                    SetPage(1, 50);
                }

                return onePageItems;
            }
        }

        public void RegisterFilter(Filter filter)
        {
            if (_lazyFilters.Contains(filter))
            {
                return;
            }

            _lazyFilters.Add(filter);
        }

        public void SetPage(int? page, int count)
        {
            var skipItemsCount = page.HasValue && page.Value > 0 ? (page.Value - 1) * count : 0;

            onePageItems = QueryableItems.Skip(skipItemsCount).Take(count);
            totalItemsCount = QueryableItems.Count();
        }

        public void OrderBy<TKey>(Expression<Func<TEntity, TKey>> expression, string direction = "asc")
        {
            if (direction == "asc")
            {
                entities = QueryableItems.OrderByDescending(expression);
            }
            else
            {
                entities = QueryableItems.OrderBy(expression);
            }
        }
    }
}
