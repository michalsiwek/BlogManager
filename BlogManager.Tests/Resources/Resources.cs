using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogManager.Tests.Resources
{
    public static class Resources
    {
        public static string Email = "test@bm.com";

        public static string NewSuffix = "new";
        public static string ModificationSuffix = "mod";

        public static string NewEntryTitle = $"Test Title {NewSuffix}";
        public static string NewEntryDescription = $"Test Description {NewSuffix}";
        public static string NewEntryContent = $" \r\n\r\n\r\nTest \r\n\r\n\r\n Content \r\n\r\n\r\n {NewSuffix} \r\n\r\n\r\n";
        public static string NewEntryImgUrl = $"https://www.testimgurl.com/{NewSuffix}";

        public static string ModifiedEntryTitle = $"Test Title {ModificationSuffix}";
        public static string ModifiedEntryDescription = $"Test Description {ModificationSuffix}";
        public static string ModifiedEntryContent = $" Test \r\n\r\n\r\n Content \r\n\r\n\r\n {ModificationSuffix} ";
        public static string ModifiedEntryImgUrl = $"https://www.testimgurl.com/{ModificationSuffix}";
    }
}