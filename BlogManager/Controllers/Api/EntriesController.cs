using BlogManager.Dtos;
using BlogManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Data.Entity;
using AutoMapper;
using BlogManager.Models.Entries;

namespace BlogManager.Controllers.Api
{
    public class EntriesController : ApiController
    {
        private ApplicationDbContext _context;

        public EntriesController()
        {
            _context = new ApplicationDbContext();
        }

        public IEnumerable<EntryDto> GetEntries(string query = null)
        {
            var entriesQuery = _context.Entries
                .Include(e => e.Account)
                .Include(e => e.EntryCategory)
                .Include(e => e.Paragraphs)
                .Where(e => e.IsVisible == true);

            if(!String.IsNullOrWhiteSpace(query))
                entriesQuery = entriesQuery.Where(e => e.EntryCategory.Name.Replace(" ", "_").ToLower().Equals(query));

            return entriesQuery.ToList().Select(Mapper.Map<Entry, EntryDto>);
        }

        public IHttpActionResult GetEntry(int id)
        {
            var entry = _context.Entries
                .Include(e => e.Account)
                .Include(e => e.EntryCategory)
                .Include(e => e.Paragraphs)
                .SingleOrDefault(e => e.Id == id && e.IsVisible == true);

            if (entry == null)
                return NotFound();

            return Ok(Mapper.Map<Entry, EntryDto>(entry));
        }
    }
}
