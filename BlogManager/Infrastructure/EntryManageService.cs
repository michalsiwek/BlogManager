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
        void PreSavingDataProcessing(Account account, Entry entry, EntryCategory entryCategory, Entry formEntry = null);
        void ValidateEntry(Entry entry, string isVisible, Account account);
    }

    public class EntryManageService : IEntryManageService
    {
        public void PreSavingDataProcessing(Account account, Entry entry, EntryCategory entryCategory, Entry formEntry = null)
        {
            entry.NormalizeEntry();

            entry.EntryCategory = entryCategory;

            if (formEntry == null)
            {
                entry.Account = account;
                entry.CreateDate = DateTime.Now;
                entry.IsVisible = false;
            }
            else
            {
                entry.Title = formEntry.Title;
                entry.Description = formEntry.Description;
                entry.Content = formEntry.Content;
                entry.ImageUrl = formEntry.ImageUrl;
                entry.LastModifiedBy = account;
                entry.LastModification = DateTime.Now;
            }

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