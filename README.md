# Financial-Dashboard-App

A WPF application for managing financial transactions, generating reports, and visualizing financial data using interactive charts.

## Features

### Dashboard View:
- Displays total income, total expenses, net profit and growth rate.
- Visualizes income vs. expense, expense breakdown and profit growth charts.
- Groups expenses by description and categorizes smaller expenses under "Other" if there are more than five categories.

### Transactions View:
- List all financial transactions with filtering and searching.
- Allows editing and deleting transactions.
- Supports infinite scrolling when transactions exceed screen size.

### ImportData View:
- Manually add transactions by selecting type, entering description, amount and choosing a date or import transactions from an Excel file.
- Validates imported data before saving transactions to the database.

### Reports View:
- Generates reports of the transactions based on the chosen time range.
- Reports can be exported to **PDF** format.

## Technologies Used
- **WPF (Windows Presentation Foundation)**
- **MVVM Architecture**
- **LiveCharts** (For data visualization)
- **ClosedXML** (For Excel handling)
- **PDFsharp** (For PDF handling)
- **SQLite** (For database storage)

## Installation & Setup
1. Clone the repository:
   ```sh
   git clone https://github.com/yourusername/Financial-Dashboard-App.git
2. Open the project in **Visual Studio**
3. Build and run the application

## Usage
- **Adding Transactions:**
  - Navigate to the Import Data view and fill in the details and click "Add Entry".
  - Optionally import transactions from an Excel File.
- **Editing & Deleting Transactions:**
  - Navigate to the Transactions view.
  - Click "Edit" to modify a transaction.
  - Click "Delete" to remove a transaction.
- **Inspecting Data**
  - In the Dashboard view, you can see the overview of all the transactions and inspect the the displayed data and interactive charts.
- **Generating Reports**
  - In the Reports view, select a "From" and "To" date from which date range you want to create the report based on and then click "Generate Report".
  - After generating the report click "Export to PDF" and choose a location and save the file.
