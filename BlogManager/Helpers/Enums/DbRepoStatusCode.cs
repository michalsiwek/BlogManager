using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogManager.Helpers.Enums
{
    public enum DbRepoStatusCode
    {
        NotFound = 0,
        Success = 1,
        BadRequest = 2,
        Failed = 3,
        PartialSuccess = 4
    }
}