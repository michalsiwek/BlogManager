using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlogManager.Models.Entries;
using BlogManager.Tests.Resources;
using NUnit.Framework;

namespace BlogManager.UnitTests.Models
{
    [TestFixture]
    public class EntryTests
    {
        private Entry _entry;

        [SetUp]
        public void SetUp()
        {
            _entry = new Entry
            {
                Title = Resources.NewEntryTitle,
                Description = Resources.NewEntryDescription,
                Content = Resources.NewEntryContent
            };
        }

        [Test]
        public void NormalizeContent()
        {
            _entry.NormalizeContent();

            Assert.False(_entry.Content.StartsWith(" "));
            Assert.False(_entry.Content.EndsWith(" "));
            Assert.False(_entry.Content.Contains("\r\n\r\n\r\n"));
        }

        [Test]
        public void GetParagraphsFromContent()
        {
            _entry.GetParagraphsFromContent();

            Assert.AreEqual(3, _entry.Paragraphs.Count);
        }
    }
}
