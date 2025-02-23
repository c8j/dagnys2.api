using dagnys2.api.Data;
using dagnys2.api.Entities;
using dagnys2.api.ViewModels.Batch;
using dagnys2.api.ViewModels.Entity;
using dagnys2.api.ViewModels.Product;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dagnys2.api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController(DataContext dataContext) : ControllerBase
{
    private readonly DataContext _dataContext = dataContext;

    [HttpGet]
    [ProducesResponseType<List<SimpleProductVM>>(200)]
    public async Task<ActionResult> GetProducts()
    {
        var products = await _dataContext.Products
        .Select(p => new SimpleProductVM
        {
            ID = p.ID,
            ItemNumber = p.ItemNumber,
            Name = p.Name,
            WeightInGrams = p.WeightInGrams,
            PriceKrPerUnit = p.PriceKrPerUnit,
            AmountInPackage = p.AmountInPackage
        })
        .ToListAsync();
        return Ok(products);
    }

    [HttpGet("{id}")]
    [ProducesResponseType<ProductVM>(200)]
    [ProducesResponseType<string>(404)]
    public async Task<ActionResult> GetProduct(int id)
    {
        var product = await _dataContext.Products
            .Include(p => p.ProductBatches)
            .ThenInclude(pb => pb.Batch)
            .Include(p => p.OrderItems)
            .ThenInclude(oi => oi.Order.Customer)
            .AsSplitQuery()
            .FirstOrDefaultAsync(p => p.ID == id);

        return product is null
            ? NotFound($"Kunde inte hitta någon produkt med id {id}")
            : Ok(new
            ProductVM
            {
                ItemNumber = product.ItemNumber,
                Name = product.Name,
                WeightInGrams = product.WeightInGrams,
                PriceKrPerUnit = product.PriceKrPerUnit,
                AmountInPackage = product.AmountInPackage,
                Batches = [.. product.ProductBatches
                    .Select(pb => new ProductBatchVM
                    {
                        ID = pb.BatchID,
                        ManufactureDate = pb.Batch.ManufactureDate,
                        ExpirationDate = pb.Batch.ExpirationDate,
                        Quantity = pb.Quantity
                    })],
                Customers = [.. product.OrderItems
                    .Select(oi => new SimpleEntityVM{
                        ID = oi.Order.CustomerID,
                        Name = oi.Order.Customer.Name,
                        ContactName = oi.Order.Customer.ContactName,
                        Email = oi.Order.Customer.Email
                    })]
            });
    }

    [HttpPost]
    [ProducesResponseType<ProductVM>(201)]
    [ProducesResponseType<string>(400)]
    public async Task<ActionResult> CreateProduct(ProductPostVM productPostVM)
    {
        var product = await _dataContext.Products.FirstOrDefaultAsync(p => p.ItemNumber == productPostVM.ItemNumber);
        if (product is not null)
        {
            return BadRequest($"Produkten med produktnummer {productPostVM.ItemNumber} finns redan.");
        }

        var newProduct = new Product
        {
            ItemNumber = productPostVM.ItemNumber,
            Name = productPostVM.Name,
            WeightInGrams = productPostVM.WeightInGrams,
            PriceKrPerUnit = productPostVM.PriceKrPerUnit,
            AmountInPackage = productPostVM.AmountInPackage
        };

        try
        {
            _dataContext.Products.Add(newProduct);
            await _dataContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduct), new { id = newProduct.ID }, new
            ProductVM
            {
                ItemNumber = newProduct.ItemNumber,
                Name = newProduct.Name,
                WeightInGrams = newProduct.WeightInGrams,
                PriceKrPerUnit = newProduct.PriceKrPerUnit,
                AmountInPackage = newProduct.AmountInPackage
            });
        }
        catch (Exception ex)
        {
            return BadRequest($"Något gick fel: {ex.Message}");
        }
    }

    [HttpPatch("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType<string>(404)]
    public async Task<ActionResult> ChangePrice(int id, ProductPricePatchVM productPricePatchVM)
    {
        var product = await _dataContext.Products
        .FirstOrDefaultAsync(i => i.ID == id);

        if (product is null) return NotFound($"Kunde inte hitta någon produkt med id {id}");

        product.PriceKrPerUnit = productPricePatchVM.PriceKrPerUnit;
        await _dataContext.SaveChangesAsync();
        return NoContent();
    }
}
