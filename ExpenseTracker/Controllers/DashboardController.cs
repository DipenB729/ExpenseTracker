using System.Globalization;
using ExpenseTracker.Data;
using ExpenseTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {

            //last 7 days
            DateTime StartDate = DateTime.Today.AddDays(-6);
            DateTime EndDate = DateTime.Today;
            List<Transaction>SelectedTransalations =await _context.Transaction
                .Include(x=>x.Category)
                .Where(y=>y.Date>=StartDate&&y.Date<=EndDate)
                .ToListAsync();
            //Total income
            int TotalIncome = SelectedTransalations
                .Where(i => i.Category.Type == "Income")
                .Sum(j => j.Amount);
            ViewBag.TotalIncome = TotalIncome.ToString("C0");
            //total expense
            int TotalExpense = SelectedTransalations
                .Where(i => i.Category.Type == "Expense")
                .Sum(j => j.Amount);
            ViewBag.TotalExpense= TotalExpense.ToString("C0");
            //balance
            int Balance = TotalIncome - TotalExpense;
            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-us");
            culture.NumberFormat.CurrencyNegativePattern = 1;
            ViewBag.Balance = string.Format(culture,"{0:C0}",Balance);

            //recent transaction
            ViewBag.RecentTransaction = await _context.Transaction
                .Include(i => i.Category)
                .OrderByDescending(j => j.Date)
                .Take(5)
                .ToListAsync();

            return View();
        }
    }
}
