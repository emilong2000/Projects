using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryData;
using LibraryData.Models;
using Microsoft.AspNetCore.Mvc;

namespace E_Library.Controllers
{
    public class VideoController : Controller
    {
        private ILibraryAsset _libraryAsset;
        public VideoController(ILibraryAsset libraryAsset)
        {
            _libraryAsset = libraryAsset;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult VideoList()
        {
          var result =   _libraryAsset.videos();
            return View(result);
        }
        public IActionResult Add(int id)
        {
            return View();

        }
        [HttpPost]
        public IActionResult Add(Video video)
        {
            _libraryAsset.AddVideo(video);
            return RedirectToAction("VideoList");
        }
        public IActionResult Update(int id)
        {
            var query = _libraryAsset.GetById(id);
            if (query == null)
            {
                return View();
            }
            return View(query);
        }
        [HttpPost]
        public IActionResult Update(Video video)
        {
            var query = _libraryAsset.UpdateVideo(video);
            return RedirectToAction("VideoList");
        }
        public IActionResult Delete(int id)
        {
            var result = _libraryAsset.GetById(id);
            if(result == null)
            {
                return View();
            }
            return View(result);
        }
        [HttpPost]
        public IActionResult Delete(int? id, Video video)
        {
            _libraryAsset.DeleteVideo(video.Id);
            return RedirectToAction("VideoList");
        }
        public IActionResult Detail(int id)
        {
            var query = _libraryAsset.GetById(id);
            if (query == null)
                return View();
            else
                return View(query);
        }
    }
}