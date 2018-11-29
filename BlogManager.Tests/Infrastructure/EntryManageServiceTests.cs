using System;
using System.Threading;
using BlogManager.Infrastructure;
using BlogManager.Models.Accounts;
using BlogManager.Models.Categories;
using BlogManager.Models.Entries;
using BlogManager.Tests.Resources;
using NUnit.Framework;

namespace BlogManager.UnitTests.Infrastructure
{
    [TestFixture]
    public class EntryManageServiceTests
    {
        private Entry _entry;
        private Account _account;
        private DateTime _testDate;
        private IEntryManageService _entryManageService;

        [SetUp]
        public void SetUp()
        {
            _entry = new Entry
            {
                Title = Resources.NewEntryTitle,
                Description = Resources.NewEntryDescription,
                Content = Resources.NewEntryContent,
                ImageUrl = Resources.NewEntryImgUrl
            };
            _account = new Account();
            _testDate = DateTime.Now;;
            _entryManageService = new EntryManageService();
        }

        [Test]
        [TestCase("true", true)]
        [TestCase("false", false)]
        public void EntryValidate(string request, bool isActive)
        {
            _entryManageService.ValidateEntry(_entry, request, _account);

            Assert.AreEqual(_entry.IsVisible, isActive);
        }

        [Test]
        public void PreSavingNewDataProcessing()
        {
            var entryCategory = new EntryCategory();

            Thread.Sleep(1000);
            _entryManageService.PreSavingNewDataProcessing(_account, _entry, entryCategory);

            Assert.AreEqual(_entry.Account, _account);
            Assert.Greater(_entry.CreateDate, _testDate);
            Assert.IsNull(_entry.LastModification);
            Assert.False(_entry.IsVisible);
            Assert.IsNotEmpty(_entry.Paragraphs);
        }

        [Test]
        public void PreSavingModifiedDataProcessing()
        {
            _entry.CreateDate = _testDate;
            var entryCategory = new EntryCategory();
            var existingEntry = new Entry
            {
                Title = Resources.ModifiedEntryTitle,
                Description = Resources.ModifiedEntryDescription,
                Content = Resources.ModifiedEntryContent,
                ImageUrl = Resources.ModifiedEntryImgUrl
            };

            Thread.Sleep(1000);
            _entryManageService.PreSavingModifiedDataProcessing(_account, _entry, entryCategory, existingEntry);

            Assert.AreEqual(_entry.LastModifiedBy, _account);
            Assert.Greater(_entry.LastModification, _testDate);
            Assert.IsNotEmpty(_entry.Paragraphs);
            StringAssert.Contains(Resources.ModificationSuffix, _entry.Title);
            StringAssert.Contains(Resources.ModificationSuffix, _entry.Description);
            StringAssert.Contains(Resources.ModificationSuffix, _entry.Content);
            StringAssert.Contains(Resources.ModificationSuffix, _entry.ImageUrl);
        }
    }
}
