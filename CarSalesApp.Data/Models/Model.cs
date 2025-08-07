namespace CarSalesApp.Data.Models
{
	public class Model
	{
		public int Id { get; set; }
		public string Name { get; set; } = "";
		public int BrandId { get; set; }
		public Brand Brand { get; set; } = null!;
		public ICollection<Order> Orders { get; set; } = new List<Order>();
	}
}
