﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
            Assert.AreEqual(_entry.LastModifiedBy, _account);
            Assert.GreaterOrEqual(_entry.LastModification, _testDate);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void PreSavingDataProcessing(bool isNew)
        {
            var entryCategory = new EntryCategory();
            var existingEntry = new Entry
            {
                Title = Resources.ModifiedEntryTitle,
                Description = Resources.ModifiedEntryDescription,
                Content = Resources.ModifiedEntryContent,
                ImageUrl = Resources.ModifiedEntryImgUrl
            };

            Thread.Sleep(1000);
            if(isNew)
                _entryManageService.PreSavingDataProcessing(_account, _entry, entryCategory);
            else
            {
                _entry.CreateDate = _testDate;
                _entryManageService.PreSavingDataProcessing(_account, _entry, entryCategory, existingEntry);
            }

            switch (isNew)
            {
                case true:
                    Assert.AreEqual(_entry.Account, _account);
                    Assert.Greater(_entry.CreateDate, _testDate);
                    Assert.IsNull(_entry.LastModification);
                    Assert.False(_entry.IsVisible);
                    Assert.IsNotEmpty(_entry.Paragraphs);
                    break;
                case false:
                    Assert.AreEqual(_entry.LastModifiedBy, _account);
                    Assert.Greater(_entry.LastModification, _testDate);
                    Assert.IsNotEmpty(_entry.Paragraphs);
                    StringAssert.Contains(Resources.ModificationSuffix, _entry.Title);
                    StringAssert.Contains(Resources.ModificationSuffix, _entry.Description);
                    StringAssert.Contains(Resources.ModificationSuffix, _entry.Content);
                    StringAssert.Contains(Resources.ModificationSuffix, _entry.ImageUrl);
                    break;
            }

        }
    }
}
