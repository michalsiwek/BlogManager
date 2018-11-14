using BlogManager.Models;
using BlogManager.Models.Accounts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BlogManager.Helpers.Enums;
using BlogManager.Infrastructure;
using Microsoft.AspNet.Identity;

namespace BlogManager.Repositories
{
    public interface IAccountRepository
    {
        Account GetSignedUser(string userIdentityName);
        Account GetAccountById(int id);
        Account GetAccountByEmail(string email);
        IEnumerable<Account> GetAllAccountsButAdmin();
        DbRepoStatusCode ValidateAccount(int accountId, string isActive);
        DbRepoStatusCode DeleteAccount(int accountId);
        IEnumerable<PasswordRecoveryQuestion> GetPasswordRecoveryQuestions();
        PasswordRecoveryQuestion GetPasswordRecoveryQuestionById(int id);
        DbRepoStatusCode AssignPasswordRecoveryQuestion(string email, int passRecoveryId);
        DbRepoStatusCode VerifyCode(int accountId, string code);
        DbRepoStatusCode SendVerificationCode(string email);
        DbRepoStatusCode SaveAccountChanges(Account account, ApplicationUserManager usermanager);
    }

    public class AccountRepository : IAccountRepository
    {
        private IAccountManageService _accountService;
        private IMailingService _emailService;

        public AccountRepository()
        {
            _accountService = new AccountManageService();
            _emailService = new MailingService();
        }

        public Account GetSignedUser(string userIdentityName)
        {
            using (var context = new ApplicationDbContext())
            {
                return context.Users
                    .Include(u => u.AccountType)
                    .SingleOrDefault(u => u.Email.Equals(userIdentityName));
            }
        }

        public IEnumerable<Account> GetAllAccountsButAdmin()
        {
            using (var context = new ApplicationDbContext())
            {
                var result = context.Users
                    .Include(u => u.AccountType)
                    .Where(u => u.Id != 1)
                    .ToList();
                return result;
            }
        }

        public Account GetAccountById(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                var result = context.Users
                    .Include(u => u.AccountType)
                    .SingleOrDefault(u => u.Id == id);
                return result;
            }
        }

        public Account GetAccountByEmail(string email)
        {
            using (var context = new ApplicationDbContext())
            {
                var result = context.Users
                    .Include(u => u.AccountType)
                    .Include(u => u.PasswordRecoveryQuestion)
                    .SingleOrDefault(u => u.Email.Equals(email));
                return result;
            }
        }

        public DbRepoStatusCode ValidateAccount(int accountId, string isActive)
        {
            using (var context = new ApplicationDbContext())
            {
                var account = context.Users
                    .Include(u => u.AccountType)
                    .SingleOrDefault(u => u.Id == accountId);

                if (account == null)
                    return DbRepoStatusCode.NotFound;

                if (account.AccountType == null)
                    return DbRepoStatusCode.BadRequest;

                _accountService.ValidateAccount(account, isActive);

                context.SaveChanges();

                return DbRepoStatusCode.Success;
            }
        }

        public DbRepoStatusCode DeleteAccount(int accountId)
        {
            using (var context = new ApplicationDbContext())
            {
                var account = context.Users.SingleOrDefault(u => u.Id == accountId);

                if (account == null)
                    return DbRepoStatusCode.NotFound;

                context.Users.Remove(account);
                context.SaveChanges();

                return DbRepoStatusCode.Success;
            }
        }

        public IEnumerable<PasswordRecoveryQuestion> GetPasswordRecoveryQuestions()
        {
            using (var context = new ApplicationDbContext())
            {
                var output = context.PasswordRecoveryQuestions.ToList();
                return output;
            }
        }

        public PasswordRecoveryQuestion GetPasswordRecoveryQuestionById(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                var output = context.PasswordRecoveryQuestions.SingleOrDefault(q => q.Id == id);
                return output;
            }
        }

        public DbRepoStatusCode AssignPasswordRecoveryQuestion(string email, int passRecoveryId)
        {
            using (var context = new ApplicationDbContext())
            {
                var account = context.Users.SingleOrDefault(a => a.Email.Equals(email));
                if (account == null)
                    return DbRepoStatusCode.NotFound;

                var question = context.PasswordRecoveryQuestions
                    .SingleOrDefault(p => p.Id == passRecoveryId);
                if (question == null)
                    return DbRepoStatusCode.NotFound;

                account.PasswordRecoveryQuestion = question;
                context.SaveChanges();

                return DbRepoStatusCode.Success;
            }
        }

        public DbRepoStatusCode VerifyCode(int accountId, string code)
        {
            using (var context = new ApplicationDbContext())
            {
                var dbCode = context.PasswordResetVerificationCodes
                .SingleOrDefault(c => c.Account.Id == accountId && c.IsActive && c.ExpirationDate > DateTime.Now);

                if (dbCode == null)
                    return DbRepoStatusCode.NotFound;

                if (code.Equals(dbCode.Code))
                {
                    dbCode.IsActive = false;
                    context.SaveChanges();

                    return DbRepoStatusCode.Success;
                }

                return DbRepoStatusCode.Failed;                
            }
        }

        public DbRepoStatusCode SendVerificationCode(string email)
        {
            using (var context = new ApplicationDbContext())
            {
                var account = context.Users
                .Include(a => a.PasswordRecoveryQuestion)
                .SingleOrDefault(a => a.Email.Equals(email));

                if (account == null)
                    return DbRepoStatusCode.NotFound;

                var verificationCode = context.PasswordResetVerificationCodes
                    .Include(c => c.Account)
                    .SingleOrDefault(c => c.Account.Id == account.Id && c.IsActive && c.ExpirationDate > DateTime.Now);

                if (verificationCode == null)
                {
                    verificationCode = new PasswordResetVerificationCode
                    {
                        Account = account,
                        Code = new Random().Next(10000, 99999).ToString(),
                        ExpirationDate = DateTime.Now.AddMinutes(15),
                        IsActive = true
                    };

                    context.PasswordResetVerificationCodes.Add(verificationCode);
                    context.SaveChanges();

                    _emailService.SendVerificationCode(email, verificationCode.Code);

                    return DbRepoStatusCode.Success;
                }

                return DbRepoStatusCode.Failed;
            }
        }

        public DbRepoStatusCode SaveAccountChanges(Account account, ApplicationUserManager userManager)
        {
            using (var context = new ApplicationDbContext())
            {
                var dbAccount = context.Users.SingleOrDefault(u => u.Id == account.Id);

                if (dbAccount == null)
                    return DbRepoStatusCode.NotFound;

                dbAccount.IsActive = account.IsActive;
                dbAccount.AccountType = context.Roles.SingleOrDefault(r => r.Id == account.AccountType.Id);

                userManager.AddToRole(dbAccount.Id, dbAccount.AccountType.Name);

                context.SaveChanges();

                return DbRepoStatusCode.Success;
            }
        }

    }
}