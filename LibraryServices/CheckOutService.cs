using LibraryData;
using LibraryData.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;


namespace LibraryServices
{
    public class CheckOutService : ICheckOut
    {
        private LibraryContext _context;
        public CheckOutService(LibraryContext context)
        {
            _context = context;
        }
        public void Add(Checkout newCheckOut)
        {
            _context.Add(newCheckOut);
            _context.SaveChanges();
        }

        public void CheckOutItem(int assetId, int LibrarycardId)
        {
            if (IsCheckedOut(assetId))
            {
                return;
                    //Add some logic to handle feedback to the user
                
            }
            var item = _context.LibraryAssets
                .FirstOrDefault(a => a.Id == assetId);
           
            UpdateAssetStatus(assetId, "Checked Out");
            var librarycard = _context.LibraryCards
                .Include(card => card.CheckOuts)
                .FirstOrDefault(card => card.Id == LibrarycardId);

            var now = DateTime.Now;
            var checkout = new Checkout()
            {
                LibraryAsset = item,
                LibraryCard = librarycard,
                Since = now,
                Until = GetDefaultCheckOutTime(now)
            };
            _context.Add(checkout);

            var checkoutHistory = new CheckOutHistory()
            {
                CheckedOut = now,
                LibraryAsset = item,
                LibraryCard = librarycard
            };

            _context.Add(checkoutHistory);
            _context.SaveChanges();
        }
        private DateTime GetDefaultCheckOutTime(DateTime now)
        {
            return now.AddDays(30);
        }

        public bool IsCheckedOut(int assetId)
        {
                return _context.CheckOuts
                .Where(p => p.LibraryAsset.Id == assetId)
                .Any();
        }

        public IEnumerable<Checkout> GetAll()
        {
            return _context.CheckOuts;
        }

        public Checkout GetById(int CheckOutId)
        {
            return GetAll().FirstOrDefault(check => check.Id == CheckOutId);
        }

        public IEnumerable<CheckOutHistory> GetCheckOutHistory(int id)
        {
            return _context.CheckOutHistories
                .Include(p=>p.LibraryAsset)
                .Include(p=>p.LibraryCard)
                .Where(p => p.LibraryAsset.Id == id);
        }

        public string GetCurrentHoldPatronName(int HoldId)
        {
            var hold = _context.Holds
                .Include(p => p.LibraryAsset)
                .Include(p => p.LibraryCard)
                .FirstOrDefault(p => p.Id == HoldId);

                var cardId = hold?.LibraryCard.Id;
            var patron = _context.Patrons.Include(p => p.LibraryCard)
                .FirstOrDefault(p => p.LibraryCard.Id == cardId);

            return patron?.FirstName + "" + patron?.LastName;
        }

        public DateTime GetCurrentHoldPlaced(int holdId)
        {
            return
                _context.Holds
                .Include(p => p.LibraryAsset)
                .Include(p => p.LibraryCard)
                .FirstOrDefault(p => p.Id == holdId)
                .HoldPlaced;
        }

        public IEnumerable<Hold> GetCurrentHolds(int id)
        {
            return _context.Holds
                .Include(p => p.LibraryAsset)
                .Where(p => p.LibraryAsset.Id == id);
        }

        public Checkout GetLatestCheckOut(int AssetId)
        {
            return _context.CheckOuts
                .Where(k => k.LibraryAsset.Id == AssetId)
                .OrderByDescending(p => p.Since)
                .FirstOrDefault();
        }

        public void MarkFound(int assetId)
        {
            var now = DateTime.Now;
           
            UpdateAssetStatus(assetId, "Available");
            //Remove any existing checkouts on the item
            RemoveExistingCheckouts(assetId);
            
            //Close any existing checkout history
            CloseExistingCheckoutHistory(assetId, now);

            _context.SaveChanges();
        }

        private void UpdateAssetStatus(int assetId, string newStatus)
        {
            var item = _context.LibraryAssets
               .FirstOrDefault(p => p.Id == assetId);

            item.Status = _context.Statuses
               .FirstOrDefault(p => p.Name == newStatus);


            _context.Update(item);  
        }

