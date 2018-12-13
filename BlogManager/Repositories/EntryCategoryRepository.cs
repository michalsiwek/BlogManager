using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using BlogManager.Helpers.Enums;
using BlogManager.Infrastructure;
using BlogManager.Models;
using BlogManager.Models.Categories;

namespace BlogManager.Repositories
{
    public interface IContentCategoryRepository
    {
        IEnumerable<ContentCategory> GetContentCategories();
        IEnumerable<ContentCategory> GetActiveContentCategories();
        ContentCategory GetContentCategory(int categoryId);
        DbRepoStatusCode DeleteContentCategory(int categoryId);
        DbRepoStatusCode ActivateContentCategory(int categoryId, string isActive);
        void SaveContentCategory(ContentCategory contentCategory);
    }

    public class ContentCategoryRepository : IContentCategoryRepository
    {
        private readonly IContentCategoryManageService _categoryManageService;

        public ContentCategoryRepository(IContentCategoryManageService categoryManageService)
        {
            _categoryManageService = categoryManageService;
        }

        public IEnumerable<ContentCategory> GetContentCategories()
        {
            using (var context = new ApplicationDbContext())
                return context.ContentCategories.ToList();
        }

        public IEnumerable<ContentCategory> GetActiveContentCategories()
        {
            using (var context = new ApplicationDbContext())
                return context.ContentCategories.Where(c => c.IsActive).ToList();
        }

        public ContentCategory GetContentCategory(int categoryId)
        {
            using (var context = new ApplicationDbContext())
                return context.ContentCategories.SingleOrDefault(c => c.Id == categoryId);
        }

        public DbRepoStatusCode DeleteContentCategory(int categoryId)
        {
            using (var context = new ApplicationDbContext())
            {
                var contentCategory = context.ContentCategories.SingleOrDefault(c => c.Id == categoryId);

                if (contentCategory == null)
                    return DbRepoStatusCode.NotFound;

                var entries = context.Entries.Where(e => e.ContentCategory.Id == categoryId).ToList();

                if (entries.Count > 0)
                {
                    var defaultCategory = context.ContentCategories.SingleOrDefault(c => c.Id == 1);
                    foreach (var entry in entries)
                        entry.ContentCategory = defaultCategory;
                }

                context.ContentCategories.Remove(contentCategory);
                context.SaveChanges();

                return DbRepoStatusCode.Success;
            }
        }

        public DbRepoStatusCode ActivateContentCategory(int categoryId, string isActive)
        {
            using (var context = new ApplicationDbContext())
            {
                var contentCategory = context.ContentCategories.SingleOrDefault(c => c.Id == categoryId);

                if (contentCategory == null)
                    return DbRepoStatusCode.NotFound;

                _categoryManageService.ActivateContentCategory(contentCategory, isActive);

                context.SaveChanges();

                return DbRepoStatusCode.Success;
            }
        }

        public void SaveContentCategory(ContentCategory contentCategory)
        {
            using (var context = new ApplicationDbContext())
            {
                _categoryManageService.PreSavingDataProcessing(contentCategory);

                context.ContentCategories.AddOrUpdate(contentCategory);

                context.SaveChanges();
            }
        }

    }
}