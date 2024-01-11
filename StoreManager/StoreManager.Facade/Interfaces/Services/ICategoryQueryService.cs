using StoreManager.DTO;

namespace StoreManager.Facade.Interfaces.Services;

public interface ICategoryQueryService : IQueryService<Category>
{
	IEnumerable<Category> Search(string text);
}
