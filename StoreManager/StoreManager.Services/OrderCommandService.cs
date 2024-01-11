using StoreManager.DTO;
using StoreManager.Facade.Interfaces.Repositories;
using StoreManager.Facade.Interfaces.Services;

namespace StoreManager.Services;

public class OrderCommandService : CommandServiceBase<Order, IOrderRepository>, IOrderCommandService
{
    private readonly IUnitOfWork _unitOfWork;

    public OrderCommandService(IUnitOfWork unitOfWork) : base(unitOfWork, unitOfWork.OrderRepository)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public override int Insert(Order entity)
    {
        if (entity.OrderDetails == null || !entity.OrderDetails.Any())
        {
            throw new ArgumentException("Order is empty.");
        }

        _unitOfWork.BeginTransaction();
        _unitOfWork.OrderRepository.Insert(entity);
        _unitOfWork.OrderDetailsRepository.InsertRange(entity.OrderDetails);
        _unitOfWork.CommitTransaction();
        _unitOfWork.SaveChanges();

        return entity.Id;
    }

    public override void Update(Order entity) => throw new NotSupportedException();

    public override void Delete(Order entity) => throw new NotSupportedException();
}
