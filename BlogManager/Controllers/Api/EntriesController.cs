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
            var entryQuery = _context.Entries
                .Include(e => e.EntryCategory)
                .Where(e => e.IsVisible == true);

            if(!String.IsNullOrWhiteSpace(query))
                entryQuery = entryQuery.Where(e => e.Title.Contains(query));

            var entryDtos = entryQuery.ToList().Select(Mapper.Map<Entry, EntryDto>);

            return Ok(entryDtos);
        }
    }
}
