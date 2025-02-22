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
            dataContext.Ingredients.Any() ||
            dataContext.Suppliers.Any() ||
            dataContext.SupplierAddresses.Any() ||
            dataContext.SupplierPhones.Any() ||
            dataContext.SupplierIngredients.Any()
        ) return;

        var addresses = JsonSerializer.Deserialize<List<Address>>(File.ReadAllText("Data/json/addresses.json"), Options);
        var addressTypes = JsonSerializer.Deserialize<List<AddressType>>(File.ReadAllText("Data/json/addressTypes.json"), Options);
        var phones = JsonSerializer.Deserialize<List<Phone>>(File.ReadAllText("Data/json/phones.json"), Options);
        var phoneTypes = JsonSerializer.Deserialize<List<PhoneType>>(File.ReadAllText("Data/json/phoneTypes.json"), Options);
        var ingredients = JsonSerializer.Deserialize<List<Ingredient>>(File.ReadAllText("Data/json/ingredients.json"), Options);
        var suppliers = JsonSerializer.Deserialize<List<Supplier>>(File.ReadAllText("Data/json/suppliers.json"), Options);
        var supplierAddresses = JsonSerializer.Deserialize<List<SupplierAddress>>(File.ReadAllText("Data/json/supplierAddresses.json"), Options);
        var supplierPhones = JsonSerializer.Deserialize<List<SupplierPhone>>(File.ReadAllText("Data/json/supplierPhones.json"), Options);
        var supplierIngredients = JsonSerializer.Deserialize<List<SupplierIngredient>>(File.ReadAllText("Data/json/supplierIngredients.json"), Options);

        if (
            addresses is not null && addresses.Count > 0 &&
            addressTypes is not null && addressTypes.Count > 0 &&
            phones is not null && phones.Count > 0 &&
            phoneTypes is not null && phoneTypes.Count > 0 &&
            ingredients is not null && ingredients.Count > 0 &&
            suppliers is not null && suppliers.Count > 0 &&
            supplierAddresses is not null && supplierAddresses.Count > 0 &&
            supplierPhones is not null && supplierPhones.Count > 0 &&
            supplierIngredients is not null && supplierIngredients.Count > 0
            )
        {
            dataContext.AddressTypes.AddRange(addressTypes);
            dataContext.Addresses.AddRange(addresses);
            dataContext.PhoneTypes.AddRange(phoneTypes);
            dataContext.Phones.AddRange(phones);
            dataContext.Ingredients.AddRange(ingredients);
            dataContext.Suppliers.AddRange(suppliers);
            await dataContext.SaveChangesAsync();
            dataContext.SupplierAddresses.AddRange(supplierAddresses);
            dataContext.SupplierPhones.AddRange(supplierPhones);
            dataContext.SupplierIngredients.AddRange(supplierIngredients);
            await dataContext.SaveChangesAsync();
        }
    }
}
