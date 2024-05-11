using Magazyn.Model;

namespace Magazyn.Service;

public interface IWarehouseService {
    Task<int?> PostProductWarehouse(ProductWarehouse productWarehouse);
}