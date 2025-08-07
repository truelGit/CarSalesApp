using CarSalesApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSalesApp.Data
{
	public class CarSalesContext : DbContext
	{
		public DbSet<Brand> Brands => Set<Brand>();
		public DbSet<Model> Models => Set<Model>();
		public DbSet<Order> Orders => Set<Order>();

		public string DbPath { get; }

		public CarSalesContext()
		{
			var folder = Environment.CurrentDirectory;
			DbPath = Path.Combine(folder, "carsales.db");
			Database.EnsureCreated();
		}

		protected override void OnConfiguring(DbContextOptionsBuilder options)
			=> options.UseSqlite($"Data Source={DbPath}");
	}
}
