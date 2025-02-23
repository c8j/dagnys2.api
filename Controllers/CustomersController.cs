using dagnys2.api.Data;
using dagnys2.api.Entities;
using dagnys2.api.ViewModels.Address;
using dagnys2.api.ViewModels.Customer;
using dagnys2.api.ViewModels.Entity;
using dagnys2.api.ViewModels.Order;
using dagnys2.api.ViewModels.Phone;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dagnys2.api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomersController(DataContext dataContext) : ControllerBase
{
    private readonly DataContext _dataContext = dataContext;

    [HttpGet]
    [ProducesResponseType<List<SimpleEntityVM>>(200)]
    public async Task<ActionResult> GetCustomers()
    {
        var customers = await _dataContext.Customers
        .Select(customer => new
        SimpleEntityVM
        {
            ID = customer.ID,
            Name = customer.Name,
            ContactName = customer.ContactName,
            Email = customer.Email,
        })
        .ToListAsync();
        return Ok(customers);
    }

    [HttpGet("{id}")]
    [ProducesResponseType<CustomerVM>(200)]
    [ProducesResponseType<string>(404)]
    public async Task<ActionResult> GetCustomer(int id)
    {
        var customer = await _dataContext.Customers
        .Where(customer => customer.ID == id)
        .Include(context => context.EntityAddresses)
        .Include(context => context.EntityPhones)
        .Include(context => context.Orders)
        .Select(customer => new
        CustomerVM
        {
            Name = customer.Name,
            ContactName = customer.ContactName,
            Email = customer.Email,
            Addresses = customer.EntityAddresses
            .Select(customerAddress => new
            AddressVM
            {
                // ID = customerAddress.AddressID,
                Type = customerAddress.AddressType.Name,
                StreetLine = customerAddress.Address.StreetLine,
                PostalCode = customerAddress.Address.PostalCode,
                City = customerAddress.Address.City
            })
            .ToList(),
            Phones = customer.EntityPhones
            .Select(customerPhone => new
            PhoneVM
            {
                // ID = customerPhone.PhoneID,
                Type = customerPhone.PhoneType.Name,
                Number = customerPhone.Phone.Number
            })
            .ToList(),
            Orders = customer.Orders
            .Select(order => new
            SimpleOrderVM
            {
                ID = order.ID,
                OrderNumber = order.GeneratedNumber,
                DateCreated = order.DateCreated
            }).ToList()
        })
        .AsSplitQuery()
        .FirstOrDefaultAsync();

        return customer is null ?
        NotFound($"Kunde inte hitta någon kund med id {id}") :
        Ok(customer);
    }

    [HttpPost]
    [ProducesResponseType<CustomerVM>(201)]
    [ProducesResponseType<string>(400)]
    public async Task<ActionResult> CreateCustomer(EntityPostVM postVM)
    {
        var customer = await _dataContext.Customers.FirstOrDefaultAsync(
            c => c.Email.ToLower().Trim() == postVM.Email.ToLower().Trim() ||
            c.Name.Trim() == postVM.Name.Trim()
        );

        if (customer is not null)
        {
            return BadRequest("Kunden finns redan.");
        }

        customer = new Customer
        {
            Name = postVM.Name.Trim(),
            ContactName = postVM.ContactName.Trim(),
            Email = postVM.Email.ToLower().Trim()
        };
        _dataContext.Add(customer);
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
                Entity = customer,
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
                Entity = customer,
                Phone = newPhone,
                PhoneType = newPhoneType
            };
            _dataContext.Add(newEntityPhone);
        }
        await _dataContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCustomer), new { id = customer.ID }, new CustomerVM
        {
            Name = customer.Name,
            ContactName = customer.ContactName,
            Email = customer.Email
        });
    }

    [HttpPatch("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType<string>(404)]
    public async Task<ActionResult> ChangeContact(int id, EntityPatchVM patchVM)
    {
        var customer = await _dataContext.Customers
        .FirstOrDefaultAsync(i => i.ID == id);

        if (customer is null) return NotFound($"Kunde inte hitta någon kund med id {id}");

        customer.ContactName = patchVM.ContactName.Trim();
        await _dataContext.SaveChangesAsync();
        return NoContent();
    }
}
