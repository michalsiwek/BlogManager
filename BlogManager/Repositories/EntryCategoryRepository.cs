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
    public interface IEntryCategoryRepository
    {
        List<EntryCategory> GetEntryCategories();
        EntryCategory GetEntryCategory(int categoryId);
        DbRepoStatusCode DeleteEntryCategory(int categoryId);
        DbRepoStatusCode ActivateEntryCategory(int categoryId, string isActive);
        void SaveEntryCategory(EntryCategory entryCategory);
    }

    public class EntryCategoryRepository : IEntryCategoryRepository
    {
        private readonly EntryCategoryManageService _categoryManageService;

        public EntryCategoryRepository()
        {
            _categoryManageService = new EntryCategoryManageService();
        }

        public List<EntryCategory> GetEntryCategories()
        {
            using (var context = new ApplicationDbContext())
                return context.EntryCategories.ToList();
        }

        public EntryCategory GetEntryCategory(int categoryId)
        {
            using (var context = new ApplicationDbContext())
                return context.EntryCategories.SingleOrDefault(c => c.Id == categoryId);
        }

        public DbRepoStatusCode DeleteEntryCategory(int categoryId)
        {
            using (var context = new ApplicationDbContext())
            {
                var entryCategory = context.EntryCategories.SingleOrDefault(c => c.Id == categoryId);

                if (entryCategory == null)
                    return DbRepoStatusCode.NotFound;

                context.EntryCategories.Remove(entryCategory);
                context.SaveChanges();

                return DbRepoStatusCode.Success;
            }
        }

        public DbRepoStatusCode ActivateEntryCategory(int categoryId, string isActive)
        {
            using (var context = new ApplicationDbContext())
            {
                var entryCategory = context.EntryCategories.SingleOrDefault(c => c.Id == categoryId);

                if (entryCategory == null)
                    return DbRepoStatusCode.NotFound;

                _categoryManageService.ActivateEntryCategory(entryCategory, isActive);

                context.SaveChanges();

                return DbRepoStatusCode.Success;
            }
        }

        public void SaveEntryCategory(EntryCategory entryCategory)
        {
            using (var context = new ApplicationDbContext())
            {
                _categoryManageService.PreSavingDataProcessing(entryCategory);

                context.EntryCategories.AddOrUpdate(entryCategory);

                context.SaveChanges();
            }
        }

    }
}