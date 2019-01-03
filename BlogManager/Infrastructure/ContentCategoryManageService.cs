using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BlogManager.Models.Categories;

namespace BlogManager.Infrastructure
{
    public interface IContentCategoryManageService
    {
        void ActivateContentCategory(ContentCategory contentCategory, string isActive);
        void PreSavingDataProcessing(ContentCategory contentCategory);
    }

    public class ContentCategoryManageService : IContentCategoryManageService
    {
        public void ActivateContentCategory(ContentCategory contentCategory, string isActive)
        {
            switch (isActive.ToLower())
            {
                case "true":
                    contentCategory.IsActive = true;
                    break;
                case "false":
                    contentCategory.IsActive = false;
                    break;
                default: break;
            }
        }

        public void PreSavingDataProcessing(ContentCategory contentCategory)
        {
            if (contentCategory.CreateDate == DateTime.MinValue)
                contentCategory.CreateDate = DateTime.Now;
            else
            {
                contentCategory.LastModification = DateTime.Now;
                contentCategory.Name = contentCategory.Name;
                contentCategory.Description = contentCategory.Description;
                contentCategory.IsActive = contentCategory.IsActive;
            }
        }
    }
}