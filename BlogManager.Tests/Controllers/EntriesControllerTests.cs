using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using BlogManager.Controllers;
using BlogManager.Models.Entries;
using NUnit.Framework;

namespace BlogManager.Tests.Controllers
{
    [TestFixture]
    public class EntriesControllerTests
    {
        [Test]
        public void Index(string userIdentityName)
        {
            // Arrange
            var controller = new EntriesController();

            // Act
            var result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

    }
}
