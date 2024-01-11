using StoreManager.DTO;
using StoreManager.Facade.Interfaces.Repositories;
using StoreManager.Facade.Interfaces.Services;

namespace StoreManager.Services;

public sealed class CategoryCommandService : CommandServiceBase<Category, ICategoryRepository>, ICategoryCommandService
{
    public CategoryCommandService(IUnitOfWork unitOfWork) : base(unitOfWork, unitOfWork.CategoryRepository)
    {
        
    }
}
