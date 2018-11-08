using BlogManager.Models;
using BlogManager.Models.Galleries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogManager.Repositories
{
    public interface IDbRepository
    {
        int GetNewestGalleryId();
        int GetRecentCreatedGalleryIdByAccount(Gallery gallery);
    }
    public class DbRepository : IDbRepository
    {
        public int GetNewestGalleryId()
        {
            using (var context = new ApplicationDbContext())
            {
                int output = context.Database
                    .SqlQuery<int>(@"SELECT MAX(Id) FROM dbo.Galleries")
                    .ToList()
                    .First();
                return output;
            }
        }

        public int GetRecentCreatedGalleryIdByAccount(Gallery gallery)
        {
            using (var context = new ApplicationDbContext())
            {
                int output = context.Database
                    .SqlQuery<int>(string.Format(@"SELECT MAX(Id) FROM dbo.Galleries 
                                        WHERE Account_Id = {0}
                                        AND Title = '{1}'", gallery.Account.Id, gallery.Title))
                    .ToList()
                    .First();
                return output;
            }
        }
    }
}