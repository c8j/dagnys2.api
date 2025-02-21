using dagnys2.api.Data;
using dagnys2.api.Entities;
using dagnys2.api.ViewModels;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dagnys2.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngredientsController(DataContext dataContext) : ControllerBase
    {
        private readonly DataContext _dataContext = dataContext;

        [HttpGet("types")]
        public async Task<ActionResult> GetIngredientTypes()
        {
            var ingredientTypes = await _dataContext.IngredientTypes
            .Select(ingredientType => new
            {
                ingredientType.ID,
                ingredientType.Name
            })
            .ToListAsync();
            return Ok(ingredientTypes);
        }

        [HttpGet("types/{typeID}")]
        public async Task<ActionResult> GetIngredientType(int typeID)
        {
            var ingredientType = await _dataContext.IngredientTypes.FindAsync(typeID);

            if (ingredientType is null) return NotFound($"Kunde inte hitta någon ingredienskategori med id {typeID}");

            var suppliers = await _dataContext.SupplierIngredients
            .Where(supplierIngredient => supplierIngredient.IngredientTypeID == ingredientType.ID)
            .Select(supplierIngredient => new
            {
                supplierIngredient.SupplierID,
                supplierIngredient.Supplier.Name,
                supplierIngredient.Ingredient.ItemNumber,
                supplierIngredient.Ingredient.Price
            }).ToListAsync();

            return Ok(new
            {
                Type = ingredientType.Name,
                Suppliers = suppliers
            });
        }

        [HttpPost]
        public async Task<ActionResult> AddIngredientToSupplier(IngredientPostVM ingredientPostVM)
        {
            var ingredientType = await _dataContext.IngredientTypes.FirstOrDefaultAsync(ingredientType => ingredientType.Name == ingredientPostVM.Type);
            if (ingredientType is null)
            {
                ingredientType = new IngredientType()
                {
                    Name = ingredientPostVM.Type
                };
                try
                {
                    await _dataContext.IngredientTypes.AddAsync(ingredientType);
                    await _dataContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return BadRequest($"Något gick fel: {ex.Message}");
                }
            }

            var supplier = await _dataContext.Suppliers.FirstOrDefaultAsync(supplier => supplier.ID == ingredientPostVM.SupplierID);
            if (supplier is null) return BadRequest($"Kunde inte hitta någon leverantör med id {ingredientPostVM.SupplierID}. Stavade du fel?");

            var supplierIngredients = await _dataContext.SupplierIngredients
                .Where(supplierIngredient => supplierIngredient.SupplierID == supplier.ID)
                .Include(supplierIngredient => supplierIngredient.IngredientType)
                .ToListAsync();

            if (supplierIngredients.Any(supplierIngredient => supplierIngredient.IngredientType.Name == ingredientType.Name)) return BadRequest($"Ingrediensen redan finns hos leverantören.");

            var newIngredient = new Ingredient
            {
                ItemNumber = ingredientPostVM.ItemNumber,
                Price = ingredientPostVM.Price
            };

            try
            {
                await _dataContext.Ingredients.AddAsync(newIngredient);
                await _dataContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest($"Något gick fel: {ex.Message}");
            }

            var newSupplierIngredient = new SupplierIngredient
            {
                Supplier = supplier,
                Ingredient = newIngredient,
                IngredientType = ingredientType
            };

            try
            {
                await _dataContext.SupplierIngredients.AddAsync(newSupplierIngredient);
                await _dataContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest($"Något gick fel: {ex.Message}");
            }

            return CreatedAtAction(nameof(GetIngredientType), new { typeID = newIngredient.ID }, new
            {
                ingredientType.Name,
                SupplierID = supplier.ID,
                SupplierName = supplier.Name,
                newIngredient.ItemNumber,
                newIngredient.Price
            });
        }

        [HttpPatch("{typeID}")]
        public async Task<ActionResult> ChangePrice(int typeID, IngredientPricePatchVM ingredientPricePatchVM)
        {
            var ingredientType = await _dataContext.IngredientTypes.FindAsync(typeID);
            if (ingredientType is null) return NotFound($"Kunde inte hitta någon ingredienskategori med id {typeID}");

            var supplier = await _dataContext.Suppliers.FindAsync(ingredientPricePatchVM.SupplierID);
            if (supplier is null) return NotFound($"Kunde inte hitta någon leverantör med id {ingredientPricePatchVM.SupplierID}");

            var supplierIngredient = await _dataContext.SupplierIngredients
            .Where(supplierIngredient => supplierIngredient.IngredientTypeID == typeID && supplierIngredient.SupplierID == ingredientPricePatchVM.SupplierID)
            .Include(supplierIngredient => supplierIngredient.Ingredient)
            .FirstOrDefaultAsync();
            if (supplierIngredient is null) return NotFound($"Kunde inte hitta någon ingrediens av typ {ingredientType.Name} som säljs av leverantören med id {ingredientPricePatchVM.SupplierID}");

            supplierIngredient.Ingredient.Price = ingredientPricePatchVM.Price;
            await _dataContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
