using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StoreManager.Web.Interfaces;
using StoreManager.Web.Models;
using System.Text;

namespace StoreManager.Web.Controllers;

public class ProductController : Controller
{
    private readonly IApiHelper _apiHelper;
    private readonly string _apiEndpoint;

    public ProductController(IApiHelper apiHelper, IConfiguration configuration)
    {
        _apiEndpoint = configuration["ApiEndpoints:ProductEndpoint"] ?? throw new InvalidOperationException("ApiSettings:ProductEndpoint is not configured");
        _apiHelper = apiHelper;
    }

    public async Task<IActionResult> Index()
    {
        var (isSuccess, products) = await _apiHelper.SendRequest<IEnumerable<ProductModel>>(_apiEndpoint, HttpMethod.Get);

        if (!isSuccess)
        {
            return BadRequest(products);
        }

        return View(products);
    }

    public async Task<IActionResult> Details(int id)
    {
        var (isSuccess, product) = await _apiHelper.SendRequest<ProductModel>($"{_apiEndpoint}/{id}", HttpMethod.Get);

        if (!isSuccess)
        {
            return NotFound();
        }

        return View(product);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(ProductModel model)
    {
        if (ModelState.IsValid)
        {
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var (editSuccess, _) = await _apiHelper.SendRequest<object>($"{_apiEndpoint}/{model.Id}", HttpMethod.Put, content);

            if (!editSuccess)
            {
                return BadRequest(new { Model = model, Message = "Invalid model" });
            }
        }

        return RedirectToAction("Details", new { id = model.Id });
    }

    [HttpGet]
    public IActionResult Add() => View();

    [HttpPost]
    public async Task<IActionResult> Add(ProductModel model)
    {
        if (ModelState.IsValid)
        {
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var (addSuccess, _) = await _apiHelper.SendRequest<object>(_apiEndpoint, HttpMethod.Post, content);

            if (!addSuccess)
            {
                return BadRequest(new { Model = model, Message = "Invalid model" });
            }
        }

        return RedirectToAction("Index");
    }
}
