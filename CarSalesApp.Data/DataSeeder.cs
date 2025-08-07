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
				new Brand { Name = "Audi" },
				new Brand { Name = "BMW" },
				new Brand { Name = "Toyota" },
			};

			foreach (var brand in brands)
			{
				db.Brands.Add(brand);
			}

			db.SaveChanges();

			var models = new[]
			{
				new Model { Name = "A4", BrandId = brands[0].Id },
				new Model { Name = "A6", BrandId = brands[0].Id },
				new Model { Name = "X2", BrandId = brands[1].Id },
				new Model { Name = "X3", BrandId = brands[1].Id },
				new Model { Name = "Corolla", BrandId = brands[2].Id },
				new Model { Name = "Camry", BrandId = brands[2].Id },
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
