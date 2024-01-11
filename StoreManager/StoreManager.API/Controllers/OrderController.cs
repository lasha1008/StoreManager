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
public class OrderController : ControllerBase
{
    private readonly IOrderQueryService _orderQueryService;
    private readonly IOrderCommandService _orderCommandService;
    private readonly IMapper _mapper;

    public OrderController(IOrderQueryService orderQueryService, IOrderCommandService orderCommandService, IMapper mapper)
    {
        _orderQueryService = orderQueryService;
        _orderCommandService = orderCommandService;
        _mapper = mapper;
    }

    [HttpGet]
    [Route("get/{id}")]
    public OrderModel Get(int id) => _mapper.Map<OrderModel>(_orderQueryService.Get(id));

    [HttpGet]
    [Route("search/{text}")]
    public IEnumerable<OrderModel> Search(string text) => _mapper.Map<IEnumerable<OrderModel>>(_orderQueryService.Search(text));

    [HttpPost]
    public int Insert(OrderModel model) => _orderCommandService.Insert(_mapper.Map<Order>(model));

    [HttpPut]
    [Route("{id}")]
    public void Update(int id, OrderModel model)
    {
        var order = _mapper.Map<Order>(model);
        order.Id = id;
        _orderCommandService.Update(order);
    }
}