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

        [HttpGet("{typeID}")]
        public async Task<ActionResult> GetProductType(int typeID)
        {
            var productType = await _dataContext.ProductTypes.FindAsync(typeID);

            if (productType is null) return NotFound($"Kunde inte hitta någon produktkategori med id {typeID}");

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

        [HttpPost]
        public async Task<ActionResult> AddProductToSupplier(ProductPost productPost)
        {
            var productType = await _dataContext.ProductTypes.FirstOrDefaultAsync(productType => productType.Name == productPost.Type);
            if (productType is null)
            {
                productType = new ProductType()
                {
                    Name = productPost.Type
                };
                try
                {
                    await _dataContext.ProductTypes.AddAsync(productType);
                    await _dataContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return BadRequest($"Något gick fel: {ex.Message}");
                }
            }

            var supplier = await _dataContext.Suppliers.FirstOrDefaultAsync(supplier => supplier.ID == productPost.SupplierID);
            if (supplier is null) return BadRequest($"Kunde inte hitta någon leverantör med id {productPost.SupplierID}. Stavade du fel?");

            var supplierProducts = await _dataContext.SupplierProducts
                .Where(supplierProduct => supplierProduct.SupplierID == supplier.ID)
                .Include(supplierProduct => supplierProduct.ProductType)
                .ToListAsync();

            if (supplierProducts.Any(supplierProduct => supplierProduct.ProductType.Name == productType.Name)) return BadRequest($"Produkten redan finns hos leverantören.");

            var newProduct = new Product
            {
                ItemNumber = productPost.ItemNumber,
                Price = productPost.Price
            };

            try
            {
                await _dataContext.Products.AddAsync(newProduct);
                await _dataContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest($"Något gick fel: {ex.Message}");
            }

            var newSupplierProduct = new SupplierProduct
            {
                Supplier = supplier,
                Product = newProduct,
                ProductType = productType
            };

            try
            {
                await _dataContext.SupplierProducts.AddAsync(newSupplierProduct);
                await _dataContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest($"Något gick fel: {ex.Message}");
            }

            return CreatedAtAction(nameof(GetProductType), new { typeID = newProduct.ID }, new
            {
                productType.Name,
                SupplierID = supplier.ID,
                SupplierName = supplier.Name,
                newProduct.ItemNumber,
                newProduct.Price
            });
        }

        [HttpPatch("{typeID}")]
        public async Task<ActionResult> ChangePrice(int typeID, ProductPricePatch newProductPatch)
        {
            var productType = await _dataContext.ProductTypes.FindAsync(typeID);
            if (productType is null) return NotFound($"Kunde inte hitta någon produktkategori med id {typeID}");

            var supplier = await _dataContext.Suppliers.FindAsync(newProductPatch.SupplierID);
            if (supplier is null) return NotFound($"Kunde inte hitta någon leverantör med id {typeID}");

            var supplierProduct = await _dataContext.SupplierProducts
            .Where(supplierProduct => supplierProduct.ProductTypeID == typeID && supplierProduct.SupplierID == newProductPatch.SupplierID)
            .Include(supplierProduct => supplierProduct.Product)
            .FirstOrDefaultAsync();
            if (supplierProduct is null) return NotFound($"Kunde inte hitta någon produkt av typ {productType.Name} som säljs av leverantören med id {newProductPatch.SupplierID}");

            supplierProduct.Product.Price = newProductPatch.Price;
            await _dataContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
