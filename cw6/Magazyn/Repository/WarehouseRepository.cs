using Magazyn.Model;
using Microsoft.Data.SqlClient;

namespace Magazyn.Repository;

public class WarehouseRepository : IWarehouseRepository {
    private IConfiguration _configuration;

    private string[] _tablesDefult = { "Doctor", "Patient" };

    private readonly List<string> _tables;

    public WarehouseRepository(IConfiguration configuration) {
        _configuration = configuration;
        _tables = new List<string>(_tablesDefult);
    }

    public int? GetProduct(int id) {
        await using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        await con.OpenAsync();
        await using var cmd = new SqlCommand();
        cmd.Connection = con;

        cmd.CommandText = "select Price from Product where IdProduct = @IdProduct";
        cmd.Parameters.AddWithValue("@IdProduct", id);

        var dr = await cmd.ExecuteReaderAsync();
        return !await dr.ReadAsync() ? null : (int)dr["Price"];
    }

    public int GetWarehouse(int id) {
        await using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        await con.OpenAsync();
        await using var cmd = new SqlCommand();
        cmd.Connection = con;

        cmd.CommandText = "SELECT 1 FROM Warehouse WHERE IdWarehouse = @IdWarehouse";
        cmd.Parameters.AddWithValue("@IdWarehouse", id);

        var dr = await cmd.ExecuteReaderAsync();
        return !await dr.ReadAsync() ? 0 : 1;
    }

    public int? GetOrder(int idProduct, int amount, DateTime createdAt) {
        await using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        await con.OpenAsync();
        await using var cmd = new SqlCommand();
        cmd.Connection = con;

        cmd.CommandText = """
                          SELECT o.IdOrder
                                  FROM "Order" o
                                     LEFT JOIN Product_Warehouse pw ON o.IdOrder = pw.IdOrder
                                  WHERE o.IdProduct = @IdProduct
                                    AND o.Amount = @Amount
                                    AND pw.IdProductWarehouse IS NULL
                                    AND o.CreatedAt < @CreatedAt
                                    and o.FulfilledAt is null;
                          """;
        cmd.Parameters.AddWithValue("@IdProduct", idProduct);
        cmd.Parameters.AddWithValue("@Amount", amount);
        cmd.Parameters.AddWithValue("@CreatedAt", createdAt);

        var dr = await cmd.ExecuteReaderAsync();
        return !await dr.ReadAsync() ? null : (int)dr["o.IdOrder"];
    }
    
    public int UpdateOrder(int? idOrder, DateTime createdAt) {
        await using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        await con.OpenAsync();

        await using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = """
                          UPDATE "Order"
                          SET FulfilledAt=@CreatedAt
                          WHERE IdOrder = @IdOrder;
                          """;
        cmd.Parameters.AddWithValue("@CreatedAt", createdAt);
        cmd.Parameters.AddWithValue("@IdOrder", idOrder);

        var affectedCount = await cmd.ExecuteNonQueryAsync();
        return affectedCount;
    }

    public int? InsertProductWarehouse(ProductWarehouse productWarehouse, int? idOrder, int? price) {
        await using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        await con.OpenAsync();

        await using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = """
                          INSERT INTO Product_Warehouse(IdWarehouse, IdProduct, IdOrder, Amount, Price, CreatedAt)
                          VALUES (@IdWarehouse, @IdProduct, @IdOrder, @Amount, @Price, @CreatedAt);
                          """;
        cmd.Parameters.AddWithValue("@IdWarehouse", productWarehouse.IdWarehouse);
        cmd.Parameters.AddWithValue("@IdProduct", productWarehouse.IdProduct);
        cmd.Parameters.AddWithValue("@IdOrder", idOrder);
        cmd.Parameters.AddWithValue("@Amount", productWarehouse.Amount);
        cmd.Parameters.AddWithValue("@Price", productWarehouse.Amount * price);
        cmd.Parameters.AddWithValue("@CreatedAt", productWarehouse.CreatedAt);
        
        //todo return idProductWarehouse
        //todo fixme UpdateOrder and InsertProductWarehouse as one transaction?
        //todo fixme async
        //todo fixme maybe just run the proc(2).sql idk
        //todo fixme throw HTML errors in WarehouseService

        var affectedCount = await cmd.ExecuteNonQueryAsync();
        return affectedCount;
    }
}