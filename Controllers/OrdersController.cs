using dagnys2.api.Data;
using dagnys2.api.Entities;
using dagnys2.api.ViewModels.Order;
using dagnys2.api.ViewModels.OrderItem;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dagnys2.api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdersController(DataContext dataContext) : ControllerBase
{
    private readonly DataContext _dataContext = dataContext;

    [HttpGet]
    [ProducesResponseType<List<SimpleOrderVM>>(200)]
    public async Task<ActionResult> GetOrders()
    {
        var orders = await _dataContext.Orders
        .Select(o => new
        SimpleOrderVM
        {
            ID = o.ID,
            OrderNumber = o.GeneratedNumber,
            DateCreated = o.DateCreated
        })
        .ToListAsync();
        return Ok(orders);
    }

    [HttpGet("findByNumber/{orderNumber}")]
    [ProducesResponseType<OrderVM>(200)]
    [ProducesResponseType<string>(404)]
    public async Task<ActionResult> GetOrderFromOrderNumber(int orderNumber)
    {
        var order = await _dataContext.Orders
            .Include(o => o.Customer)
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .AsSplitQuery()
            .FirstOrDefaultAsync(o => o.GeneratedNumber == orderNumber);

        return order is null
            ? NotFound($"Kunde inte hitta någon beställning med beställningsnummer {orderNumber}")
            : Ok(new
            OrderVM
            {
                OrderNumber = order.GeneratedNumber,
                DateCreated = order.DateCreated,
                CustomerID = order.CustomerID,
                CustomerName = order.Customer.Name,
                OrderItems = [..order.OrderItems.Select(
                    oi => new OrderItemVM{
                        ItemNumber = oi.Product.ItemNumber,
                        ProductName = oi.Product.Name,
                        PriceKrPerUnit = oi.Product.PriceKrPerUnit,
                        Quantity = oi.Quantity,
                        TotalPriceKr = oi.Product.PriceKrPerUnit * oi.Quantity
                    }
                )]
            });
    }

    [HttpGet("findByDate/{orderDate}")]
    [ProducesResponseType<List<OrderVM>>(200)]
    [ProducesResponseType<string>(400)]
    public async Task<ActionResult> GetOrdersFromDate(string orderDate)
    {
        if (!DateOnly.TryParse(orderDate, out var dateCreated))
        {
            return BadRequest("Kunde inte tolka datum. Har du stavat fel? (ÅÅÅÅ-MM-DD)");
        }

        var orders = await _dataContext.Orders
            .Where(o => o.DateCreated == dateCreated)
            .Include(o => o.Customer)
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .AsSplitQuery()
            .ToListAsync();

        List<OrderVM> response = [.. orders.Select(
            o => new OrderVM{
                OrderNumber = o.GeneratedNumber,
                DateCreated = o.DateCreated,
                CustomerID = o.CustomerID,
                CustomerName = o.Customer.Name,
                OrderItems = [..o.OrderItems.Select(
                    oi => new OrderItemVM{
                        ItemNumber = oi.Product.ItemNumber,
                        ProductName = oi.Product.Name,
                        PriceKrPerUnit = oi.Product.PriceKrPerUnit,
                        Quantity = oi.Quantity,
                        TotalPriceKr = oi.Product.PriceKrPerUnit * oi.Quantity
                    }
                )]
            }
        ).ToList()];

        return Ok(response);
    }

    private static int GenerateOrderNumber(
        int customerID,
        int orderID,
        int amountOfProducts
    ) => int.Parse($"{customerID % 90 + 10}{orderID % 90 + 10}{orderID}{amountOfProducts % 900 + 100}");

    [HttpPost]
    [ProducesResponseType<OrderVM>(201)]
    [ProducesResponseType<string>(400)]
    public async Task<ActionResult> CreateOrder(OrderPostVM postVM)
    {
        var customer = await _dataContext.Customers.FindAsync(postVM.CustomerID);
        if (customer is null)
        {
            return BadRequest($"Kunde inte hitta någon kund med id {postVM.CustomerID}");
        }

        var newOrder = new Order
        {
            Customer = customer,
            DateCreated = DateOnly.FromDateTime(DateTime.Now),
        };

        _dataContext.Add(newOrder);

        var productQuantities = new Dictionary<int, int>();

        foreach (var orderItem in postVM.OrderItems)
        {
            if (!productQuantities.ContainsKey(orderItem.ProductID))
            {
                productQuantities.Add(orderItem.ProductID, orderItem.Quantity);
            }
            else
            {
                productQuantities[orderItem.ProductID] += orderItem.Quantity;
            }
        }

        foreach (var productQuantity in productQuantities)
        {
            var product = await _dataContext.Products.FindAsync(productQuantity.Key);
            if (product is null) return BadRequest($"Kunde inte hitta någon produkt med ID {productQuantity.Key}");

            var newOrderItem = new OrderItem
            {
                Order = newOrder,
                Product = product,
                Quantity = productQuantity.Value
            };

            _dataContext.Add(newOrderItem);
        }

        await _dataContext.SaveChangesAsync();

        newOrder.GeneratedNumber = GenerateOrderNumber(customer.ID, newOrder.ID, postVM.OrderItems.Count);

        await _dataContext.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetOrderFromOrderNumber),
            new { orderNumber = newOrder.GeneratedNumber },
            new OrderVM
            {
                OrderNumber = newOrder.GeneratedNumber,
                DateCreated = newOrder.DateCreated,
                CustomerID = newOrder.CustomerID,
                CustomerName = newOrder.Customer.Name,
                OrderItems = [..newOrder.OrderItems.Select(
                    oi => new OrderItemVM{
                        ItemNumber = oi.Product.ItemNumber,
                        ProductName = oi.Product.Name,
                        PriceKrPerUnit = oi.Product.PriceKrPerUnit,
                        Quantity = oi.Quantity,
                        TotalPriceKr = oi.Product.PriceKrPerUnit * oi.Quantity
                    }
                )]
            }
        );
    }

}
