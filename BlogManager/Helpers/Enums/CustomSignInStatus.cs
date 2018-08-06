using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogManager.Helpers.Enums
{
    public enum CustomSignInStatus
    {
        Success = 0,
        LockedOut = 1,
        RequiresVerification = 2,
        Failure = 3,
        NeedToBeActivate = 4,
        UnknownError = 5
    }
}