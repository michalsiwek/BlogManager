using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BlogManager.Models.Categories;

namespace BlogManager.Infrastructure
{
    public interface IEntryCategoryManageService
    {
        void ActivateEntryCategory(EntryCategory entryCategory, string isActive);
        void PreSavingDataProcessing(EntryCategory entryCategory);
    }

    public class EntryCategoryManageService : IEntryCategoryManageService
    {
        public void ActivateEntryCategory(EntryCategory entryCategory, string isActive)
        {
            switch (isActive.ToLower())
            {
                case "true":
                    entryCategory.IsActive = true;
                    break;
                case "false":
                    entryCategory.IsActive = false;
                    break;
                default: break;
            }
        }

        public void PreSavingDataProcessing(EntryCategory entryCategory)
        {
            if (entryCategory.CreateDate == DateTime.MinValue)
                entryCategory.CreateDate = DateTime.Now;
            else
            {
                entryCategory.LastModification = DateTime.Now;
                entryCategory.Name = entryCategory.Name;
                entryCategory.Description = entryCategory.Description;
                entryCategory.IsActive = entryCategory.IsActive;
            }
        }
    }
}