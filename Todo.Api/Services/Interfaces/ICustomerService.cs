using System.Collections.Generic;
using Todo.Api.Models;

namespace Todo.Api.Services.Interfaces
{
    public interface ICustomerService
    {
        IEnumerable<Customer> GetAll();
        Customer Get(int id);
        int Update(Customer customer);
        int Add(Customer customer);
        int Delete(int customerId);
        IEnumerable<Technology> GetTechnologyList();
        int AddTechnology(Technology technology);
        int DeleteTechnology(string name);
    }
}
