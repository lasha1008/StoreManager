using StoreManager.DTO;
using StoreManager.Facade.Interfaces.Repositories;
using StoreManager.Facade.Interfaces.Services;

namespace StoreManager.Services;

public sealed class CategoryQueryService : QueryServiceBase<Category, ICategoryRepository>, ICategoryQueryService
{
    private readonly IUnitOfWork _unitOfWork;

    public CategoryQueryService(IUnitOfWork unitOfWork) : base(unitOfWork.CategoryRepository)
    {
        _unitOfWork = unitOfWork;
    }

    public IEnumerable<Category> Search(string text)
    {
        if (text == null) throw new ArgumentNullException(nameof(text));

        return _unitOfWork
            .CategoryRepository
            .Set()
            .Where(x => x.Name.Contains(text) || 
                        (x.Description != null && x.Description.Contains(text))
            );
    }
}
