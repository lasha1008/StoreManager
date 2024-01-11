using StoreManager.Facade.Interfaces.Repositories;

namespace StoreManager.Tests.Services.Command;

public class CommandUnitTestsBase : ServiceUnitTestsBase
{
    public CommandUnitTestsBase(IUnitOfWork unitOfWork) : base(unitOfWork) { }
}
