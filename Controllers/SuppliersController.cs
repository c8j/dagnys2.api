using dagnys2.api.Data;
using dagnys2.api.Entities;
using dagnys2.api.ViewModels.Address;
using dagnys2.api.ViewModels.Entity;
using dagnys2.api.ViewModels.Phone;
using dagnys2.api.ViewModels.Supplier;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dagnys2.api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SuppliersController(DataContext dataContext) : ControllerBase
{
    private readonly DataContext _dataContext = dataContext;

    [HttpGet]
    [ProducesResponseType<List<SimpleEntityVM>>(200)]
    public async Task<ActionResult> GetSuppliers()
    {
        var suppliers = await _dataContext.Suppliers
        .Select(supplier => new
        SimpleEntityVM
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
                // ID = supplierAddress.AddressID,
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
                // ID = supplierPhone.PhoneID,
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

    [HttpPost]
    [ProducesResponseType<SupplierVM>(201)]
    [ProducesResponseType<string>(400)]
    public async Task<ActionResult> CreateSupplier(EntityPostVM postVM)
    {
        var supplier = await _dataContext.Suppliers.FirstOrDefaultAsync(
            s => s.Email.ToLower().Trim() == postVM.Email.ToLower().Trim() ||
            s.Name.Trim() == postVM.Name.Trim()
        );

        if (supplier is not null)
        {
            return BadRequest("Leverantören finns redan.");
        }

        supplier = new Supplier
        {
            Name = postVM.Name.Trim(),
            ContactName = postVM.ContactName.Trim(),
            Email = postVM.Email.ToLower().Trim()
        };
        _dataContext.Add(supplier);
        //await _dataContext.SaveChangesAsync();

        var typeAdded = new Dictionary<int, bool>();

        foreach (var addressVM in postVM.Addresses)
        {
            var newAddressType = await _dataContext.AddressTypes.FindAsync(addressVM.TypeID);
            if (newAddressType is null)
            {
                return BadRequest($"Det finns ingen adresstyp med ID '{addressVM.TypeID}'.");
            }
            if (!typeAdded.TryAdd(newAddressType.ID, true))
            {
                return BadRequest("Vänligen ange endast en post per adresstyp.");
            }

            var newAddress = await _dataContext.Addresses.FirstOrDefaultAsync(
                a => a.PostalCode.Replace(" ", "") == addressVM.PostalCode.Replace(" ", "")
            );
            if (newAddress is null)
            {
                newAddress = new Address
                {
                    StreetLine = addressVM.StreetLine.Trim(),
                    PostalCode = addressVM.PostalCode.Replace(" ", ""),
                    City = addressVM.City.Trim()
                };
                _dataContext.Add(newAddress);
            }

            var newEntityAddress = new EntityAddress
            {
                Entity = supplier,
                Address = newAddress,
                AddressType = newAddressType
            };
            _dataContext.Add(newEntityAddress);
        }
        //await _dataContext.SaveChangesAsync();

        typeAdded.Clear();

        foreach (var phoneVM in postVM.Phones)
        {
            var newPhoneType = await _dataContext.PhoneTypes.FindAsync(phoneVM.TypeID);
            if (newPhoneType is null)
            {
                return BadRequest($"Det finns ingen telefontyp med ID '{phoneVM.TypeID}'.");
            }
            if (!typeAdded.TryAdd(newPhoneType.ID, true))
            {
                return BadRequest("Vänligen ange endast en post per telefontyp.");
            }

            var newPhone = await _dataContext.Phones.FirstOrDefaultAsync(
                a => a.Number.Replace(" ", "") == phoneVM.Number.Replace(" ", "")
            );
            if (newPhone is null)
            {
                newPhone = new Phone
                {
                    Number = phoneVM.Number.Replace(" ", "")
                };
                _dataContext.Add(newPhone);
            }

            var newEntityPhone = new EntityPhone
            {
                Entity = supplier,
                Phone = newPhone,
                PhoneType = newPhoneType
            };
            _dataContext.Add(newEntityPhone);
        }
        await _dataContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetSupplier), new { id = supplier.ID }, new SupplierVM
        {
            Name = supplier.Name,
            ContactName = supplier.ContactName,
            Email = supplier.Email
        });
    }

    [HttpPatch("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType<string>(404)]
    public async Task<ActionResult> ChangeContact(int id, EntityPatchVM patchVM)
    {
        var supplier = await _dataContext.Suppliers
        .FirstOrDefaultAsync(i => i.ID == id);

        if (supplier is null) return NotFound($"Kunde inte hitta någon leverantör med id {id}");

        supplier.ContactName = patchVM.ContactName.Trim();
        await _dataContext.SaveChangesAsync();
        return NoContent();
    }
}
