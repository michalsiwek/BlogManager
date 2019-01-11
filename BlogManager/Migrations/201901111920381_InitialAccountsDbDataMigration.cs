namespace BlogManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialAccountsDbDataMigration : DbMigration
    {
        public override void Up()
        {
            Sql(@"BEGIN TRANSACTION [UserInitialData]

                SET IDENTITY_INSERT [dbo].[AspNetRoles] ON
                INSERT INTO [dbo].[AspNetRoles] ([Id], [Description], [CreateDate], [LastModification], [Name]) VALUES (1, N'Admin Access', N'2019-01-01 12:00:00', NULL, N'Admin')
                INSERT INTO [dbo].[AspNetRoles] ([Id], [Description], [CreateDate], [LastModification], [Name]) VALUES (2, N'Editor Access', N'2019-01-01 12:00:00', NULL, N'Editor')
                INSERT INTO [dbo].[AspNetRoles] ([Id], [Description], [CreateDate], [LastModification], [Name]) VALUES (3, N'Guest Access', N'2019-01-01 12:00:00', NULL, N'Guest')
                SET IDENTITY_INSERT [dbo].[AspNetRoles] OFF

                SET IDENTITY_INSERT [dbo].[AspNetUsers] ON
                INSERT INTO [dbo].[AspNetUsers] ([Id], [CreateDate], [LastModification], [FirstName], [LastName], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName], [AccountType_Id], [IsActive], [Nickname], [PasswordRecoveryAnswer], [PasswordRecoveryQuestion_Id])
                VALUES (1, N'2019-01-01 12:00:00', NULL, NULL, NULL, N'admin@bm.com', 0, N'APVMatQqIGNaaeMCra+JaA/9vMVeJpN9uo02OgQP+wH6BXDrX7PlvK+SDjConOBwCw==', N'd88d6ec5-ffde-4aa3-9119-06bb6507b85e', NULL, 0, 0, NULL, 0, 0, N'admin@bm.com', 1, 1, N'Admin', N'Answer', NULL)
                SET IDENTITY_INSERT [dbo].[AspNetUsers] OFF

                INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (1, 1)

                COMMIT TRANSACTION [UserInitialData]");
        }
        
        public override void Down()
        {
        }
    }
}
