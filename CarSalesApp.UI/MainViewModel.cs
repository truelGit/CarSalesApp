using CarSalesApp.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;

namespace CarSalesApp.UI
{
	public class MonthlySalesRow
	{
		public string ModelName { get; set; } = "";
		public decimal[] MonthlyTotals { get; set; } = new decimal[12];
	}

	public partial class MainViewModel : ObservableObject
	{
		private readonly CarSalesContext _db = new();

		[ObservableProperty]
		private int selectedYear = DateTime.Now.Year;

		[ObservableProperty]
		private string? selectedModel;

		public ObservableCollection<int> Years { get; } = new();
		public ObservableCollection<string> Models { get; } = new();
		public ObservableCollection<MonthlySalesRow> Sales { get; } = new();

		public MainViewModel()
		{
			LoadFilters();
			LoadData();
		}

		private void LoadFilters()
		{
			Years.Clear();
			Models.Clear();

			var years = _db.Orders
				.Select(o => o.OrderDate.Year)
				.Distinct()
				.OrderBy(y => y)
				.ToList();

			foreach (var year in years)
				Years.Add(year);

			var models = _db.Models
				.OrderBy(m => m.Name)
				.Select(m => m.Name)
				.Distinct()
				.ToList();

			foreach (var model in models)
				Models.Add(model);
		}

		[RelayCommand]
		public void LoadData()
		{
			Sales.Clear();

			var query = _db.Orders
				.Include(o => o.Model)
				.Where(o => o.OrderDate.Year == SelectedYear);

			if (!string.IsNullOrEmpty(SelectedModel))
			{
				query = query.Where(o => o.Model.Name == SelectedModel);
			}

			var grouped = query
				.AsEnumerable()
				.GroupBy(o => o.Model.Name)
				.Select(g => new MonthlySalesRow
				{
					ModelName = g.Key,
					MonthlyTotals = Enumerable.Range(1, 12)
						.Select(month => g.Where(o => o.OrderDate.Month == month).Sum(o => o.Price))
						.ToArray()
				});

			foreach (var row in grouped)
				Sales.Add(row);
		}

		[RelayCommand]
		public void ExportToExcel()
		{
			var wb = new ClosedXML.Excel.XLWorkbook();
			var ws = wb.Worksheets.Add("Продажи");

			// Заголовки
			ws.Cell(1, 1).Value = "Модель";
			var months = new[] { "Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь" };
			for (int i = 0; i < 12; i++)
				ws.Cell(1, i + 2).Value = months[i];

			// Данные
			for (int row = 0; row < Sales.Count; row++)
			{
				var sale = Sales[row];
				ws.Cell(row + 2, 1).Value = sale.ModelName;

				for (int col = 0; col < 12; col++)
				{
					var cell = ws.Cell(row + 2, col + 2);
					cell.Value = sale.MonthlyTotals[col];

					// Подсветка, если больше 25 млн
					if (sale.MonthlyTotals[col] > 25_000_000)
						cell.Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.LightGreen;
				}
			}

			// Сохраняем
			var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), $"Продажи_{SelectedYear}.xlsx");
			wb.SaveAs(path);

			MessageBox.Show($"Экспорт завершён!\nФайл: {path}", "Excel", MessageBoxButton.OK, MessageBoxImage.Information);
		}
	}
}
