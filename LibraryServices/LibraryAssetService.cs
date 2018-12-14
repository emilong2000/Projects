using LibraryData;
using LibraryData.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LibraryServices
{
    public class LibraryAssetService : ILibraryAsset
    {
        private LibraryContext _libraryContext;
        public LibraryAssetService(LibraryContext libraryContext)
        {
            _libraryContext = libraryContext;
        }
        public void AddAsset(LibraryAsset newAsset)
        {
            _libraryContext.LibraryAssets.Add(newAsset);
            _libraryContext.SaveChanges();
           
        }

        public IEnumerable<LibraryAsset> GetAll()
        {
            return _libraryContext.LibraryAssets
                .Include(asset => asset.Status)
                .Include(asset => asset.Location);
        }

        public LibraryAsset GetById(int id)
        {
            /*
            return _libraryContext.LibraryAssets
                 .Include(asset => asset.status)
                 .Include(asset => asset.Location)
                 .FirstOrDefault(asset => asset.AssetId == id);
                 */
                return   
                GetAll().FirstOrDefault(asset => asset.Id == id);
        }

        public LibraryBranch GetCurrentLocation(int id)
        {
            //return _libraryContext.LibraryAssets.FirstOrDefault(asset => asset.AssetId == id).Location;
            return GetById(id).Location;
        }

        public string GetDeweyIndex(int id)
        {
            if (_libraryContext.Books.Any(book => book.Id == id))
            {
                return _libraryContext.Books.FirstOrDefault(book => book.Id == id).DeweyIndex;
            }
            else return "";
        }

        public string GetISBN(int id)
        {
            if (_libraryContext.Books.Any(book => book.Id == id))
            {
                return _libraryContext.Books.FirstOrDefault(book => book.Id == id).ISBN;
            }
            else return "";
        }

        public string GetTitle(int id)
        {
            return _libraryContext.LibraryAssets.FirstOrDefault(a => a.Id == id).Title;
        }

        public string GetType(int id)
        {
            var book = _libraryContext.LibraryAssets.OfType<Book>().Where(b => b.Id == id);
            return book.Any() ? "Book" : "Video";
        }
        public string GetAuthorOrDirector(int id)
        {
            var IsBook = _libraryContext.LibraryAssets.OfType<Book>()
                .Where(a => a.Id == id).Any();
            var IsVideo = _libraryContext.LibraryAssets.OfType<Video>()
               .Where(a => a.Id == id).Any();

            return IsBook ?
                _libraryContext.Books.FirstOrDefault(book => book.Id == id).Author :
                _libraryContext.Videos.FirstOrDefault(video => video.Id == id).Director
                ?? "Unknown";
        }

       

        public LibraryAsset AddRecord(Book libraryAsset)
        {
            _libraryContext.LibraryAssets.Add(libraryAsset);
            _libraryContext.SaveChanges();
            return libraryAsset;
        }

        public IEnumerable<LibraryAsset> LibraryAssets()
        {
            return _libraryContext.LibraryAssets.ToList();
        }

        public bool Update(Book book)
        {
            try
            {
                var query = _libraryContext.Books
                       .Where(p => p.Id == book.Id)
                       .SingleOrDefault();
                if (query != null)
                {
                    query.Title = book.Title;
                    query.Year = book.Year;
                    query.Status = book.Status;
                    query.Cost = book.Cost;
                    query.ImageUrl = book.ImageUrl;
                    query.NumberOfCopies = book.NumberOfCopies;
                    query.Location = book.Location;
                    query.ISBN = book.ISBN;
                    query.DeweyIndex = book.DeweyIndex;
                    query.Author = book.Author;

                    _libraryContext.Entry(book).State = EntityState.Detached;
                    _libraryContext.SaveChanges();

                }
                return true;
            }
            catch (Exception Ex)
            {

                string log = Ex.Message;
            }
            return false;
        }
        public int Delete(int id)
        {
            try
            {
                var query = _libraryContext.Books
                      .Where(c => c.Id == id)
                      .FirstOrDefault();
                if (query != null)
                {
                    _libraryContext.Books.Remove(query);
                    _libraryContext.SaveChanges();
                    return 1;
                }
                
            }
            catch (Exception ex)
            {
                string log = ex.Message;
            }
             return 0;
        }

        public int BookDetail(int id)
        {
            var query = _libraryContext.Books.Where(p => p.Id == id).SingleOrDefault();
            if (query != null)
                return 1;
            else
                return 0;
        }

        public LibraryAsset AddVideo(Video video)
        {
             _libraryContext.Videos.Add(video);
            _libraryContext.SaveChanges();
            return video;
        }

        public bool UpdateVideo(Video video)
        {
            var query = _libraryContext.Videos.Where(p => p.Id == video.Id).SingleOrDefault();
            if(query != null)
            {
                query.Director = video.Director;
                query.Cost = video.Cost;
                query.ImageUrl = video.ImageUrl;
                query.Location = video.Location;
                query.StatusId = video.StatusId;
                query.Title = video.Title;
                query.Year = video.Year;
                query.NumberOfCopies = video.NumberOfCopies;
                _libraryContext.Entry(video).State = EntityState.Detached;
                _libraryContext.SaveChanges();
                return true;
            }    
            else
            return false;
        }
        public int DeleteVideo(int id)
        {
            var query = _libraryContext.Videos.Where(p => p.Id == id).FirstOrDefault();
            if (query != null)
            {
                _libraryContext.LibraryAssets.Remove(query);
                _libraryContext.SaveChanges();
                return 1;
            }
            else
                return 0;
        }

        public int VideoDetail(int id)
        {
            var query = _libraryContext.Videos.Where(p => p.Id == id);
            if (query != null)
                return 1;
            else
                return 0;
        }

        public IEnumerable<Video> videos()
        {
            return _libraryContext.Videos.ToList();
        }

        public IEnumerable<Book> Books()
        {
            return _libraryContext.Books.ToList();
        }
    }
}
