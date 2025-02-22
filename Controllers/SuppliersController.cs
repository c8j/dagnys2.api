using dagnys2.api.Data;
using dagnys2.api.ViewModels.Address;
using dagnys2.api.ViewModels.Phone;
using dagnys2.api.ViewModels.Supplier;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dagnys2.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuppliersController(DataContext dataContext) : ControllerBase
    {
        private readonly DataContext _dataContext = dataContext;

        [HttpGet]
        [ProducesResponseType<List<SimpleSupplierVM>>(200)]
        public async Task<ActionResult> GetSuppliers()
        {
            var suppliers = await _dataContext.Suppliers
            .Select(supplier => new
            SimpleSupplierVM
            {
                ID = supplier.ID,
                Name = supplier.Name,
                ContactName = supplier.ContactName,
                Email = supplier.Email,
            })
            .ToListAsync();
            return Ok(suppliers);
        }

        [HttpGet("{id}")]
        [ProducesResponseType<SupplierVM>(200)]
        [ProducesResponseType<string>(404)]
        public async Task<ActionResult> GetSupplier(int id)
        {
            var supplier = await _dataContext.Suppliers
            .Where(supplier => supplier.ID == id)
            .Include(context => context.EntityAddresses)
            .Include(context => context.EntityPhones)
            .Include(context => context.SupplierIngredients)
            .Select(supplier => new
            SupplierVM
            {
                Name = supplier.Name,
                ContactName = supplier.ContactName,
                Email = supplier.Email,
                Addresses = supplier.EntityAddresses
                .Select(supplierAddress => new
                AddressVM
                {
                    ID = supplierAddress.AddressID,
                    Type = supplierAddress.AddressType.Name,
                    StreetLine = supplierAddress.Address.StreetLine,
                    PostalCode = supplierAddress.Address.PostalCode,
                    City = supplierAddress.Address.City
                })
                .ToList(),
                Phones = supplier.EntityPhones
                .Select(supplierPhone => new
                PhoneVM
                {
                    ID = supplierPhone.PhoneID,
                    Type = supplierPhone.PhoneType.Name,
                    Number = supplierPhone.Phone.Number
                })
                .ToList(),
                Products = supplier.SupplierIngredients
                .Select(supplierIngredient => new
                SupplierIngredientVM
                {
                    ID = supplierIngredient.IngredientID,
                    ItemNumber = supplierIngredient.Ingredient.ItemNumber,
                    Name = supplierIngredient.Ingredient.Name,
                    PriceKrPerKg = supplierIngredient.PriceKrPerKg
                }).ToList()
            })
            .AsSplitQuery()
            .FirstOrDefaultAsync();

            return supplier is null ?
            NotFound($"Kunde inte hitta någon leverantör med id {id}") :
            Ok(supplier);
        }
    }
}
