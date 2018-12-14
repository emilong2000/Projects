using LibraryData;
using LibraryData.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibraryServices
{
    public class LibraryBranchService : ILibraryBranch
    {
        private LibraryContext _context;

        public LibraryBranchService(LibraryContext context)
        {
            _context = context;
        }
        public void Add(LibraryBranch newBranch)
        {
            _context.Add(newBranch);
            _context.SaveChanges();
        }

        public LibraryBranch GetBy(int branchId)
        {
           return
                /*
                _context.LibraryBranches
                .Include(p => p.Patrons)
                .Include(p => p.LibraryAssets)
                */
                GetAll()
                .FirstOrDefault(P => P.Id == branchId);
        }

        public IEnumerable<LibraryBranch> GetAll()
        {
            return
                _context.LibraryBranches
                .Include(p => p.Patrons)
                .Include(p => p.LibraryAssets);
                
        }

        public IEnumerable<LibraryAsset> GetAssets(int BranchId)
        {
            return
                _context.LibraryBranches
                .Include(p => p.LibraryAssets)
                .FirstOrDefault(p => p.Id == BranchId).LibraryAssets;
        }

        public IEnumerable<string> GetBranchHours(int BranchId)
        {
            var hour = _context.BranchHours.Where(b => b.branch.Id == BranchId);
                return DataHelpers.HumanizeBizHours(hour);
        }

        public IEnumerable<Patron> GetPatrons(int BranchId)
        {
            return
                 _context.LibraryBranches
                 .Include(p => p.Patrons)
                 .FirstOrDefault(p => p.Id == BranchId)
                 .Patrons;
        }

        public bool IsBranchOpen(int BranchId)
        {
            var currentTimeHour = DateTime.Now.Hour;
            var currentDayOfWeek = (int)DateTime.Now.DayOfWeek + 1;
            var hour = _context.BranchHours.Where(b => b.branch.Id == BranchId);
            var daysHour = hour.FirstOrDefault(p => p.DayOfWeek == currentDayOfWeek);

            return  currentTimeHour < daysHour.CloseTime && currentTimeHour > daysHour.OpenTime;
            
        }
    }
}
