using Magazyn.Model;

namespace Magazyn.Repository;

public interface IWarehouseRepository {
    Task<int?> GetProduct(int id);
    Task<int> GetWarehouse(int id);
    Task<int?> GetOrder(int idProduct, int amount, DateTime createdAt);
    Task<int> UpdateOrder(int? idOrder, DateTime createdAt);
    Task<int?> InsertProductWarehouse(ProductWarehouse productWarehouse, int? idOrder, int? price);
}