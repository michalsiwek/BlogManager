using BlogManager.Models;
using BlogManager.Models.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogManager.Repositories.Api
{
    public interface ICatetegoriesRepository
    {
        IEnumerable<ContentCategory> GetActiveContentCategories();
        IEnumerable<ContentSubcategory> GetContentSubcategoriesByParentId(int id);
    }

    public class CategoriesRepository : ICatetegoriesRepository
    {
        public IEnumerable<ContentCategory> GetActiveContentCategories()
        {
            using (var context = new ApplicationDbContext())
                return context.ContentCategories
                    .Where(c => c.IsActive && c.Id != 1).ToList();
        }

        public IEnumerable<ContentSubcategory> GetContentSubcategoriesByParentId(int id)
        {
            var query = string.Format(@";WITH CTE AS (SELECT DISTINCT cs.Id
				FROM dbo.ContentSubcategories cs
                            INNER JOIN dbo.Entries e ON e.ContentSubcategory_Id = cs.Id
                        WHERE cs.ContentCategory_Id = {0})
						SELECT cs.* FROM dbo.ContentSubcategories cs
							INNER JOIN CTE c ON c.Id = cs.Id
						ORDER BY [Name] ASC", id);

            using (var context = new ApplicationDbContext())
            {
                var output = context.ContentSubcategories
                    .SqlQuery(query)
                    .ToList();

                return output;
            }                
        }
    }
}