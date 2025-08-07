using CarSalesApp.Data;
using System.Configuration;
using System.Data;
using System.Windows;

namespace CarSalesApp.UI;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
	protected override void OnStartup(StartupEventArgs e)
	{
		using var db = new CarSalesContext();
		DataSeeder.Seed(db);

		base.OnStartup(e);
	}
}

