using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BlogManager.Infrastructure;
using BlogManager.Models.Categories;
using NUnit.Framework;

namespace BlogManager.UnitTests.Infrastructure
{
    [TestFixture]
    public class EntryCategoryManageServiceTests
    {
        private IEntryCategoryManageService _entryCategoryManageService;
        private EntryCategory _entryCategory;

        [SetUp]
        public void SetUp()
        {
            _entryCategoryManageService = new EntryCategoryManageService();
            _entryCategory = new EntryCategory();
        }

        [Test]
        [TestCase("true", true)]
        [TestCase("false", false)]
        public void ActivateEntryCategory(string request, bool isActive)
        {
            _entryCategoryManageService.ActivateEntryCategory(_entryCategory, request);

            Assert.AreEqual(_entryCategory.IsActive, isActive);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void PreSavingDataProcessing(bool isNew)
        {
            var testDate = DateTime.Now;
            if (!isNew)
                _entryCategory.CreateDate = testDate;

            Thread.Sleep(1000);
            _entryCategoryManageService.PreSavingDataProcessing(_entryCategory);

            switch (isNew)
            {
                case true:
                    Assert.Greater(_entryCategory.CreateDate, testDate);
                    Assert.IsNull(_entryCategory.LastModification);
                    break;
                case false:
                    Assert.AreEqual(_entryCategory.CreateDate, testDate);
                    Assert.Greater(_entryCategory.LastModification, testDate);
                    break;
            }
            
        }
    }
}
