using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace SportsStore.Controllers
{
    public class OrderController : Controller
    {
        private IOrderRepository repository;
        private Cart cart;

        public OrderController(IOrderRepository repoService, Cart cartService)
        {
            repository = repoService;
            cart = cartService;
        }

        [Authorize(Roles ="Admin,Manager")]
        //List the orders to be shipped
        public ViewResult List() => View(repository.Orders.Where(o => !o.Shipped));
        
        
        /// <summary>
        /// Best practice, do not return view, but redirects user to another view. MVC Standard
        /// Redirect to Action whenever using HTTPPost
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles ="Admin")]
        public IActionResult MarkShipped(int orderID)
        {
            Order order = repository.Orders.FirstOrDefault(o => o.OrderID == orderID);

            if (order != null)
            {
                order.Shipped = true;
                // Leveraging the method
                repository.SaveOrder(order);
            }

            return RedirectToAction(nameof(List)); // Refresh the view
        }

        public ViewResult Checkout() => View(new Order());
        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Sorry, your cart is empty!"); 
            }
            if (ModelState.IsValid)
            {
                order.Lines = cart.Lines.ToArray();
                repository.SaveOrder(order);
                //return View("Completed");
                return RedirectToAction(nameof(Completed));
            }
            else
            {
                return View(order);
            }
        }
        
        public ViewResult Completed()
        {
            cart.Clear();
            return View();
        }
    }
}