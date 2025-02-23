using dagnys2.api.Data;
using dagnys2.api.ViewModels.Batch;
using dagnys2.api.ViewModels.Product;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dagnys2.api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BatchesController(DataContext dataContext) : ControllerBase
{
    private readonly DataContext _dataContext = dataContext;

    [HttpGet]
    [ProducesResponseType<List<BatchVM>>(200)]
    public async Task<ActionResult> GetBatches()
    {
        var batches = await _dataContext.Batches
            .Include(b => b.ProductBatches)
            .ThenInclude(pb => pb.Product)
            .Select(b => new BatchVM
            {
                ManufactureDate = b.ManufactureDate,
                ExpirationDate = b.ExpirationDate,
                Products = b.ProductBatches.Select(
                    pb => new BatchItemVM
                    {
                        ProductID = pb.Product.ID,
                        ItemNumber = pb.Product.ItemNumber,
                        Quantity = pb.Quantity
                    }).ToList()
            })
            .AsSplitQuery()
            .ToListAsync();

        return Ok(batches);
    }
}
