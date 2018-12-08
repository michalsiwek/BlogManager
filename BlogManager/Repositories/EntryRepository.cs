using BlogManager.Models;
using BlogManager.Models.Entries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using BlogManager.Helpers.Enums;
using BlogManager.Infrastructure;
using System.Data.Entity.Migrations;

namespace BlogManager.Repositories
{
    public interface IEntryRepository
    {
        Entry GetEntry(int id);
        IEnumerable<Entry> GetEntries();
        void SaveEntry(Entry entry, string userIdentityName);
        DbRepoStatusCode PublishEntry(int entryId, string isActive, string userIdentityName);
        DbRepoStatusCode DeleteEntry(int id);
    }

    public class EntryRepository : IEntryRepository
    {
        private IEntryManageService _entryService;

        public EntryRepository(IEntryManageService entryService)
        {
            _entryService = entryService;
        }

        public Entry GetEntry(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                var entry = context.Entries
                    .Include(e => e.EntryCategory)
                    .Include(e => e.Account)
                    .Include(e => e.Paragraphs)
                    .SingleOrDefault(e => e.Id == id);

                return entry;
            }
        }

        public IEnumerable<Entry> GetEntries()
        {
            using (var context = new ApplicationDbContext())
            {
                var entries = context.Entries
                    .Include(e => e.EntryCategory)
                    .Include(e => e.Account)
                    .Include(e => e.Paragraphs)
                    .ToList();
                return entries;
            }
        }

        public void SaveEntry(Entry entry, string userIdentityName)
        {
            using (var context = new ApplicationDbContext())
            {
                var dbEntry = context.Entries
                    .Include(e => e.Account)
                    .Include(e => e.EntryCategory)
                    .SingleOrDefault(e => e.Id == entry.Id);

                var dbParagraphs = context.Paragraphs.Where(p => p.EntryId == entry.Id).ToList();

                var entryCategory = context.EntryCategories.SingleOrDefault(c => c.Id == entry.EntryCategory.Id);

                var account = context.Users
                    .Include(a => a.AccountType)
                    .SingleOrDefault(a => a.Email.Equals(userIdentityName));

                if (dbEntry == null)
                {
                    _entryService.PreSavingNewDataProcessing(account, entry, entryCategory);
                    context.Entries.Add(entry);
                }
                else
                {
                    _entryService.PreSavingModifiedDataProcessing(account, dbEntry, entryCategory, entry);

                    foreach (var p in dbParagraphs)
                        context.Paragraphs.Remove(p);

                    dbParagraphs.Clear();
                    dbParagraphs = dbEntry.Paragraphs;
                }

                context.SaveChanges();
            }
        }

        public DbRepoStatusCode PublishEntry(int entryId, string isActive, string userIdentityName)
        {
            using (var context = new ApplicationDbContext())
            {
                var entry = context.Entries.SingleOrDefault(e => e.Id == entryId);

                if (entry == null)
                    return DbRepoStatusCode.NotFound;

                var account = context.Users.SingleOrDefault(a => a.Email.Equals(userIdentityName));

                _entryService.ValidateEntry(entry, isActive, account);

                context.SaveChanges();

                return DbRepoStatusCode.Success;
            }
        }

        public DbRepoStatusCode DeleteEntry(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                var entry = context.Entries.SingleOrDefault(e => e.Id == id);

                if (entry == null)
                    return DbRepoStatusCode.NotFound;

                context.Entries.Remove(entry);
                context.SaveChanges();

                return DbRepoStatusCode.Success;
            }
        }

    }
}