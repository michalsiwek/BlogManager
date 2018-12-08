using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using BlogManager.Infrastructure;
using NUnit.Framework;

namespace BlogManager.UnitTests.Infrastructure
{
    [TestFixture]
    public class FileSecurityValidatorTests
    {
        /*private Mock<HttpPostedFileBase> _file;
        private IFileSecurityValidator _fileSecurityValidator;

        [SetUp]
        public void SetUp()
        {
            _file = new Mock<HttpPostedFileBase>();
            _fileSecurityValidator = new FileSecurityValidator();
        }

        [Test]
        [TestCase("test.png", "image/", true)]
        [TestCase("test.jpeg", "image/", true)]
        [TestCase("test.jpg", "image/", true)]
        [TestCase("test.png", "file/", false)]
        [TestCase("test.jpeg", "file/", false)]
        [TestCase("test.jpg", "file/", false)]
        [TestCase("test.bmp", "image/", false)]
        [TestCase("test.gif", "image/", false)]
        [TestCase("test.webp", "image/", false)]
        public void FileSecurityValidation(string fileName, string contentType, bool validationResult)
        {
            _file.Setup(m => m.FileName).Returns(fileName);
            _file.Setup(m => m.ContentType).Returns(contentType);

            var result = _fileSecurityValidator.FileSecurityValidation(_file.Object);

            Assert.AreEqual(validationResult, result);
        }*/
    }
}
