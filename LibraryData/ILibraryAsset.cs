using LibraryData.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryData
{
    public interface ILibraryAsset
    {
        IEnumerable<LibraryAsset> GetAll();
        LibraryAsset GetById(int id);
        void AddAsset(LibraryAsset newAsset);
        LibraryAsset AddRecord(Book libraryAsset);
        string GetAuthorOrDirector(int id);
        string GetDeweyIndex(int id);
        string GetType(int id);
        string GetTitle(int id);
        string GetISBN(int id);
        LibraryBranch GetCurrentLocation(int id);
        IEnumerable<LibraryAsset> LibraryAssets();
        bool Update(Book book);
        int Delete(int id);
        int BookDetail(int id);
        LibraryAsset AddVideo(Video video);
        bool UpdateVideo(Video video);
        int DeleteVideo(int id);
        int VideoDetail(int id);
        IEnumerable<Video> videos();
        IEnumerable<Book> Books();
    }
}
