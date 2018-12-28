﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using BlogManager.Helpers.Enums;
using BlogManager.Infrastructure;
using BlogManager.Models;
using BlogManager.Models.Categories;
using System.Data.Entity;

namespace BlogManager.Repositories
{
    public interface IContentCategoryRepository
    {
        IEnumerable<ContentCategory> GetContentCategories();
        IEnumerable<ContentCategory> GetActiveContentCategories();
        ContentCategory GetContentCategory(int categoryId);
        IEnumerable<ContentSubcategory> GetContentSubcategoriesByParentId(int id);
        DbRepoStatusCode DeleteContentCategory(int categoryId);
        DbRepoStatusCode ActivateContentCategory(int categoryId, string isActive);
        void SaveContentCategory(ContentCategory contentCategory);
        void SaveContentSubcategory(ContentSubcategoryViewModel model);
        DbRepoStatusCode DeleteContentSubcategory(int id);
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
                return context.ContentCategories
                    .Include(c => c.Subcategories)
                    .ToList();
        }

        public IEnumerable<ContentCategory> GetActiveContentCategories()
        {
            using (var context = new ApplicationDbContext())
                return context.ContentCategories
                    .Include(c => c.Subcategories)
                    .Where(c => c.IsActive).ToList();
        }

        public ContentCategory GetContentCategory(int categoryId)
        {
            using (var context = new ApplicationDbContext())
                return context.ContentCategories
                    .Include(c => c.Subcategories)
                    .SingleOrDefault(c => c.Id == categoryId);
        }

        public IEnumerable<ContentSubcategory> GetContentSubcategoriesByParentId(int id)
        {
            using (var context = new ApplicationDbContext())
                return context.ContentCategories
                    .Include(c => c.Subcategories)
                    .SingleOrDefault(c => c.Id == id)
                    .Subcategories;
        }

        public DbRepoStatusCode DeleteContentCategory(int categoryId)
        {
            using (var context = new ApplicationDbContext())
            {
                var contentCategory = context.ContentCategories
                    .Include(c => c.Subcategories)
                    .SingleOrDefault(c => c.Id == categoryId);

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

        public void SaveContentSubcategory(ContentSubcategoryViewModel model)
        {
            using (var context = new ApplicationDbContext())
            {
                var cat = context.ContentCategories
                    .Include(c => c.Subcategories)
                    .Single(c => c.Id == model.ContentCategoryId);

                model.ContentSubcategory.CreateDate = DateTime.Now;

                cat.Subcategories.Add(model.ContentSubcategory);
                
                context.SaveChanges();
            }
        }

        public DbRepoStatusCode DeleteContentSubcategory(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                var subcontentCategory = context.ContentSubcategories
                    .SingleOrDefault(c => c.Id == id);

                if (subcontentCategory == null)
                    return DbRepoStatusCode.NotFound;

                var entries = context.Entries.Where(e => e.ContentSubcategory.Id == id).ToList();

                if (entries.Count > 0)
                {
                    foreach (var entry in entries)
                        entry.ContentSubcategory = null;
                }

                context.ContentSubcategories.Remove(subcontentCategory);
                context.SaveChanges();

                return DbRepoStatusCode.Success;
            }
        }
    }
}