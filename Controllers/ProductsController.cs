using dagnys2.api.Data;
using dagnys2.api.Entities;
using dagnys2.api.ViewModels;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dagnys2.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(DataContext dataContext) : ControllerBase
    {
        private readonly DataContext _dataContext = dataContext;

        [HttpGet]
        public async Task<ActionResult> GetProductTypes()
        {
            var productTypes = await _dataContext.ProductTypes
            .Select(productType => new
            {
                productType.ID,
                productType.Name
            })
            .ToListAsync();
            return Ok(productTypes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetProductType(int id)
        {
            var productType = await _dataContext.ProductTypes.FindAsync(id);

            if (productType is null) return NotFound($"Kunde inte hitta någon produktkategori med id {id}");

            var suppliers = await _dataContext.SupplierProducts
            .Where(supplierProduct => supplierProduct.ProductTypeID == productType.ID)
            .Select(supplierProduct => new
            {
                supplierProduct.SupplierID,
                supplierProduct.Supplier.Name,
                supplierProduct.Product.ItemNumber,
                supplierProduct.Product.Price
            }).ToListAsync();

            return Ok(new
            {
                Type = productType.Name,
                Suppliers = suppliers
            });
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> ChangePrice(int id, ProductPricePatch newProductPatch)
        {
            var productType = await _dataContext.ProductTypes.FindAsync(id);
            if (productType is null) return NotFound($"Kunde inte hitta någon produktkategori med id {id}");

            var supplier = await _dataContext.Suppliers.FindAsync(newProductPatch.SupplierID);
            if (supplier is null) return NotFound($"Kunde inte hitta någon leverantör med id {id}");

            var supplierProduct = await _dataContext.SupplierProducts
            .Where(supplierProduct => supplierProduct.ProductTypeID == id && supplierProduct.SupplierID == newProductPatch.SupplierID)
            .Include(supplierProduct => supplierProduct.Product)
            .FirstOrDefaultAsync();
            if (supplierProduct is null) return NotFound($"Kunde inte hitta någon produkt av typ {productType.Name} som säljs av leverantören med id {newProductPatch.SupplierID}");

            supplierProduct.Product.Price = newProductPatch.Price;
            await _dataContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
