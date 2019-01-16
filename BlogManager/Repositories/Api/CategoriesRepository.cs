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
        IEnumerable<ContentCategory> GetActiveEntryContentCategories();
        IEnumerable<ContentCategory> GetActiveGalleryContentCategories();
        IEnumerable<ContentSubcategory> GetEntryContentSubcategoriesByParentId(int id);
        IEnumerable<ContentSubcategory> GetGalleryContentSubcategoriesByParentId(int id);
    }

    public class CategoriesRepository : ICatetegoriesRepository
    {
        public IEnumerable<ContentCategory> GetActiveEntryContentCategories()
        {
            var query = string.Format(@";WITH CTE AS (SELECT DISTINCT c.Id
				FROM dbo.ContentCategories c
                            INNER JOIN dbo.Entries e ON e.ContentCategory_Id = c.Id
                        WHERE c.Id != 1)
						SELECT c.* FROM dbo.ContentCategories c
							INNER JOIN CTE cat ON cat.Id = c.Id
						ORDER BY [Name] ASC");

            using (var context = new ApplicationDbContext())
            {
                var output = context.ContentCategories
                    .SqlQuery(query)
                    .ToList();

                return output;
            }
        }

        public IEnumerable<ContentCategory> GetActiveGalleryContentCategories()
        {
            var query = string.Format(@";WITH CTE AS (SELECT DISTINCT c.Id
				FROM dbo.ContentCategories c
                            INNER JOIN dbo.Galleries g ON g.ContentCategory_Id = c.Id
                        WHERE c.Id != 1)
						SELECT c.* FROM dbo.ContentCategories c
							INNER JOIN CTE cat ON cat.Id = c.Id
						ORDER BY [Name] ASC");

            using (var context = new ApplicationDbContext())
            {
                var output = context.ContentCategories
                    .SqlQuery(query)
                    .ToList();

                return output;
            }
        }

        public IEnumerable<ContentSubcategory> GetEntryContentSubcategoriesByParentId(int id)
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

        public IEnumerable<ContentSubcategory> GetGalleryContentSubcategoriesByParentId(int id)
        {
            var query = string.Format(@";WITH CTE AS (SELECT DISTINCT cs.Id
				FROM dbo.ContentSubcategories cs
                            INNER JOIN dbo.Galleries g ON g.ContentSubcategory_Id = cs.Id
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