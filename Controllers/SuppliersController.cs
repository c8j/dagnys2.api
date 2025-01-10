using dagnys2.api.Data;
using dagnys2.api.Entities;

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
        public async Task<ActionResult<List<Supplier>>> GetSuppliers()
        {
            var suppliers = await _dataContext.Suppliers
            .Include(context => context.SupplierAddresses)
            .Include(context => context.SupplierPhones)
            .Include(context => context.SupplierProducts)
            .Select(supplier => new
            {
                SupplierID = supplier.ID,
                supplier.Name,
                supplier.ContactName,
                supplier.Email,
                Addresses = supplier.SupplierAddresses
                .Select(supplierAddress => new
                {
                    AddressType = supplierAddress.AddressType.Name,
                    supplierAddress.Address.StreetLine,
                    supplierAddress.Address.PostalCode,
                    supplierAddress.Address.City
                }),
                Phones = supplier.SupplierPhones
                .Select(supplierPhones => new
                {
                    PhoneType = supplierPhones.PhoneType.Name,
                    supplierPhones.Phone.Number
                }),
                Products = supplier.SupplierProducts
                .Select(supplierProduct => new
                {
                    supplierProduct.Product.ItemNumber,
                    ProductType = supplierProduct.ProductType.Name,
                    supplierProduct.Product.Price
                })
            })
            .AsSplitQuery()
            .ToListAsync();
            return Ok(suppliers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Supplier>> GetSupplier(int id)
        {
            var supplier = await _dataContext.Suppliers
            .Where(supplier => supplier.ID == id)
            .Include(context => context.SupplierAddresses)
            .Include(context => context.SupplierPhones)
            .Include(context => context.SupplierProducts)
            .Select(supplier => new
            {
                SupplierID = supplier.ID,
                supplier.Name,
                supplier.ContactName,
                supplier.Email,
                Addresses = supplier.SupplierAddresses
                .Select(supplierAddress => new
                {
                    AddressType = supplierAddress.AddressType.Name,
                    supplierAddress.Address.StreetLine,
                    supplierAddress.Address.PostalCode,
                    supplierAddress.Address.City
                }),
                Phones = supplier.SupplierPhones
                .Select(supplierPhones => new
                {
                    PhoneType = supplierPhones.PhoneType.Name,
                    supplierPhones.Phone.Number
                }),
                Products = supplier.SupplierProducts
                .Select(supplierProduct => new
                {
                    supplierProduct.Product.ItemNumber,
                    ProductType = supplierProduct.ProductType.Name,
                    supplierProduct.Product.Price
                })
            })
            .AsSplitQuery()
            .FirstOrDefaultAsync();

            return supplier is null ?
            (ActionResult<Supplier>)NotFound($"Kunde inte hitta någon leverantör med id {id}") :
            (ActionResult<Supplier>)Ok(supplier);
        }
    }
}
