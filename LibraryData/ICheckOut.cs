using LibraryData.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryData
{
    public interface ICheckOut
    {
        IEnumerable<Checkout> GetAll();
        IEnumerable<CheckOutHistory> GetCheckOutHistory(int id);
        IEnumerable<Hold> GetCurrentHolds(int id);
        Checkout GetById(int CheckOutId);
        Checkout GetLatestCheckOut(int AssetId);

        void Add(Checkout NewCheckOut);
        void CheckOutItem(int assetId, int LibraryCardId);
        void CheckInItem(int assetId);
        void MarkLost(int AssetId);
        void MarkFound(int AssetId);
        void PlaceHold(int AssetId, int LibraryCardId);
       

        string GetCurrentCheckOutPatron(int AssetId);
        string GetCurrentHoldPatronName(int id);
        DateTime GetCurrentHoldPlaced(int id);
        bool IsCheckedOut(int id);
        
    }
}
