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
    public class ContentCategoryManageServiceTests
    {
        private IContentCategoryManageService _contentCategoryManageService;
        private ContentCategory _contentCategory;

        [SetUp]
        public void SetUp()
        {
            _contentCategoryManageService = new ContentCategoryManageService();
            _contentCategory = new ContentCategory();
        }

        [Test]
        [TestCase("true", true)]
        [TestCase("false", false)]
        public void ActivateContentCategory(string request, bool isActive)
        {
            _contentCategoryManageService.ActivateContentCategory(_contentCategory, request);

            Assert.AreEqual(_contentCategory.IsActive, isActive);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void PreSavingDataProcessing(bool isNew)
        {
            var testDate = DateTime.Now;
            if (!isNew)
                _contentCategory.CreateDate = testDate;

            Thread.Sleep(1000);
            _contentCategoryManageService.PreSavingDataProcessing(_contentCategory);

            switch (isNew)
            {
                case true:
                    Assert.Greater(_contentCategory.CreateDate, testDate);
                    Assert.IsNull(_contentCategory.LastModification);
                    break;
                case false:
                    Assert.AreEqual(_contentCategory.CreateDate, testDate);
                    Assert.Greater(_contentCategory.LastModification, testDate);
                    break;
            }
            
        }
    }
}
