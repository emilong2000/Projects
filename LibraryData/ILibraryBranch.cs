using LibraryData.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryData
{
    public interface ILibraryBranch
    {
        IEnumerable<LibraryBranch> GetAll();
        IEnumerable<Patron> GetPatrons(int BranchId);
        IEnumerable<LibraryAsset> GetAssets(int BranchId);
        IEnumerable<string> GetBranchHours(int BranchId);
        LibraryBranch GetBy(int branchId);
        void Add(LibraryBranch newBranch);
        bool IsBranchOpen(int BranchId);
    }
}
