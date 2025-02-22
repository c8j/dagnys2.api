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
            dataContext.EntityAddresses.Any() ||
            dataContext.EntityPhones.Any() ||
            dataContext.SupplierIngredients.Any()
        ) return;

        var addresses = JsonSerializer.Deserialize<List<Address>>(File.ReadAllText("Data/json/addresses.json"), Options);
        var addressTypes = JsonSerializer.Deserialize<List<AddressType>>(File.ReadAllText("Data/json/addressTypes.json"), Options);
        var phones = JsonSerializer.Deserialize<List<Phone>>(File.ReadAllText("Data/json/phones.json"), Options);
        var phoneTypes = JsonSerializer.Deserialize<List<PhoneType>>(File.ReadAllText("Data/json/phoneTypes.json"), Options);
        var ingredients = JsonSerializer.Deserialize<List<Ingredient>>(File.ReadAllText("Data/json/ingredients.json"), Options);
        var suppliers = JsonSerializer.Deserialize<List<Supplier>>(File.ReadAllText("Data/json/suppliers.json"), Options);
        var entityAddresses = JsonSerializer.Deserialize<List<EntityAddress>>(File.ReadAllText("Data/json/entityAddresses.json"), Options);
        var entityPhones = JsonSerializer.Deserialize<List<EntityPhone>>(File.ReadAllText("Data/json/entityPhones.json"), Options);
        var supplierIngredients = JsonSerializer.Deserialize<List<SupplierIngredient>>(File.ReadAllText("Data/json/supplierIngredients.json"), Options);

        if (
            addresses is not null && addresses.Count > 0 &&
            addressTypes is not null && addressTypes.Count > 0 &&
            phones is not null && phones.Count > 0 &&
            phoneTypes is not null && phoneTypes.Count > 0 &&
            ingredients is not null && ingredients.Count > 0 &&
            suppliers is not null && suppliers.Count > 0 &&
            entityAddresses is not null && entityAddresses.Count > 0 &&
            entityPhones is not null && entityPhones.Count > 0 &&
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
            dataContext.EntityAddresses.AddRange(entityAddresses);
            dataContext.EntityPhones.AddRange(entityPhones);
            dataContext.SupplierIngredients.AddRange(supplierIngredients);
            await dataContext.SaveChangesAsync();
        }
    }
}
