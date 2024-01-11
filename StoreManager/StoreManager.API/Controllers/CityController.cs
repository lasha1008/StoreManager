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
public class CityController : ControllerBase
{
	private readonly ICityQueryService _cityQueryService;
	private readonly ICityCommandService _cityCommandService;
	private readonly IMapper _mapper;

	public CityController(ICityQueryService cityQueryService, ICityCommandService cityCommandService, IMapper mapper)
    {
        _cityQueryService = cityQueryService;
        _cityCommandService = cityCommandService;
        _mapper = mapper;
    }

    [HttpGet]
    [Route("{id}")]
    public CityModel Get(int id) => _mapper.Map<CityModel>(_cityQueryService.Get(id));

    [HttpGet]
    [Route("search/{text}")]
    public IEnumerable<CityModel> Search(string text) => _cityQueryService
        .Search(text)
        .Select(x => _mapper.Map<CityModel>(x));

    [HttpPost]
    public int Insert(CityModel model) => _cityCommandService.Insert(_mapper.Map<City>(model));

    [HttpPut]
    [Route("{id}")]
    public void Update(int id, CityModel model)
    {
        var city = _mapper.Map<City>(model);
        city.Id = id;
        _cityCommandService.Update(city);
    }

    [HttpDelete]
    [Route("{id}")]
    public void Delete(int id) => _cityCommandService.Delete(id);
}
