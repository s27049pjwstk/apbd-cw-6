using Magazyn.Model;
using Magazyn.Repository;

namespace Magazyn.Service;

public class WarehouseService : IWarehouseService {
    private IWarehouseRepository _warehouseRepository;

    public WarehouseService(IWarehouseRepository warehouseRepository) {
        _warehouseRepository = warehouseRepository;
    }

    public async Task<int?> PostProductWarehouse(ProductWarehouse productWarehouse) {
        var price = await _warehouseRepository.GetProduct(productWarehouse.IdProduct);
        if (price is null) {
            Console.WriteLine("Product does not exist!");
            return null;
        }

        if (await _warehouseRepository.GetWarehouse(productWarehouse.IdWarehouse) != 1) {
            Console.WriteLine("Warehouse does not exist!");
            return null;
        }

        var idOrder = await _warehouseRepository.GetOrder(productWarehouse.IdProduct, productWarehouse.Amount,
            productWarehouse.CreatedAt);
        if (idOrder is null) {
            Console.WriteLine("No order found!");
            return null;
        }

        await _warehouseRepository.UpdateOrder(idOrder, productWarehouse.CreatedAt);

        return await _warehouseRepository.InsertProductWarehouse(productWarehouse, idOrder, price);
    }
}