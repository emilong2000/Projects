using E_Library.ModelViews.Catalog;
using E_Library.ModelViews.Catalog.CheckOut;
using E_Library.ModelViews.CataLog;
using LibraryData;
using LibraryData.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_Library.Controllers
{
    public class CatalogController : Controller
    {
        
        private ILibraryAsset _libraryAsset;
        private ICheckOut _checkOut;                                                          
        public CatalogController(ILibraryAsset libraryAsset, ICheckOut checkOut)
        {
            _libraryAsset = libraryAsset;
            _checkOut = checkOut;
        }
        public IActionResult Index()
        {
            var assetModel = _libraryAsset.GetAll();

            var indexListing = assetModel
            .Select(result => new AssetIndexListingModel
            {
                Id = result.Id,
                ImageUrl = result.ImageUrl,
                AuthorOrDirector = _libraryAsset.GetAuthorOrDirector(result.Id),
                DeweyCallNumber = _libraryAsset.GetDeweyIndex(result.Id),
                Title = result.Title,
                Type = _libraryAsset.GetType(result.Id)
            });

            var Model = new AssetViewModel()
            {
                Assets = indexListing
            };
            return View(Model);
        }
        public IActionResult Detail(int id)
        {
            var asset = _libraryAsset.GetById(id);
            var currentHolds = _checkOut.GetCurrentHolds(id)
                .Select(a => new AssetHoldModel
                {
                    HoldPlaced = _checkOut.GetCurrentHoldPlaced(a.Id).ToString("d"),
                    PatronName = _checkOut.GetCurrentHoldPatronName(a.Id)

                });

            var model = new AssetDetailModel()
            {
                AssetId = id,
                Title = asset.Title,
                Year = asset.Year,
                Cost = asset.Cost,
                Status = asset.Status.Name,
                ImageUrl = asset.ImageUrl,
                AuthorOrDirector = _libraryAsset.GetAuthorOrDirector(id),
                CurrentLocation = _libraryAsset.GetCurrentLocation(id).Name,
                DeweyCallNumber = _libraryAsset.GetDeweyIndex(id),
                ISBN = _libraryAsset.GetISBN(id),
                LatestCheckout = _checkOut.GetLatestCheckOut(id),
                CheckoutHistory = _checkOut.GetCheckOutHistory(id),
                PatronName = _checkOut.GetCurrentHoldPatronName(id),
                CurrentHolds = currentHolds

            };
            return View(model);
        }
        public IActionResult CheckOut(int id)
        {
            var asset = _libraryAsset.GetById(id);
            var model = new CheckOutModel()
            {
                AssetId = id,
                ImageUrl = asset.ImageUrl,
                Title = asset.Title,
                LibraryCardId = "",
                IsCheckedOut = _checkOut.IsCheckedOut(id)
            };
            return View(model);
        }
        public IActionResult CheckIn(int id)
        {
            _checkOut.CheckInItem(id);
            return RedirectToAction("Detail", new { Id = id });
        }
       
        public IActionResult Hold(int id)
        {
            var asset = _libraryAsset.GetById(id);
            var model = new CheckOutModel
            {
                AssetId = id,
                ImageUrl = asset.ImageUrl,
                Title = asset.Title,
                LibraryCardId = "",
                IsCheckedOut = _checkOut.IsCheckedOut(id),
                HoldCount = _checkOut.GetCurrentHolds(id).Count()
            };
            return View(model);
        }
        public IActionResult MarkLost(int assetId)
        {
            _checkOut.MarkLost(assetId);
            return RedirectToAction("Detail", new { id = assetId });
        }
        public IActionResult MarkFound(int assetId)
        {
            _checkOut.MarkFound(assetId);
            return RedirectToAction("Detail", new { id = assetId });
        }

        [HttpPost]
        public IActionResult PlaceCheckout(int AssetId, int LibraryCardId)
        {
            _checkOut.CheckOutItem(AssetId, LibraryCardId);
            return RedirectToAction("Detail", new { id = AssetId });
        }
        [HttpPost]
        public IActionResult PlaceHold(int AssetId, int LibraryCardId)
        {
            _checkOut.PlaceHold(AssetId, LibraryCardId);
            return RedirectToAction("Detail", new { id = AssetId });
        }
    }
}
