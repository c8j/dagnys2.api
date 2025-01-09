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
            dataContext.Phones.Any() ||
            dataContext.PhoneTypes.Any() ||
            dataContext.ProductTypes.Any() ||
            dataContext.Products.Any() ||
            dataContext.Suppliers.Any()
        ) return;

        // var addresses = JsonSerializer.Deserialize<List<Address>>(File.ReadAllText("Data/json/addresses.json"), Options);
        // var addressTypes = JsonSerializer.Deserialize<List<AddressType>>(File.ReadAllText("Data/json/addressTypes.json"), Options);
        // var phones = JsonSerializer.Deserialize<List<Phone>>(File.ReadAllText("Data/json/phones.json"), Options);
        // var phoneTypes = JsonSerializer.Deserialize<List<PhoneType>>(File.ReadAllText("Data/json/phoneTypes.json"), Options);
        // var productTypes = JsonSerializer.Deserialize<List<ProductType>>(File.ReadAllText("Data/json/productTypes.json"), Options);
        // var products = JsonSerializer.Deserialize<List<Product>>(File.ReadAllText("Data/json/products.json"), Options);
        var suppliers = JsonSerializer.Deserialize<List<Supplier>>(File.ReadAllText("Data/json/suppliers.json"), Options);

        if (
            /* addresses is not null && addresses.Count > 0 &&
            addressTypes is not null && addressTypes.Count > 0 &&
            phones is not null && phones.Count > 0 &&
            phoneTypes is not null && phoneTypes.Count > 0 &&
            productTypes is not null && productTypes.Count > 0 &&
            products is not null && products.Count > 0 && */
            suppliers is not null && suppliers.Count > 0
            )
        {
            // await dataContext.Addresses.AddRangeAsync(addresses);
            // await dataContext.AddressTypes.AddRangeAsync(addressTypes);
            // await dataContext.Phones.AddRangeAsync(phones);
            // await dataContext.PhoneTypes.AddRangeAsync(phoneTypes);
            // await dataContext.ProductTypes.AddRangeAsync(productTypes);
            // await dataContext.Products.AddRangeAsync(products);
            await dataContext.Suppliers.AddRangeAsync(suppliers);
            await dataContext.SaveChangesAsync();
        }
    }
}
