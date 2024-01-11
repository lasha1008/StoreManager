using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StoreManager.API.JwtToken;
using StoreManager.DTO;
using StoreManager.Facade.Interfaces.Services;
using StoreManager.Models;

namespace StoreManager.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[JwtTokenAuthorization]
public class CategoryController : ControllerBase
{
    private readonly ICategoryQueryService _categoryQueryService;
    private readonly ICategoryCommandService _categoryCommandService;
    private readonly IMapper _mapper;

    public CategoryController(ICategoryQueryService categoryQueryService, ICategoryCommandService categoryCommandService, IMapper mapper)
    {
        _categoryQueryService = categoryQueryService;
        _categoryCommandService = categoryCommandService;
        _mapper = mapper;
    }

    [HttpGet]
    [Route("get/{id}")]
    public CategoryModel Get(int id) => _mapper.Map<CategoryModel>(_categoryQueryService.Get(id));

    //TODO: Add pager functionality.
    [HttpGet]
    [Route("search/{text}")]
    public IEnumerable<CategoryModel> Search(string text) => _mapper.Map<IEnumerable<CategoryModel>>(_categoryQueryService.Search(text));

    [HttpPost]
    public int Insert(CategoryModel model) => _categoryCommandService.Insert(_mapper.Map<Category>(model));

    [HttpPut]
    [Route("{id}")]
    public void Update(int id, CategoryModel model)
    {
        var category = _mapper.Map<Category>(model);
        category.Id = id;
        _categoryCommandService.Update(category);
    }

    [HttpDelete]
    [Route("{id}")]
    public void Delete(int id) => _categoryCommandService.Delete(id);
}