using LinnworksSales.Data.Data.Models.Entity;
using LinnworksSales.Data.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LinnworksSales.Data.Models
{
    public class PageEntitiesContainer<TEntity> where TEntity : class, IEntity
    {
        private readonly IFilterableRepository<TEntity> _filterByRepository;
        private readonly IList<Filter> _lazyFilters = new List<Filter>();
        private IQueryable<TEntity> entities;
        private IEnumerable<TEntity> _onePageItems;
        private int _totalItemsCount;

        public PageEntitiesContainer(IQueryable<TEntity> entities, IFilterableRepository<TEntity> filterByRepository)
        {
            this.entities = entities;
            _filterByRepository = filterByRepository;
        }

        public int TotalItemsCount
        {
            get
            {
                if (_onePageItems != null)
                {
                    return _totalItemsCount;
                }

                return QueryableItems.Count();
            }
        }

        public IQueryable<TEntity> QueryableItems
        {
            get
            {
                if (_lazyFilters.Count != 0 && _filterByRepository != null)
                {
                    entities = _filterByRepository.FilterBy(entities, _lazyFilters.ToArray());
                    _lazyFilters.Clear();
                }

                return entities;
            }
        }

        public IEnumerable<TEntity> OnePageItems
        {
            get
            {
                if (_onePageItems == null)
                {
                    SetPage(1, 50);
                }

                return _onePageItems;
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

            _onePageItems = QueryableItems.Skip(skipItemsCount).Take(count);
            _totalItemsCount = QueryableItems.Count();
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
