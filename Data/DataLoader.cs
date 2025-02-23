using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

using dagnys2.api.Entities;

namespace dagnys2.api.Data;

public static class DataLoader
{
    private static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
    };

    public static async Task LoadData(DataContext dataContext)
    {
        if (
            dataContext.Addresses.Any() ||
            dataContext.AddressTypes.Any() ||
            dataContext.Batches.Any() ||
            dataContext.Entities.Any() ||
            dataContext.EntityAddresses.Any() ||
            dataContext.EntityPhones.Any() ||
            dataContext.Ingredients.Any() ||
            dataContext.OrderItems.Any() ||
            dataContext.Orders.Any() ||
            dataContext.Phones.Any() ||
            dataContext.PhoneTypes.Any() ||
            dataContext.ProductBatches.Any() ||
            dataContext.Products.Any() ||
            dataContext.SupplierIngredients.Any()
        ) return;

        var addresses = JsonSerializer.Deserialize<List<Address>>(File.ReadAllText("Data/json/addresses.json"), Options);
        var addressTypes = JsonSerializer.Deserialize<List<AddressType>>(File.ReadAllText("Data/json/addressTypes.json"), Options);
        var batches = JsonSerializer.Deserialize<List<Batch>>(File.ReadAllText("Data/json/batches.json"), Options);
        var customers = JsonSerializer.Deserialize<List<Customer>>(File.ReadAllText("Data/json/customers.json"), Options);
        var entityAddresses = JsonSerializer.Deserialize<List<EntityAddress>>(File.ReadAllText("Data/json/entityAddresses.json"), Options);
        var entityPhones = JsonSerializer.Deserialize<List<EntityPhone>>(File.ReadAllText("Data/json/entityPhones.json"), Options);
        var ingredients = JsonSerializer.Deserialize<List<Ingredient>>(File.ReadAllText("Data/json/ingredients.json"), Options);
        var orderItems = JsonSerializer.Deserialize<List<OrderItem>>(File.ReadAllText("Data/json/orderItems.json"), Options);
        var orders = JsonSerializer.Deserialize<List<Order>>(File.ReadAllText("Data/json/orders.json"), Options);
        var phones = JsonSerializer.Deserialize<List<Phone>>(File.ReadAllText("Data/json/phones.json"), Options);
        var phoneTypes = JsonSerializer.Deserialize<List<PhoneType>>(File.ReadAllText("Data/json/phoneTypes.json"), Options);
        var productBatches = JsonSerializer.Deserialize<List<ProductBatch>>(File.ReadAllText("Data/json/productBatches.json"), Options);
        var products = JsonSerializer.Deserialize<List<Product>>(File.ReadAllText("Data/json/products.json"), Options);
        var supplierIngredients = JsonSerializer.Deserialize<List<SupplierIngredient>>(File.ReadAllText("Data/json/supplierIngredients.json"), Options);
        var suppliers = JsonSerializer.Deserialize<List<Supplier>>(File.ReadAllText("Data/json/suppliers.json"), Options);

        if (
            addresses is not null && addresses.Count > 0 &&
            addressTypes is not null && addressTypes.Count > 0 &&
            batches is not null && batches.Count > 0 &&
            customers is not null && customers.Count > 0 &&
            entityAddresses is not null && entityAddresses.Count > 0 &&
            entityPhones is not null && entityPhones.Count > 0 &&
            ingredients is not null && ingredients.Count > 0 &&
            orderItems is not null && orderItems.Count > 0 &&
            orders is not null && orders.Count > 0 &&
            phones is not null && phones.Count > 0 &&
            phoneTypes is not null && phoneTypes.Count > 0 &&
            products is not null && products.Count > 0 &&
            productBatches is not null && productBatches.Count > 0 &&
            suppliers is not null && suppliers.Count > 0 &&
            supplierIngredients is not null && supplierIngredients.Count > 0
            )
        {
            dataContext.Addresses.AddRange(addresses);
            dataContext.AddressTypes.AddRange(addressTypes);
            dataContext.Batches.AddRange(batches);
            dataContext.Customers.AddRange(customers);
            dataContext.Ingredients.AddRange(ingredients);
            dataContext.Orders.AddRange(orders);
            dataContext.Phones.AddRange(phones);
            dataContext.PhoneTypes.AddRange(phoneTypes);
            dataContext.Products.AddRange(products);
            dataContext.Suppliers.AddRange(suppliers);
            await dataContext.SaveChangesAsync();
            dataContext.EntityAddresses.AddRange(entityAddresses);
            dataContext.EntityPhones.AddRange(entityPhones);
            dataContext.OrderItems.AddRange(orderItems);
            dataContext.ProductBatches.AddRange(productBatches);
            dataContext.SupplierIngredients.AddRange(supplierIngredients);
            await dataContext.SaveChangesAsync();
        }
    }
}
