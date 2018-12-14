using LibraryData;
using LibraryData.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibraryServices
{
    public class Patronservice : IPatron
    {
        private LibraryContext _context;
        
        public Patronservice(LibraryContext context)
        {
            _context = context;
        }
        public void Add(Patron newPatron)
        {
            _context.Add(newPatron);
            _context.SaveChanges();
        }

        public Patron Get(int id)
        {
            /*
            return _context.Patrons 
                .Include(p => p.LibraryCard)
                .Include(p => p.HomeLibraryBranch)
                .FirstOrDefault(p => p.Id == id);
                */

            return GetAll()
                .FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<Patron> GetAll()
        {
            return _context.Patrons
                .Include(p => p.LibraryCard)
                .Include(p => p.HomeLibraryBranch);
        }

        public IEnumerable<CheckOutHistory> GetCheckOutHistories(int patronId)
        {
            /*
            var cardId = _context.Patrons
                .Include(patron => patron.LibraryCard)
                .FirstOrDefault(patron => patron.Id == patronId)
                .LibraryCard.Id;
               */
            var cardId = Get(patronId).LibraryCard.Id;

            return _context.CheckOutHistories
                .Include(k => k.LibraryCard)
                .Include(k => k.LibraryAsset)
                .Where(k => k.LibraryCard.Id == cardId)
                .OrderByDescending(k => k.CheckedOut);
        }

        public IEnumerable<Checkout> GetCheckouts(int patronId)
        {
            /*
            var cardId = _context.Patrons
                .Include(patron => patron.LibraryCard)
                .FirstOrDefault(patron => patron.Id == patronId)
                .LibraryCard.Id;
              */
            var cardId = Get(patronId).LibraryCard.Id;

            return _context.CheckOuts
            .Include(c => c.LibraryAsset)
            .Include(c => c.LibraryCard)
            .Where(c => c.LibraryCard.Id == cardId);
        }

        public IEnumerable<Hold> GetHolds(int patronId)
        {
            var cardId = Get(patronId).LibraryCard.Id;

            return _context.Holds
                .Include(p => p.LibraryAsset)
                .Include(p => p.LibraryCard)
                .Where(p => p.LibraryCard.Id == cardId)
                .OrderByDescending(p => p.HoldPlaced);
        }
    }
}
