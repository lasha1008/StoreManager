using StoreManager.DTO;
using StoreManager.Facade.Interfaces.Repositories;
using StoreManager.Facade.Interfaces.Services;

namespace StoreManager.Services;

public sealed class CustomerCommandService : CommandServiceBase<Customer, ICustomerRepository>, ICustomerCommandService
{
    private readonly IUnitOfWork _unitOfWork;

    public CustomerCommandService(IUnitOfWork unitOfWork) : base(unitOfWork, unitOfWork.CustomerRepository)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    override public void Update(Customer entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));

        Customer? customer = _unitOfWork.CustomerRepository
            .Set()
            .SingleOrDefault(x => x.Id == entity.Id && !x.IsDeleted);

        entity.AccountDetails = customer?.AccountDetails;

        _unitOfWork.CustomerRepository.Update(entity);
        _unitOfWork.SaveChanges();
    }
}

