using System.Data;
using Magazyn.Model;
using Magazyn.Repository;

namespace Magazyn.Service;

public class WarehouseService : IWarehouseService {
    private IWarehouseRepository _warehouseRepository;

    public WarehouseService(IWarehouseRepository warehouseRepository) {
        _warehouseRepository = warehouseRepository;
    }

    public int? PostProductWarehouse(ProductWarehouse productWarehouse) {
        var price = _warehouseRepository.GetProduct(productWarehouse.IdProduct);
        if (price is null) {
            //product does not exist
        }

        if (_warehouseRepository.GetWarehouse(productWarehouse.IdWarehouse) != 1) {
            //warehouse does not exist
        }

        var idOrder = _warehouseRepository.GetOrder(productWarehouse.IdProduct, productWarehouse.Amount,
            productWarehouse.CreatedAt);
        if (idOrder is null) {
            //no order with given params
        }

        _warehouseRepository.UpdateOrder(idOrder, productWarehouse.CreatedAt);

        return _warehouseRepository.InsertProductWarehouse(productWarehouse, idOrder, price);
    }
}