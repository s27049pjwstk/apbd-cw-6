using Magazyn.Model;

namespace Magazyn.Service;

public interface IWarehouseService {
    int? PostProductWarehouse(ProductWarehouse productWarehouse);
}