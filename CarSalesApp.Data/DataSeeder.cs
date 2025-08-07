using CarSalesApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSalesApp.Data
{
	public static class DataSeeder
	{
		public static void Seed(CarSalesContext db)
		{
			if (db.Orders.Any()) return;

			var rnd = new Random();

			var brands = new[]
			{
				new Brand { Name = "Hyundai" },
				new Brand { Name = "Peugeot" },
				new Brand { Name = "Kia" },
			};

			foreach (var brand in brands)
			{
				db.Brands.Add(brand);
			}

			db.SaveChanges();

			var models = new[]
			{
				new Model { Name = "i20", BrandId = brands[0].Id },
				new Model { Name = "i30", BrandId = brands[0].Id },
				new Model { Name = "308", BrandId = brands[1].Id },
				new Model { Name = "408", BrandId = brands[1].Id },
				new Model { Name = "K3", BrandId = brands[2].Id },
				new Model { Name = "K5", BrandId = brands[2].Id },
			};

			foreach (var model in models)
			{
				db.Models.Add(model);
			}

			db.SaveChanges();

			var start = new DateTime(2020, 1, 1);
			for (int i = 0; i < 1200; i++)
			{
				var model = models[rnd.Next(models.Length)];
				var date = start.AddDays(rnd.Next(0, 365 * 5));
				var price = rnd.Next(2_000_000, 7_000_000);

				db.Orders.Add(new Order
				{
					ModelId = model.Id,
					OrderDate = date,
					Price = price
				});
			}

			db.SaveChanges();
		}
	}
}
