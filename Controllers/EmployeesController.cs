
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeesCh12.Models;
using EmployeesCh12.ViewModel;

public class EmployeesController : Controller
{
    private readonly EmployeeContext _context;

    public EmployeesController(EmployeeContext context)
    {
        _context = context;
    }

    // GET: EMPLOYEES
    /*public async Task<IActionResult> Index()    
    {
        return View(await _context.Employees.ToListAsync());
    }*/

    public IActionResult DeptCount()
    {
        IQueryable<DepartmentGroup> data =
             from employee in _context.Employees.Include(e => e.Department)
             group employee by employee.DepartmentID into deptGroup
             select new DepartmentGroup()
             {
                 DepartmentID = (int)deptGroup.Key,
                 DepartmentCount = deptGroup.Count()
             };
        return View(data.ToList());
    }

    public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
    {
        ViewData["CurrentSort"] = sortOrder;
        ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
        ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

        if (searchString != null)
        {
            pageNumber = 1;
        }
        else
        {
            searchString = currentFilter;
        }
        ViewData["CurrentFilter"] = searchString;

            var employees = from e in _context.Employees.Include(e => e.Department).Include(e => e.Benefits) select e;

        if (!String.IsNullOrEmpty(searchString))
        {
            employees = employees.Where(s => s.LastName.Contains(searchString) || s.FirstName.Contains(searchString));
        }

        switch (sortOrder)
        {
            case "name_desc":
                employees = employees.OrderByDescending(s => s.LastName);
                break;
            case "Date":
                employees = employees.OrderBy(s  => s.LastName); 
                break;
            case "date_desc":
                employees = employees.OrderByDescending(s => s.HireDate);
                break;
            default:
                employees = employees.OrderBy(s => s.LastName);
                break;
        }
        int pageSize = 3;
        return View(await PaginatedList<Employee>.CreateAsync(employees.AsNoTracking(), pageNumber ?? 1, pageSize));
    }

    // GET: EMPLOYEES/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var employee = await _context.Employees
            .FirstOrDefaultAsync(m => m.ID == id);
        if (employee == null)
        {
            return NotFound();
        }

        return View(employee);
    }

    // GET: EMPLOYEES/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: EMPLOYEES/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("ID,FirstName,LastName,HireDate,DepartmentID,BenefitsID,Department,Benefits")] Employee employee)
    {
        if (ModelState.IsValid)
        {
            _context.Add(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(employee);
    }

    // GET: EMPLOYEES/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var employee = await _context.Employees.FindAsync(id);
        if (employee == null)
        {
            return NotFound();
        }
        return View(employee);
    }

    // POST: EMPLOYEES/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("ID,FirstName,LastName,HireDate,DepartmentID,BenefitsID,Department,Benefits")] Employee employee)
    {
        if (id != employee.ID)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(employee);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(employee.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(employee);
    }

    // GET: EMPLOYEES/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var employee = await _context.Employees
            .FirstOrDefaultAsync(m => m.ID == id);
        if (employee == null)
        {
            return NotFound();
        }

        return View(employee);
    }

    // POST: EMPLOYEES/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var employee = await _context.Employees.FindAsync(id);
        if (employee != null)
        {
            _context.Employees.Remove(employee);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool EmployeeExists(int? id)
    {
        return _context.Employees.Any(e => e.ID == id);
    }
}
