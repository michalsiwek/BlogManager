using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlogManager.Infrastructure;
using BlogManager.Models.Accounts;
using NUnit.Framework;

namespace BlogManager.UnitTests.Infrastructure
{
    [TestFixture]
    public class AccountManageServiceTests
    {
        private Account _account;
        private IAccountManageService _accountManageService;

        [SetUp]
        public void SetUp()
        {
            _account = new Account();
            _accountManageService = new AccountManageService();
        }

        [Test]
        [TestCase("true", true)]
        [TestCase("false", false)]
        public void AccountValidate(string request, bool isActive)
        {
            _accountManageService.ValidateAccount(_account, request);

            Assert.AreEqual(_account.IsActive, isActive);
        }
    }
}
