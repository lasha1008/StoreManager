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
public class CustomerController : ControllerBase
{
    private readonly ICustomerQueryService _customerQueryService;
    private readonly ICustomerCommandService _customerCommandService;
    private readonly IMapper _mapper;

    public CustomerController(ICustomerQueryService customerQueryService, ICustomerCommandService customerCommandService, IMapper mapper)
    {
        _customerQueryService = customerQueryService;
        _customerCommandService = customerCommandService;
        _mapper = mapper;
    }

    [HttpGet]
    [Route("{id}")]
    public CustomerModel Get(int id) => _mapper.Map<CustomerModel>(_customerQueryService.Get(id));

    [HttpGet]
    [Route("search/{text}")]
    public IEnumerable<CustomerModel> Search(string text) => _customerQueryService
        .Search(text)
        .Select(x => _mapper.Map<CustomerModel>(x));

    [HttpPost]
    public int Insert(CustomerModel model) => _customerCommandService.Insert(_mapper.Map<Customer>(model));

    [HttpPut]
    [Route("{id}")]
    public void Update(int id, CustomerModel model)
    {
        var customer = _mapper.Map<Customer>(model);
        customer.Id = id;
        _customerCommandService.Update(customer);
    }

    [HttpDelete]
    [Route("{id}")]
    public void Delete(int id) => _customerCommandService.Delete(id);
}