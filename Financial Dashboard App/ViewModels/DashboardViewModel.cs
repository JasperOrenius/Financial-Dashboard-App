using DocumentFormat.OpenXml.Drawing.Diagrams;
using Financial_Dashboard_App.Models;
using Financial_Dashboard_App.Services;
using LiveCharts;
using LiveCharts.Wpf;
using System.Runtime.ExceptionServices;

namespace Financial_Dashboard_App.ViewModels
{
    public class DashboardViewModel : BaseViewModel
    {
        private readonly IDatabaseService databaseService;

        public ChartValues<decimal> IncomeSeries { get; set; } = new ChartValues<decimal>();
        public ChartValues<decimal> ExpenseSeries { get; set; } = new ChartValues<decimal>();
        public SeriesCollection ExpenseBreakdownSeries { get; set; } = new SeriesCollection();
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
                IncomeSeries.Clear();
                ExpenseSeries.Clear();
                ExpenseBreakdownSeries.Clear();
                ProfitGrowthSeries.Clear();

                TotalIncome = transactions.Where(t => t.Type == "Income").Sum(t => t.Amount);
                TotalExpenses = transactions.Where(t => t.Type == "Expense").Sum(t => t.Amount);
                NetProfit = TotalIncome - TotalExpenses;

                var startDate = transactions.Min(t => t.Date);
                var initialProfit = transactions.Where(t => t.Date == startDate).Sum(t => t.Type == "Income" ? t.Amount : -t.Amount);

                GrowthRate = Math.Round(initialProfit > 0 ? (NetProfit - initialProfit) / initialProfit * 100 : 0, 2);

                IncomeSeries.Add(TotalIncome);
                ExpenseSeries.Add(TotalExpenses);
                var expenseGroups = transactions.Where(t => t.Type == "Expense").GroupBy(t => t.Description).Select(g => new { Description = g.Key, Total = g.Sum(t => t.Amount) }).OrderByDescending(g => g.Total).ToList();
                if(expenseGroups.Count > 4)
                {
                    var topGroups = expenseGroups.Take(4).ToList();
                    var otherTotal = expenseGroups.Skip(4).Sum(g => g.Total);
                    foreach(var group in topGroups)
                    {
                        ExpenseBreakdownSeries.Add(new PieSeries
                        {
                            Title = group.Description,
                            Values = new ChartValues<decimal> { group.Total }
                        });
                    }

                    ExpenseBreakdownSeries.Add(new PieSeries
                    {
                        Title = "Other",
                        Values = new ChartValues<decimal> { otherTotal }
                    });
                }
                else
                {
                    foreach(var group in expenseGroups)
                    {
                        ExpenseBreakdownSeries.Add(new PieSeries
                        {
                            Title = group.Description,
                            Values = new ChartValues<decimal> { group.Total }
                        });
                    }
                }

                var transactionMonths = transactions.Select(t => new DateTime(t.Date.Year, t.Date.Month, 1)).Distinct().OrderBy(d => d).ToList();

                if(transactionMonths.Count > 1)
                {
                    foreach(var month in transactionMonths)
                    {
                        var monthlyProfit = transactions.Where(t => t.Date.Month == month.Month && t.Date.Year == month.Year).Sum(t => t.Type == "Income" ? t.Amount : -t.Amount);
                        ProfitGrowthSeries.Add(monthlyProfit);
                    }
                }
                else
                {
                    ProfitGrowthSeries.Add(initialProfit);
                    ProfitGrowthSeries.Add(NetProfit);
                }
            }
        }
    }
}