        private void UpdateQty(int assetId, int qty)
        {
            var qtyToUpdate = 0;
            var item = _context.LibraryAssets
               .FirstOrDefault(p => p.Id == assetId);

            if(item.NumberOfCopies > 0)
            {
                qtyToUpdate -= qty;
                item.NumberOfCopies = qtyToUpdate;
            }

            _context.Update(item);
        }

        private void CloseExistingCheckoutHistory(int assetId, DateTime now)
        {
            var history = _context.CheckOutHistories
               .FirstOrDefault(p => p.LibraryAsset.Id == assetId && p.CheckedIn == null);
            if (history != null)
            {
                _context.Update(history);
                //history.CheckedIn = DateTime.Now;
                history.CheckedIn = now;
            }
        }

        private void RemoveExistingCheckouts(int assetId)
        {
            var checkout = _context.CheckOuts
               .FirstOrDefault(p => p.LibraryAsset.Id == assetId);
            if (checkout != null)
            {
                _context.Remove(checkout);
            }
        }

        public void MarkLost(int assetId)
        {
            UpdateAssetStatus(assetId, "Lost");
            _context.SaveChanges();
           
        }

        public void PlaceHold(int assetId, int LibraryCardId)
        {
            var now = DateTime.Now;
            var asset = _context.LibraryAssets
                .Include(c=>c.Status)
                .FirstOrDefault(a => a.Id == assetId);
            var card = _context.LibraryCards.FirstOrDefault(c => c.Id == LibraryCardId);

            if(asset.Status.Name == "Available")
            {
                UpdateAssetStatus(assetId, "On Hold");
            }
            var hold = new Hold()
            {
                HoldPlaced = now,
                LibraryAsset = asset,
                LibraryCard = card
            };

            _context.Add(hold);
            _context.SaveChanges();
        }

        public void CheckInItem(int assetId)
        {
            var now = DateTime.Now;
            var item = _context.LibraryAssets.FirstOrDefault(a => a.Id == assetId);
            //Remove any existing checkouts on the item
            RemoveExistingCheckouts(assetId);
            //Close any existing check out history
            CloseExistingCheckoutHistory( assetId,now);
            //look for existing Holds on the item
            var currentHolds = _context.Holds
                .Include(k => k.LibraryAsset)
                .Include(k => k.LibraryCard)
                .Where(k => k.LibraryAsset.Id == assetId);


            // if there is existing Holds, checkout the librarycard with the earliest Hold.
            if (currentHolds.Any())
            {
                CheckoutToEarliestHold(assetId, currentHolds);
            }
            //otherwise update the item status to available
            UpdateAssetStatus(assetId, "Available");
            _context.SaveChanges();
        }

        private void CheckoutToEarliestHold(int assetId, IQueryable<Hold> currentHolds)
        {
            var EarliestHold = currentHolds.OrderBy(hold => hold.HoldPlaced)
                .FirstOrDefault();
            var card = EarliestHold.LibraryCard;
            _context.Remove(EarliestHold);
            _context.SaveChanges();
            CheckOutItem(assetId, card.Id);
        }

        public string GetCurrentCheckOutPatron(int AssetId)
        {
            var checkout = GetCheckOutByAssetId(AssetId);
            if (checkout  == null)
            {
                return "";
            }
            var cardId = checkout.LibraryCard.Id;

            var patron = _context.Patrons.Include(p => p.LibraryCard)
                .FirstOrDefault(p => p.LibraryCard.Id == cardId);
            return patron.FirstName + "" + patron.LastName;
        }

        private Checkout GetCheckOutByAssetId(int assetId)
        {
            return
                _context.CheckOuts.Include(c=>c.LibraryAsset)
                .Include(c=>c.LibraryCard)
                .FirstOrDefault(c=>c.LibraryAsset.Id == assetId);
        }
    }
}
