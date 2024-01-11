using AutoMapper;
using StoreManager.DTO;
using StoreManager.Models;

namespace StoreManager.API.MapperProfiles;

public class MapperProfileConfiguration : Profile
{
    public MapperProfileConfiguration()
    {
        CreateMap<Category, CategoryModel>().ReverseMap();
        CreateMap<Product, ProductModel>().ReverseMap();
        CreateMap<Country, CountryModel>().ReverseMap();
        CreateMap<City, CityModel>().ReverseMap();
        CreateMap<Employee, EmployeeModel>().ReverseMap();
        CreateMap<Customer, CustomerModel>().ReverseMap();
        CreateMap<Order, OrderModel>().ReverseMap();
    }
}
