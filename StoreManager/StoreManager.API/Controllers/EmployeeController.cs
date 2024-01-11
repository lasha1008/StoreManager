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
public class EmployeeController : ControllerBase
{
	private readonly IEmployeeQueryService _employeeQueryService;
	private readonly IEmployeeCommandService _employeeCommandService;
	private readonly IMapper _mapper;

	public EmployeeController(IEmployeeQueryService employeeQueryService, IEmployeeCommandService employeeCommandService, IMapper mapper)
	{
		_employeeQueryService = employeeQueryService;
		_employeeCommandService = employeeCommandService;
		_mapper = mapper;
	}

	[HttpGet]
	[Route("{id}")]
	public EmployeeModel Get(int id) => _mapper.Map<EmployeeModel>(_employeeQueryService.Get(id));

	[HttpGet]
	[Route("search/{text}")]
	public IEnumerable<EmployeeModel> Search(string text) => _employeeQueryService
		.Search(text)
		.Select(x => _mapper.Map<EmployeeModel>(x));

	[HttpPost]
	public int Insert(EmployeeModel model) => _employeeCommandService.Insert(_mapper.Map<Employee>(model));

	[HttpPut]
	[Route("{id}")]
	public void Update(int id, EmployeeModel model)
	{
		var employee = _mapper.Map<Employee>(model);
		employee.Id = id;
		_employeeCommandService.Update(employee);
	}

	[HttpDelete]
	[Route("{id}")]
	public void Delete(int id) => _employeeCommandService.Delete(id);
}
