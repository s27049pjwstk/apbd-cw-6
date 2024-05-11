using Magazyn.Model;
using Magazyn.Service;
using Microsoft.AspNetCore.Mvc;

namespace Magazyn.Controller;

[Route("api/[controller]")]
[ApiController]
public class WarehouseController : ControllerBase {
    private readonly IWarehouseService _warehouseService;

    public WarehouseController(IWarehouseService warehouseService) {
        _warehouseService = warehouseService;
    }

    [HttpPost]
    public async Task<IActionResult> tempProductWarehouse(ProductWarehouse productWarehouse) {
        var result = await _warehouseService.PostProductWarehouse(productWarehouse);
        return result is null
            ? StatusCode(StatusCodes.Status400BadRequest)
            : StatusCode(StatusCodes.Status201Created);
    }
}