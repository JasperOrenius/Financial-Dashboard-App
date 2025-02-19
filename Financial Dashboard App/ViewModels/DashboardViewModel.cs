using DocumentFormat.OpenXml.Drawing.Diagrams;
using Financial_Dashboard_App.Models;
using Financial_Dashboard_App.Services;
using LiveCharts;

namespace Financial_Dashboard_App.ViewModels
{
    public class DashboardViewModel : BaseViewModel
    {
        private readonly IDatabaseService databaseService;

        public ChartValues<decimal> IncomeExpenseSeries { get; set; } = new ChartValues<decimal>();
        public ChartValues<decimal> ExpenseBreakdownSeries { get; set; } = new ChartValues<decimal>();
        public ChartValues<decimal> ProfitGrowthSeries { get; set; } = new ChartValues<decimal>();

        private decimal totalIncome;
        public decimal TotalIncome
        {
            get => totalIncome;
            set
            {
                totalIncome = value;
                OnPropertyChanged(nameof(TotalIncome));
            }
        }

        private decimal totalExpenses;
        public decimal TotalExpenses
        {
            get => totalExpenses;
            set
            {
                totalExpenses = value;
                OnPropertyChanged(nameof(TotalExpenses));
            }
        }

        private decimal netProfit;
        public decimal NetProfit
        {
            get => netProfit;
            set
            {
                netProfit = value;
                OnPropertyChanged(nameof(NetProfit));
            }
        }

        private decimal growthRate;
        public decimal GrowthRate
        {
            get => growthRate;
            set
            {
                growthRate = value;
                OnPropertyChanged(nameof(GrowthRate));
            }
        }

        public DashboardViewModel(IDatabaseService databaseService)
        {
            this.databaseService = databaseService;
            LoadTransactions();
        }

        private async Task LoadTransactions()
        {
            var transactions = await databaseService.GetAllTransactions();
            if(transactions.Any())
            {
                TotalIncome = transactions.Where(t => t.Type == "Income").Sum(t => t.Amount);
                TotalExpenses = transactions.Where(t => t.Type == "Expense").Sum(t => t.Amount);
                NetProfit = TotalIncome - TotalExpenses;

                var startDate = transactions.Min(t => t.Date);
                var endDate = transactions.Max(t => t.Date);
                var months = (endDate.Year - startDate.Year) * 12 + endDate.Month - startDate.Month + 1;

                if(months > 1)
                {
                    var initialProfit = transactions.Where(t => t.Date.Month == startDate.Month && t.Date.Year == startDate.Year).Sum(t => t.Type == "Income" ? t.Amount : -t.Amount);
                    GrowthRate = Math.Round(initialProfit > 0 ? (NetProfit - initialProfit) / initialProfit * 100 : 0, 2);
                }

                IncomeExpenseSeries.Add(TotalIncome);
                IncomeExpenseSeries.Add(TotalExpenses);
                var expenseGroups = transactions.Where(t => t.Type == "Expense").GroupBy(t => t.Description).Select(g => new { Description = g.Key, Total = g.Sum(t => t.Amount) });
                foreach(var group in expenseGroups)
                {
                    ExpenseBreakdownSeries.Add(group.Total);
                }

                foreach(var month in Enumerable.Range(0, months))
                {
                    var monthDate = startDate.AddMonths(month);
                    var monthlyProfit = transactions.Where(t => t.Date.Month == monthDate.Month && t.Date.Year == monthDate.Year).Sum(t => t.Type == "Income" ? t.Amount : -t.Amount);
                    ProfitGrowthSeries.Add(monthlyProfit);
                }
            }
        }
    }
}