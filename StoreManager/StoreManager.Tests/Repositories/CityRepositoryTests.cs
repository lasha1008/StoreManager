using Microsoft.EntityFrameworkCore;
using StoreManager.DTO;
using StoreManager.Facade.Interfaces.Repositories;

namespace StoreManager.Tests.Repositories;

public class CityRepositoryTests : RepositoryUnitTestBase
{
    public CityRepositoryTests(IUnitOfWork unitOfWork) : base(unitOfWork) { }

    [Theory]
    [InlineData("City 1")]
    [InlineData("City 2")]
    [InlineData("City 3")]
    [InlineData("City 4")]
    public void Insert(string name)
    {
        City city = GetTestRecord(name);
        _unitOfWork.CityRepository.Insert(city);
        _unitOfWork.SaveChanges();

        Assert.True(city.Id > 0);
    }

    [Theory]
    [InlineData("City 5")]
    [InlineData("City 6")]
    [InlineData("City 7")]
    [InlineData("City 8")]
    public void NotInserted(string name)
    {
        City city = GetTestRecord(name);
        city.Id = 1;

        Assert.Throws<ArgumentException>(() =>
        {
            _unitOfWork.CityRepository.Insert(city);
            _unitOfWork.SaveChanges();
        });
    }

    [Theory]
    [InlineData("City 1")]
    [InlineData("City 2")]
    [InlineData("City 3")]
    [InlineData("City 4")]
    public void Update(string name)
    {
        City newCity = GetTestRecord(name);
        _unitOfWork.CityRepository.Insert(newCity);
        _unitOfWork.SaveChanges();

        newCity.Name = $"New {name}";
        _unitOfWork.CityRepository.Update(newCity);
        _unitOfWork.SaveChanges();

        City updatedCity = _unitOfWork.CityRepository.Set(x => x.Id == newCity.Id).Single();

        Assert.True(updatedCity.Name == newCity.Name);
    }

    [Theory]
    [InlineData("City 1")]
    [InlineData("City 2")]
    [InlineData("City 3")]
    [InlineData("City 4")]
    public void NotUpdated(string name)
    {
        City newCity = GetTestRecord(name);

        Assert.Throws<DbUpdateConcurrencyException>(() =>
        {
            _unitOfWork.CityRepository.Update(newCity);
            _unitOfWork.SaveChanges();
        });
    }

    [Theory]
    [InlineData("City 1")]
    [InlineData("City 2")]
    [InlineData("City 3")]
    [InlineData("City 4")]
    public void DeleteByObject(string name)
    {
        City city = GetTestRecord(name);
        _unitOfWork.CityRepository.Insert(city);
        _unitOfWork.SaveChanges();

        _unitOfWork.CityRepository.Delete(city);
        _unitOfWork.SaveChanges();

        Assert.Null(_unitOfWork.CityRepository.Set(x => x.Id == city.Id).SingleOrDefault());
    }

    [Theory]
    [InlineData("City 1")]
    [InlineData("City 2")]
    [InlineData("City 3")]
    [InlineData("City 4")]
    public void DeleteById(string name)
    {
        City city = GetTestRecord(name);
        _unitOfWork.CityRepository.Insert(city);
        _unitOfWork.SaveChanges();

        _unitOfWork.CityRepository.Delete(city.Id);
        _unitOfWork.SaveChanges();

        Assert.Null(_unitOfWork.CityRepository.Set(x => x.Id == city.Id).SingleOrDefault());
    }

    [Theory]
    [InlineData("City 1")]
    [InlineData("City 2")]
    [InlineData("City 3")]
    [InlineData("City 4")]
    public void GetById(string name)
    {
        City newCity = GetTestRecord(name);
        _unitOfWork.CityRepository.Insert(newCity);
        _unitOfWork.SaveChanges();

        City retrievedCity = _unitOfWork.CityRepository.Get(newCity.Id);

        Assert.True(retrievedCity.Id == newCity.Id);
    }

    [Theory]
    [InlineData("City 1")]
    [InlineData("City 2")]
    [InlineData("City 3")]
    [InlineData("City 4")]
    public void Set(string name)
    {
        City newCity = GetTestRecord(name);
        List<City> expectedSet = new();
        expectedSet.Add(newCity);

        _unitOfWork.CityRepository.Insert(newCity);
        _unitOfWork.SaveChanges();

        var retrievedCities = _unitOfWork.CityRepository.Set();

        Assert.True(expectedSet.Last().Name == retrievedCities.Last().Name);
    }

    private static City GetTestRecord(string name)
    {
        City city = new()
        {
            Name = name
        };
        return city;
    }
}
