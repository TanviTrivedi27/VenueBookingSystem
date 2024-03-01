using MetronicsAdminPanel.Data;
using MetronicsAdminPanel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Http;
using System.Text.Json.Nodes;
using System.Linq; // Add this using directive if not already present
using System.Linq.Expressions;


namespace MetronicsAdminPanel.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
       
        private readonly ILogger<HomeController> _logger;
        
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _context = context;
            _logger = logger;
   
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        //#region Listing

        //public  IActionResult GetSliderList()
        //{
        //    var data = _context.Sliders.ToList();
        //    return new JsonResult(new { data = data });
        //}
        //#endregion
        #region Listing
        [HttpPost]
        [Route("/Home/GetSliderList")]
        public IActionResult GetSliderList(string searchValue, int draw, int start, int length, string sortColumn, string sortDirection)
        {
            IQueryable<Sliders> query = _context.Sliders.AsQueryable(); // Convert DbSet to IQueryable

            // Apply search filter if searchValue is provided
            if (!string.IsNullOrEmpty(searchValue))
            {
                // Example: Filter based on the 'title' property
                query = query.Where(s => 
                    s.Title.Contains(searchValue) ||
                    s.SliderId.ToString().Contains(searchValue) ||
                    s.Description.Contains(searchValue) ||
                    s.BtnTitle.Contains(searchValue) ||
                    s.BtnRedirectUrl.Contains(searchValue));
            }
            // Sorting
            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortDirection))
            {
                switch (sortColumn.ToLower())
                {
                    case "sliderid": // Correct the case to match your column names in the database
                        query = sortDirection.ToLower() == "asc" ? query.OrderBy(s => s.SliderId) : query.OrderByDescending(s => s.SliderId);
                        break;
                    case "imgpath":
                        query = sortDirection.ToLower() == "asc" ? query.OrderBy(s => s.ImgPath) : query.OrderByDescending(s => s.ImgPath);
                        break;
                    case "title":
                        query = sortDirection.ToLower() == "asc" ? query.OrderBy(s => s.Title) : query.OrderByDescending(s => s.Title);
                        break;
                    case "description":
                        query = sortDirection.ToLower() == "asc" ? query.OrderBy(s => s.Description) : query.OrderByDescending(s => s.Description);
                        break;
                    case "btntitle":
                        query = sortDirection.ToLower() == "asc" ? query.OrderBy(s => s.BtnTitle) : query.OrderByDescending(s => s.BtnTitle);
                        break;
                    case "btnredirecturl":
                        query = sortDirection.ToLower() == "asc" ? query.OrderBy(s => s.BtnRedirectUrl) : query.OrderByDescending(s => s.BtnRedirectUrl);
                        break;
                    default:
                        // Handle the case where the sort column is not recognized
                        break;
                }
            }
            // Pagination
            query = query.Skip(start).Take(length);

            var data = query.ToList();

            return new JsonResult(new { draw = draw, recordsTotal = _context.Sliders.Count(), recordsFiltered = data.Count(), data = data });
        }
        #endregion
        public IActionResult Sliders()
        {
            return View();
        }

        public IActionResult AddSlider()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult>AddSlider(Sliders sliders)
        {
            if (ModelState.IsValid)
            {
                _context.Sliders.Add(sliders);
               await _context.SaveChangesAsync();
                return RedirectToAction("Sliders");
            }

            return View(sliders);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var slider = await _context.Sliders.FirstOrDefaultAsync(m => m.SliderId == id);
            if (slider == null)
            {
                return NotFound();
            }

            return View(slider); // Assuming you have a Details view for Slider model
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}