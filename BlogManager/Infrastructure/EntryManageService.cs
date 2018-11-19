using BlogManager.Models.Accounts;
using BlogManager.Models.Categories;
using BlogManager.Models.Entries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogManager.Infrastructure
{
    public interface IEntryManageService
    {
        void PreSavingNewDataProcessing(Account account, Entry entry, EntryCategory entryCategory);
        void PreSavingModifiedDataProcessing(Account account, Entry entry, EntryCategory entryCategory, Entry formEntry);
        void ValidateEntry(Entry entry, string isVisible, Account account);
    }

    public class EntryManageService : IEntryManageService
    { 
        public void PreSavingNewDataProcessing(Account account, Entry entry, EntryCategory entryCategory)
        {
            entry.NormalizeEntry();

            entry.EntryCategory = entryCategory;
            entry.Account = account;
            entry.CreateDate = DateTime.Now;
            entry.IsVisible = false;

            entry.GetParagraphsFromContent();
        }

        public void PreSavingModifiedDataProcessing(Account account, Entry entry, EntryCategory entryCategory, Entry formEntry)
        {
            entry.NormalizeEntry();

            entry.EntryCategory = entryCategory;
            entry.Title = formEntry.Title;
            entry.Description = formEntry.Description;
            entry.Content = formEntry.Content;
            entry.ImageUrl = formEntry.ImageUrl;
            entry.LastModifiedBy = account;
            entry.LastModification = DateTime.Now;

            entry.GetParagraphsFromContent();
        }

        public void ValidateEntry(Entry entry, string isVisible, Account account)
        {
            entry.LastModifiedBy = account;
            entry.LastModification = DateTime.Now;

            switch (isVisible.ToLower())
            {
                case "true":
                    entry.IsVisible = true;
                    break;
                case "false":
                    entry.IsVisible = false;
                    break;
                default:
                    break;
            }
        }
    }
}