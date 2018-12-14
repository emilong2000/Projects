using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using E_Library.ModelViews.Branch;
using LibraryData;
using LibraryData.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Library.Controllers
{
    [Produces("application/json")]
    [Route("api/Managers")]
    public class ManagersController : Controller
    {
        private ILibraryBranch _branch;
        public ManagersController(ILibraryBranch branch)
        {
            _branch = branch;
        }
        // GET: api/Managers
        [HttpGet]
        public IEnumerable<BranchDetailModel> Get()
        {
            var branches = _branch.GetAll().Select(branch => new BranchDetailModel
            {
                Id = branch.Id,
                Name = branch.Name,
                IsOpen = _branch.IsBranchOpen(branch.Id),
                NumberOfAssets = _branch.GetAssets(branch.Id).Count(),
                NumberOfPatrons = _branch.GetPatrons(branch.Id).Count(),
                PhoneNo = branch.Telephone

            });

            var model = new BranchIndexModel()
            {
                Branches = branches
            };
            return model.Branches;
        }

        // GET: api/Managers/5
        [HttpGet("{id}", Name = "Get")]
        public BranchDetailModel Get(int id)
        {
            var branch = _branch.GetBy(id);
            var model = new BranchDetailModel
            {
                Id = branch.Id,
                Name = branch.Name,
                Address = branch.Address,
                PhoneNo = branch.Telephone,
                OpenDate = branch.OpenDate.ToString("dd-MM-yyyy"),
                NumberOfAssets = _branch.GetAssets(id).Count(),
                NumberOfPatrons = _branch.GetAssets(id).Count(),
                TotalAssetValue = _branch.GetAssets(id).Sum(a => a.Cost),
                ImageUrl = branch.ImageUrl,
                HoursOpen = _branch.GetBranchHours(id)
            };
            return model;
        }
        
        // POST: api/Managers
        [HttpPost]
        public IActionResult Post([FromBody]BranchDetailModel value)
        {
            if(value == null)
            {
                return BadRequest();
            }
            else
            {
                return Ok();
            }
           
        }
        
        // PUT: api/Managers/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
