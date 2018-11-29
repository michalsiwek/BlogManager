using BlogManager.Models;
using BlogManager.Models.Entries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogManager.Repositories
{
    public interface IParagraphRepository
    {
        List<Paragraph> GetParagraphsByEntryId(int entryId);
    }

    public class ParagraphRepository : IParagraphRepository
    {
        public List<Paragraph> GetParagraphsByEntryId(int entryId)
        {
            using (var context = new ApplicationDbContext())
            {
                var paragraphs = context.Paragraphs.Where(p => p.EntryId == entryId).ToList();
                return paragraphs;
            }
        }
    }
}