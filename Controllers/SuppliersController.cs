using dagnys2.api.Data;

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
        public async Task<ActionResult> GetAllSuppliers()
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
            }).ToListAsync();
            return Ok(suppliers);
        }
    }
}
