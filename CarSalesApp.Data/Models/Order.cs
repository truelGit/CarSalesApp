
namespace CarSalesApp.Data.Models
{
	public class Order
	{
		public int Id { get; set; }
		public int ModelId { get; set; }
		public Model Model { get; set; } = null!;
		public DateTime OrderDate { get; set; }
		public decimal Price { get; set; }
	}
}
