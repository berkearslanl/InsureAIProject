using Microsoft.AspNetCore.Mvc;
using YapayZekaSigorta.Context;
using YapayZekaSigorta.Models;

namespace YapayZekaSigorta.ViewComponents.DashboardViewComponent
{
    public class _DashboardMainChartComponentPartial : ViewComponent
    {
        private readonly InsureContext _context;

        public _DashboardMainChartComponentPartial(InsureContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var revenueData = _context.Revenues
                .GroupBy(r => r.ProcessDate.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    TotalAmount = g.Sum(r => r.Amount)
                })
                .OrderBy(x => x.Month)
                .ToList();

            var expenseData = _context.Expenses
                .GroupBy(e => e.ProcessDate.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    TotalAmount = g.Sum(e => e.Amount)
                })
                .OrderBy(x => x.Month)
                .ToList();

            var allMonths = revenueData.Select(x => x.Month)
                .Union(expenseData.Select(x => x.Month))
                .OrderBy(m => m)
                .ToList();

            var model = new RevenueExpenseChartViewModel()
            {
                Months = allMonths.Select(x => new System.Globalization.DateTimeFormatInfo().GetAbbreviatedMonthName(x)).ToList(),
                revenueTotals= allMonths.Select(m => revenueData.FirstOrDefault(r => r.Month == m)?.TotalAmount ?? 0).ToList(),
                expenseTotals = allMonths.Select(m => expenseData.FirstOrDefault(e => e.Month == m)?.TotalAmount ?? 0).ToList()
            };

            ViewBag.v1 = _context.Revenues.Sum(x => x.Amount);
            ViewBag.v2 = _context.Expenses.Sum(x => x.Amount);
            return View(model);
        }
    }
}
