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

        [HttpGet]
        public IHttpActionResult GetEntries(string query = null)
        {
            var entriesQuery = _context.Entries
                .Include(e => e.Account)
                .Include(e => e.ContentCategory)
                .Include(e => e.Paragraphs)
                .Where(e => e.IsVisible == true && e.ContentCategory.IsActive == true);

            if(!String.IsNullOrWhiteSpace(query))
                entriesQuery = entriesQuery.Where(e => e.ContentCategory.Name
                                            .Replace(" ", "_").ToLower().Equals(query));

            return Ok(entriesQuery.ToList().Select(Mapper.Map<Entry, EntryDto>));
        }

        [HttpGet]
        public IHttpActionResult GetEntry(int id)
        {
            var entry = _context.Entries
                .Include(e => e.Account)
                .Include(e => e.ContentCategory)
                .Include(e => e.Paragraphs)
                .SingleOrDefault(e => e.Id == id && e.IsVisible == true && e.ContentCategory.IsActive== true);

            if (entry == null)
                return NotFound();

            return Ok(Mapper.Map<Entry, EntryDto>(entry));
        }

        /*[HttpDelete]
        [Authorize]
        public IHttpActionResult DeleteEntry(int id)
        {
            var entryFromDb = _context.Entries.SingleOrDefault(e => e.Id == id);

            if (entryFromDb == null)
                return NotFound();

            _context.Entries.Remove(entryFromDb);
            _context.SaveChanges();

            return Ok();
        }*/
    }
}