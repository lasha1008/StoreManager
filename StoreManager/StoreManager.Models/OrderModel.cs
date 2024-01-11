namespace StoreManager.Models;

public record OrderModel(
	int Id, 
	DateTime OrderDate, 
	DateTime? ShippedDate, 
	string? ShipAddress, 
	IEnumerable<OrderDetailsModel> Details);

public record OrderDetailsModel(int Id, int ProductId, decimal Price, int Quantity, decimal Discount, ProductModel Product );
