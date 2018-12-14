using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryData;
using LibraryData.Models;
using Microsoft.AspNetCore.Mvc;

namespace E_Library.Controllers
{
    public class BookController : Controller
    {
        private ILibraryAsset _libraryAsset;
        public BookController(ILibraryAsset libraryAsset, ICheckOut checkOut)
        {
            _libraryAsset = libraryAsset;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AssetList()
        {
            var result = _libraryAsset.Books();
            return View(result);
        }
        public IActionResult Add(int id)
        {
            return View();
        }
        [HttpPost]
        public IActionResult Add(Book libraryAsset)
        {

            var query = _libraryAsset.AddRecord(libraryAsset);
            if (query != null)

                return RedirectToAction("AssetList");

            return View(libraryAsset);
        }
        public IActionResult Update(int id)
        {
            var query = _libraryAsset.GetById(id);
            if (query == null)
            {
                return View();
            }
            else
            {
                return View(query);
            }
        }
        [HttpPost]
        public IActionResult Update(Book book)
        {
            _libraryAsset.Update(book);
            return RedirectToAction("AssetList");
        }
        public IActionResult Delete(int id)
        {
            var query = _libraryAsset.GetById(id);
            if (query != null)
            {
                return View(query);
            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        public IActionResult Delete(int? id, Book book)
        {
            _libraryAsset.Delete(book.Id);
            return RedirectToAction("AssetList");
        }
        public IActionResult BookDetail(int id)
        {
            var query = _libraryAsset.GetById(id);
            if (query == null)
                return View();
            else
                return View(query);

        }
    }
}

    