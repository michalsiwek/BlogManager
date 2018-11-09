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
        IEnumerable<EntryCategory> GetEntryCategories();
        IEnumerable<EntryCategory> GetActiveEntryCategories();
        EntryCategory GetEntryCategory(int categoryId);
        DbRepoStatusCode DeleteEntryCategory(int categoryId);
        DbRepoStatusCode ActivateEntryCategory(int categoryId, string isActive);
        void SaveEntryCategory(EntryCategory entryCategory);
    }

    public class EntryCategoryRepository : IEntryCategoryRepository
    {
        private readonly IEntryCategoryManageService _categoryManageService;

        public EntryCategoryRepository()
        {
            _categoryManageService = new EntryCategoryManageService();
        }

        public IEnumerable<EntryCategory> GetEntryCategories()
        {
            using (var context = new ApplicationDbContext())
                return context.EntryCategories.ToList();
        }

        public IEnumerable<EntryCategory> GetActiveEntryCategories()
        {
            using (var context = new ApplicationDbContext())
                return context.EntryCategories.Where(c => c.IsActive).ToList();
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

                var entries = context.Entries.Where(e => e.EntryCategory.Id == categoryId).ToList();

                if (entries.Count > 0)
                {
                    var defaultCategory = context.EntryCategories.SingleOrDefault(c => c.Id == 1);
                    foreach (var entry in entries)
                        entry.EntryCategory = defaultCategory;
                }

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