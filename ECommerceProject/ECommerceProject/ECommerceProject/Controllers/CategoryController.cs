using Microsoft.AspNetCore.Mvc;
using ECommerceProject.Data;
using ECommerceProject.Models;
using ECommerceProject.ViewModels;
using System.Linq;
using System.Threading.Tasks;

public class CategoryController : Controller
{
    private readonly ApplicationDbContext _context;

    public CategoryController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Category/Index
    public IActionResult Index()
    {
        var categories = _context.Category.ToList();
        return View(categories);
    }

    // GET: Category/Create
    public IActionResult Create()
    {
        // Populate departments enum
        var departments = Enum.GetValues(typeof(Department)).Cast<Department>().ToList();
        ViewBag.Departments = departments;

        return View(new CategoryViewModel());
    }

    // POST: Category/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CategoryViewModel categoryVM)
    {
        if (ModelState.IsValid)
        {
            var category = new Category
            {
                Name = categoryVM.Name,
                Department = categoryVM.Department
            };

            _context.Add(category);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index)); // Redirect to the Index page
        }

        // If something goes wrong, re-display the form with departments
        ViewBag.Departments = Enum.GetValues(typeof(Department)).Cast<Department>().ToList();
        return View(categoryVM);
    }

    // GET: Category/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        // Get the category from the database
        var category = await _context.Category.FindAsync(id);
        if (category == null)
        {
            return NotFound();
        }

        // Populate departments enum
        var departments = Enum.GetValues(typeof(Department)).Cast<Department>().ToList();
        ViewBag.Departments = departments;

        // Create a ViewModel with the existing category data
        var categoryVM = new CategoryViewModel
        {
            Id = category.Id,
            Name = category.Name,
            Department = category.Department
        };

        return View(categoryVM);
    }

    // POST: Category/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, CategoryViewModel categoryVM)
    {
        if (id != categoryVM.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            var category = await _context.Category.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            // Update the category properties
            category.Name = categoryVM.Name;
            category.Department = categoryVM.Department;

            // Save changes to the database
            _context.Update(category);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index)); // Redirect to the Index page
        }

        // If something goes wrong, re-display the form with departments
        ViewBag.Departments = Enum.GetValues(typeof(Department)).Cast<Department>().ToList();
        return View(categoryVM);
    }

    // GET: Category/Delete/5
    // GET: Category/Delete/5
    public IActionResult Delete(int id)
    {
        var category = _context.Category.Find(id);
        if (category == null)
        {
            return NotFound(); // Return a 404 page if category not found
        }
        return View(category);
    }

    // POST: Category/DeleteConfirmed/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var category = await _context.Category.FindAsync(id);
        if (category == null)
        {
            return NotFound();
        }

        _context.Category.Remove(category);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index)); // Redirect to the list of categories after deletion
    }
}