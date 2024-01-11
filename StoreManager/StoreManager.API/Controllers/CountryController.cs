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
public class CountryController : ControllerBase 
{
    private readonly ICountryQueryService _countryQueryService;
    private readonly ICountryCommandService _countryCommandService;
    private readonly IMapper _mapper;

    public CountryController(ICountryQueryService countryQueryService, ICountryCommandService countryCommandService, IMapper mapper)
    {
        _countryQueryService = countryQueryService;
        _countryCommandService = countryCommandService;
        _mapper = mapper;
    }

    [HttpGet]
    [Route("{id}")]
    public CountryModel Get(int id) => _mapper.Map<CountryModel>(_countryQueryService.Get(id));

    [HttpGet]
    [Route("search/{text}")]
    public IEnumerable<CountryModel> Search(string text) => _countryQueryService
        .Search(text)
        .Select(x => _mapper.Map<CountryModel>(x));

    [HttpPost]
    public int Insert(CountryModel model) => _countryCommandService.Insert(_mapper.Map<Country>(model));


    [HttpPut]
    [Route("{id}")]
    public void Update(int id, CountryModel model)
    {
        var country = _mapper.Map<Country>(model);
        country.Id = id;
        _countryCommandService.Update(country);
    }

    [HttpDelete]
    [Route("{id}")]
    public void Delete(int id) => _countryCommandService.Delete(id);
}