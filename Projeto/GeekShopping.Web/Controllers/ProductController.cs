using GeekShopping.Web.Models;
using GeekShopping.Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace GeekShopping.Web.Controllers;
public class ProductController(IProductService _service) : Controller
{
    public async Task<IActionResult> ProductIndex()
    {
        var products = await _service.FindAllProducts();

        return View(products);
    }

    public ActionResult ProductCreate() => View();

    [HttpPost]
    public async Task<IActionResult> ProductCreate(ProductModel product) 
    {
        if (ModelState.IsValid)
        {
            var response = await _service.CreateProduct(product);

            if(response is not null) 
            {
                return RedirectToAction(nameof(ProductIndex));
            }
        }

        return View(product);
    }    
    
    [HttpGet]
    public async Task<IActionResult> ProductUpdate(int id) 
    {
        var product = await _service.FindProductById(id);

        if(product is not null)
        {
            return View(product);
        }

        return NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> ProductUpdate(ProductModel product)
    {
        if(ModelState.IsValid)
        {
            var response = await _service.UpdateProduct(product);

            if (response is not null)
                return RedirectToAction(nameof(ProductIndex));

        }
        return View(product);
    }

    [HttpGet]
    public async Task<IActionResult> ProductDelete(int id)
    {
        var product = await _service.FindProductById(id);

        if (product is not null)
        {
            return View(product);
        }

        return NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> ProductDelete(ProductModel product)
    {
        if (ModelState.IsValid)
        {
            var response = await _service.DeleteProductBydId(product.Id);

            if (response)
                return RedirectToAction(nameof(ProductIndex));
        }

        return View(product);
    }
}
