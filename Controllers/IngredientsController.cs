using dagnys2.api.Data;
using dagnys2.api.Entities;
using dagnys2.api.ViewModels.Ingredient;
using dagnys2.api.ViewModels.IngredientSupplier;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dagnys2.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngredientsController(DataContext dataContext) : ControllerBase
    {
        private readonly DataContext _dataContext = dataContext;

        [HttpGet]
        [ProducesResponseType<List<SimpleIngredientVM>>(200)]
        public async Task<ActionResult> GetIngredients()
        {
            var ingredients = await _dataContext.Ingredients
            .Select(ingredient => new SimpleIngredientVM
            {
                ID = ingredient.ID,
                ItemNumber = ingredient.ItemNumber,
                Name = ingredient.Name,

            })
            .ToListAsync();
            return Ok(ingredients);
        }

        [HttpGet("{id}")]
        [ProducesResponseType<IngredientVM>(200)]
        [ProducesResponseType<string>(404)]
        public async Task<ActionResult> GetIngredient(int id)
        {
            var ingredient = await _dataContext.Ingredients
            .Include(i => i.SupplierIngredients)
            .ThenInclude(si => si.Supplier)
            .AsSplitQuery()
            .FirstOrDefaultAsync(ingredient => ingredient.ID == id);

            return ingredient is null
                ? NotFound($"Kunde inte hitta någon ingrediens med id {id}")
                : Ok(new
                IngredientVM
                {
                    ItemNumber = ingredient.ItemNumber,
                    Name = ingredient.Name,
                    Suppliers = ingredient.SupplierIngredients
                    .Select(si => new
                    IngredientSupplierVM
                    {
                        SupplierID = si.SupplierID,
                        SupplierName = si.Supplier.Name,
                        Price = si.Price
                    }).ToList()
                });
        }

        [HttpPost]
        [ProducesResponseType<IngredientVM>(201)]
        [ProducesResponseType<string>(400)]
        public async Task<ActionResult> CreateIngredient(IngredientPostVM ingredientPostVM)
        {
            var ingredient = await _dataContext.Ingredients.FirstOrDefaultAsync(ingredient => ingredient.ItemNumber == ingredientPostVM.ItemNumber);
            if (ingredient is not null)
            {
                return BadRequest($"Ingrediensen med produktnummer {ingredientPostVM.ItemNumber} finns redan.");
            }

            var newIngredient = new Ingredient
            {
                ItemNumber = ingredientPostVM.ItemNumber,
                Name = ingredientPostVM.Name
            };

            try
            {
                _dataContext.Ingredients.Add(newIngredient);
                await _dataContext.SaveChangesAsync();

                return CreatedAtAction(nameof(GetIngredient), new { id = newIngredient.ID }, new
                IngredientVM
                {
                    ItemNumber = newIngredient.ItemNumber,
                    Name = newIngredient.Name
                });
            }
            catch (Exception ex)
            {
                return BadRequest($"Något gick fel: {ex.Message}");
            }
        }

        [HttpPatch("{id}/suppliers")]
        [ProducesResponseType(204)]
        [ProducesResponseType<string>(404)]
        public async Task<ActionResult> UpdateIngredient(int id, IngredientSuppliersPatchVM patchVM)
        {
            var ingredient = await _dataContext.Ingredients
            .FirstOrDefaultAsync(i => i.ID == id);

            if (ingredient is null) return NotFound($"Kunde inte hitta någon ingrediens med id {id}");

            await _dataContext.SupplierIngredients.Where(si => si.IngredientID == ingredient.ID).ExecuteDeleteAsync();

            foreach (var supplierVM in patchVM.SuppliersVMs)
            {
                var supplierExists = await _dataContext.Suppliers.AnyAsync(s => s.ID == supplierVM.SupplierID);
                if (!supplierExists) return NotFound($"Kunde inte hitta någon leverantör med id {supplierVM.SupplierID}");

                var newSupplierIngredient = new SupplierIngredient
                {
                    IngredientID = ingredient.ID,
                    SupplierID = supplierVM.SupplierID,
                    Price = supplierVM.Price
                };

                _dataContext.Add(newSupplierIngredient);
            }

            await _dataContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType<string>(404)]
        public async Task<ActionResult> ChangePrice(int id, IngredientPricePatchVM ingredientPricePatchVM)
        {
            var ingredient = await _dataContext.Ingredients
            .FirstOrDefaultAsync(i => i.ID == id);

            if (ingredient is null) return NotFound($"Kunde inte hitta någon ingrediens med id {id}");

            var supplierIngredient = await _dataContext.SupplierIngredients
            .FirstOrDefaultAsync(si => si.SupplierID == ingredientPricePatchVM.SupplierID);

            if (supplierIngredient is null) return NotFound($"Kunde inte hitta någon ingrediens med id {id} som säljs av leverantör med id {ingredientPricePatchVM.SupplierID}.");

            supplierIngredient.Price = ingredientPricePatchVM.Price;
            await _dataContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
