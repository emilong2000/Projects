using E_Library.ModelViews.Patron;
using LibraryData.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_Library.Controllers
{
    public class PatronController : Controller
    {
        private IPatron _patron;
        public PatronController(IPatron patron)
        {
            _patron = patron;
        }
        public IActionResult Index()
        {
            var AllPatron = _patron.GetAll();

            var patronModel = AllPatron.Select(p => new PatronDetaiilModel
            {
                Id = p.Id,
                FirstName = p.FirstName,
                LastName = p.LastName,
                LibraryCardId = p.LibraryCard.Id,
                OverdueFees = p.LibraryCard.Fees,
                HomeLibraryBranch = p.HomeLibraryBranch.Name
            }).ToList();
            var model = new PatronIndexModel()
            {
                Patrons = patronModel
            };
            return View(model);
        }
        public IActionResult Detail(int Id)
        {
            var patron = _patron.Get(Id);
            var model = new PatronDetaiilModel
            {
                FirstName = patron.FirstName,
                LastName = patron.LastName,
                Address = patron.Address,
                HomeLibraryBranch = patron.HomeLibraryBranch.Name,
                MemberSince = patron.LibraryCard.Created,
                OverdueFees = patron.LibraryCard.Fees,
                LibraryCardId = patron.LibraryCard.Id,
                PhoneNo = patron.PhoneNo,
                AssetsCheckedOut = _patron.GetCheckouts(Id).ToList() ?? new List<Checkout>(),
                CheckOutHistories = _patron.GetCheckOutHistories(Id).ToList()?? new List<CheckOutHistory>(),
                Holds = _patron.GetHolds(Id)
            };
            return View(model);
        }
    }
}
