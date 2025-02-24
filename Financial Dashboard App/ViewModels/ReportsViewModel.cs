using Financial_Dashboard_App.Commands;
using Financial_Dashboard_App.Models;
using Financial_Dashboard_App.Services;
using Microsoft.Win32;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Financial_Dashboard_App.ViewModels
{
    public class ReportsViewModel : BaseViewModel
    {
        private readonly IDatabaseService databaseService;

        public ObservableCollection<Transaction> Transactions { get; set; } = new ObservableCollection<Transaction>();

        private DateTime startDate = DateTime.Today.AddMonths(-1);
        public DateTime StartDate
        {
            get => startDate;
            set
            {
                startDate = value;
                OnPropertyChanged(nameof(StartDate));
            }
        }

        private DateTime endDate = DateTime.Today;
        public DateTime EndDate
        {
            get => endDate;
            set
            {
                endDate = value; 
                OnPropertyChanged(nameof(EndDate));
            }
        }

        public ICommand GenerateReportCommand { get; }
        public ICommand ExportToPDFCommand { get; }

        public ReportsViewModel(IDatabaseService databaseService)
        {
            this.databaseService = databaseService;
            GenerateReportCommand = new RelayCommand(GenerateReport);
            ExportToPDFCommand = new RelayCommand(ExportToPDF);
        }

        private async Task GenerateReport()
        {
            Transactions.Clear();
            var allTransactions = await databaseService.GetAllTransactions();
            var filteredTransactions = allTransactions.Where(t => t.Date >= StartDate && t.Date <= EndDate).OrderBy(t => t.Date);
            foreach(var transaction in filteredTransactions)
            {
                Transactions.Add(transaction);
            }
        }

        private async Task ExportToPDF()
        {
            if(Transactions.Count == 0)
            {
                MessageBox.Show("Generate Report First!");
                return;
            }
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "PDF Files (*.pdf)|*.pdf|All Files (*.*)|*.*",
                DefaultExt = ".pdf"
            };

            if(saveFileDialog.ShowDialog() == true)
            {
                string filePath = saveFileDialog.FileName;
                CreatePDFReport(Transactions.ToList(), filePath);
                
                MessageBox.Show("Report successfully exported as PDF");
            }
            else
            {
                MessageBox.Show("File save operation was cancelled");
            }            
        }

        private void CreatePDFReport(List<Transaction> transactions, string filePath)
        {
            PdfDocument document = new PdfDocument();
            document.Info.Title = "Financial Report";

            PdfPage page = document.AddPage();

            XGraphics gfx = XGraphics.FromPdfPage(page);

            XFont titleFont = new XFont("Verdana", 16, XFontStyleEx.Bold);
            XFont regularFont = new XFont("Verdana", 12);

            gfx.DrawString("Financial Report", titleFont, XBrushes.Black, new XPoint(50, 50));

            gfx.DrawString($"Date: {DateTime.Now.ToShortDateString()}", regularFont, XBrushes.Black, new XPoint(50, 80));

            int maxDateWidth = (int)transactions.Max(t => gfx.MeasureString(t.Date.ToShortDateString(), regularFont).Width);
            int maxTypeWidth = (int)transactions.Max(t => gfx.MeasureString(t.Type, regularFont).Width);
            int maxDescriptionWidth = (int)transactions.Max(t => gfx.MeasureString(t.Description, regularFont).Width);
            int maxAmountWidth = (int)transactions.Max(t => gfx.MeasureString(t.Amount.ToString("C"), regularFont).Width);

            int padding = 10;
            maxDateWidth += padding;
            maxTypeWidth += padding;
            maxDescriptionWidth += padding;
            maxAmountWidth += padding;

            double pageWidth = page.Width - 100;
            double totalWidth = maxDateWidth + maxTypeWidth + maxDescriptionWidth + maxAmountWidth;

            double scaleFactor = pageWidth / totalWidth;

            double adjustedDateWidth = maxDateWidth * scaleFactor;
            double adjustedTypeWidth = maxTypeWidth * scaleFactor;
            double adjustedDescriptionWidth = maxDescriptionWidth * scaleFactor;
            double adjustedAmountWidth = maxAmountWidth * scaleFactor;

            int startY = 120;
            int rowHeight = 20;
            gfx.DrawString("Date", regularFont, XBrushes.Black, new XPoint(50, startY));
            gfx.DrawString("Type", regularFont, XBrushes.Black, new XPoint(50 + adjustedDateWidth, startY));
            gfx.DrawString("Description", regularFont, XBrushes.Black, new XPoint(50 + adjustedDateWidth + adjustedTypeWidth, startY));
            gfx.DrawString("Amount", regularFont, XBrushes.Black, new XPoint(50 + adjustedDateWidth + adjustedTypeWidth + adjustedDescriptionWidth, startY));

            gfx.DrawLine(XPens.Black, 50, startY + rowHeight, page.Width - 50, startY + rowHeight);

            int currentY = startY + rowHeight + 20;
            foreach(var transaction in transactions)
            {
                gfx.DrawString(transaction.Date.ToShortDateString(), regularFont, XBrushes.Black, new XPoint(50, currentY));
                gfx.DrawString(transaction.Type, regularFont, XBrushes.Black, new XPoint(50 + adjustedDateWidth, currentY));
                gfx.DrawString(transaction.Description, regularFont, XBrushes.Black, new XPoint(50 + adjustedDateWidth + adjustedTypeWidth, currentY));
                gfx.DrawString(transaction.Amount.ToString("C"), regularFont, XBrushes.Black, new XPoint(50 + adjustedDateWidth + adjustedTypeWidth + adjustedDescriptionWidth, currentY));

                currentY += rowHeight;
            }

            document.Save(filePath);
        }
    }
}
