using StoreManager.DTO;
using StoreManager.Facade.Exceptions;
using StoreManager.Facade.HelpExtentions;
using StoreManager.Facade.Interfaces.Repositories;
using StoreManager.Facade.Interfaces.Services;

namespace StoreManager.Services;

public sealed class CustomerAccountService : ICustomerAccountService
{
    private readonly IUnitOfWork _unitOfWork;

    public CustomerAccountService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public void Login(string username, string password)
    {
        if (string.IsNullOrEmpty(username)) throw new ArgumentException($"{nameof(username)} cannot be null or empty.", nameof(username));
        if (string.IsNullOrEmpty(password)) throw new ArgumentException($"{nameof(password)} cannot be null or empty.", nameof(password));

        Customer? customer = _unitOfWork.CustomerRepository
            .Set(x => x.AccountDetails != null &&
                      x.AccountDetails.Username == username &&
                      x.AccountDetails.Password == password.GetHash() &&
                      !x.IsDeleted)
            .SingleOrDefault();

        if (customer == null)
            throw new LoginException(username);
    }

    public void Register(string username, string password, Customer customer)
    {
        if (username == null) throw new ArgumentNullException(nameof(username));
        if (password == null) throw new ArgumentNullException(nameof(password));
        if (customer == null) throw new ArgumentNullException(nameof(customer));

        customer.AccountDetails = new AccountDetails
        {
            Username = username,
            Password = password.GetHash()
        };
        _unitOfWork.CustomerRepository.Insert(customer);
        _unitOfWork.SaveChanges();
    }

    public void UpdatePassword(int id, string oldPassword, string newPassword)
    {
        if (oldPassword == null) throw new ArgumentNullException(nameof(oldPassword));
        if (newPassword == null) throw new ArgumentNullException(nameof(newPassword));

        Customer customer = _unitOfWork.CustomerRepository
            .Set(x => x.Id == id && x.AccountDetails!.Password == oldPassword.GetHash() && !x.IsDeleted)
            .Single();

        customer.AccountDetails!.Password = newPassword.GetHash();

        _unitOfWork.CustomerRepository.Update(customer);
        _unitOfWork.SaveChanges();
    }

    public void Unregister(int customerId)
    {
        Customer customer = _unitOfWork.CustomerRepository
            .Set(x => x.Id == customerId && !x.IsDeleted)
            .Single();

        _unitOfWork.CustomerRepository.DeleteAccount(customer);
        _unitOfWork.SaveChanges();
    }
}

