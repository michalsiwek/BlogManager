using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogManager.Tests.Resources
{
    public static class Resources
    {
        public static readonly string Email = "test@bm.com";

        public static readonly string NewSuffix = "new";
        public static readonly string ModificationSuffix = "mod";

        public static readonly string NewEntryTitle = $"Test Title {NewSuffix}";
        public static readonly string NewEntryDescription = $"Test Description {NewSuffix}";
        public static readonly string NewEntryContent = $" \r\n\r\n\r\nTest \r\n\r\n\r\n Content \r\n\r\n\r\n {NewSuffix} \r\n\r\n\r\n";
        public static readonly string NewEntryImgUrl = $"https://www.testimgurl.com/{NewSuffix}";

        public static readonly string ModifiedEntryTitle = $"Test Title {ModificationSuffix}";
        public static readonly string ModifiedEntryDescription = $"Test Description {ModificationSuffix}";
        public static readonly string ModifiedEntryContent = $" Test \r\n\r\n\r\n Content \r\n\r\n\r\n {ModificationSuffix} ";
        public static readonly string ModifiedEntryImgUrl = $"https://www.testimgurl.com/{ModificationSuffix}";
    }
}