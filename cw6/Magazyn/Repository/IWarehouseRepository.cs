using Magazyn.Model;

namespace Magazyn.Repository;

public interface IWarehouseRepository {
    int? GetProduct(int id);
    int GetWarehouse(int id);
    int? GetOrder(int idProduct, int amount, DateTime createdAt);
    int UpdateOrder(int? idOrder, DateTime createdAt);
    int? InsertProductWarehouse(ProductWarehouse productWarehouse, int? idOrder, int? price);
}