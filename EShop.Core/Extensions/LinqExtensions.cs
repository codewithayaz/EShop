using AutoMapper;
using System.Linq.Dynamic.Core;
using EShop.Core.Models;

namespace EShop.Core.Extensions
{
    public static class LinqExtensions
    {
        public static PaginatedResult<VM> GetPaginatedResults<VM>(this IQueryable query, 
            PaginationFilter filter, IMapper mapper)
        {
            filter.Search = filter.Search == null ? "" : filter.Search.ToUpper().Trim();

            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            validFilter.Search = filter.Search;

            int totalRecords = 0;
            if (!string.IsNullOrWhiteSpace(filter.Search) && filter.SearchColumnList != null && filter.SearchColumnList.Count > 0)
            {
                List<string> conditionList = new List<string>();
                foreach (var item in filter.SearchColumnList)
                {
                    if (item.Value == "string")
                    {
                        conditionList.Add($"{item.Key}.Contains(\"{filter.Search}\")");
                    }
                    else
                    {
                        conditionList.Add($"{item.Key}.ToString().Contains(\"{filter.Search}\")");
                    }
                }
                var condition = string.Join(" or ", conditionList);
                query = query.Where(condition);


                //consider search result only
                totalRecords = query.Count();
            }
            else
            {
                //consider all data
                totalRecords = query.Count();
            }

            if (filter.SortDirection > 0 && !string.IsNullOrEmpty(filter.SortLabel))
            {
                var sortDirection = filter.SortDirection == 1 ? "asc" : "desc";
                query = query.OrderBy($"{filter.SortLabel} {sortDirection}");
            }

            //foreach (var includeProperty in includeProperties.Split
            //    (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            //{
            //    query = query.Include(includeProperty);
            //}

            var pagedData = query.Skip((validFilter.PageNumber) * validFilter.PageSize)
                .Take(validFilter.PageSize);

            var data = mapper.Map<List<VM>>(pagedData);

            var response = new PaginatedResult<VM>(data, validFilter.PageNumber, validFilter.PageSize, totalRecords);
            response.Search = validFilter.Search;
            response.SortDirection = validFilter.SortDirection;
            response.SortLabel = validFilter.SortLabel;
            return response;
        }


        public static DataTablesResponse<T> GetPaged<T>(this IQueryable<T> query,
                                                    DataTablesRequest table) where T : class
        {
            var recordsTotal = query.Count();
            //string condition = "";

            var searchText = table.Search.Value?.ToUpper();
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                var searchableCols = table.Columns.Where(x => x.Searchable==true);
                if (searchableCols.Count() > 0)
                {
                    string condition = string.Join(" or ", searchableCols.Select(c => $"{c.Name}.Contains(\"{table.Search.Value}\")"));
                    query = query.Where(condition);
                }
            }

            var recordsFiltered = query.Count();

            var sortColumnName = table.Columns.ElementAt(table.Order.ElementAt(0).Column).Name;
            var sortDirection = table.Order.ElementAt(0).Dir.ToLower();

            // using System.Linq.Dynamic.Core
            query = query.OrderBy($"{sortColumnName} {sortDirection}");

            var skip = table.Start;
            var take = table.Length;
            var data = query
                .Skip(skip)
                .Take(take)
                .ToList();


            return new DataTablesResponse<T>
            {
                Draw = table.Draw,
                RecordsTotal = recordsTotal,
                RecordsFiltered = recordsFiltered,
                Data = data
            };
        }
    }
}
