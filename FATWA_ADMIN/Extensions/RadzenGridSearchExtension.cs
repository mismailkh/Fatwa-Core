using Radzen;
using Radzen.Blazor;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;

namespace FATWA_ADMIN.Extensions
{
    public class RadzenGridSearchExtension
    {
        public async Task<List<T>> Filter<T>(IEnumerable<T> inputList, Query query)
            where T : class
        {
            try
            {
                var items = inputList.AsQueryable();
                if (query != null)
                {
                    if (!string.IsNullOrEmpty(query.Expand))
                    {
                        var propertiesToExpand = query.Expand.Split(',');
                        foreach (var p in propertiesToExpand)
                        {
                            items = items.Include(p);
                        }
                    }

                    if (!string.IsNullOrEmpty(query.Filter))
                    {
                        if (query.FilterParameters != null)
                        {
                            items = items.Where(query.Filter, query.FilterParameters);
                        }
                        else
                        {
                            items = items.Where(query.Filter);
                        }
                    }

                    if (!string.IsNullOrEmpty(query.OrderBy))
                    {
                        items = items.OrderBy(query.OrderBy);
                    }

                    if (query.Skip.HasValue)
                    {
                        items = items.Skip(query.Skip.Value);
                    }

                    if (query.Top.HasValue)
                    {
                        items = items.Take(query.Top.Value);
                    }
                }
                return items.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<T>> Sort<T>(IEnumerable<T> inputList, string property, SortOrder SortOrder)
            where T : class
        {
            try
            {
                var items = inputList.AsQueryable();
                if (!string.IsNullOrEmpty(property))
                {
                    items = items.OrderBy(property + " " + SortOrder);
                }
                return items.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}