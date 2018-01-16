using AutoMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using Todo.Api.Models;
using Todo.Api.Services.Interfaces;
using Todo.Model;
using Todo.Repository;

namespace Todo.Api.Services
{
    public class CustomerService : ICustomerService
    {
        public IEnumerable<Customer> GetAll()
        {
            var dtCustomers = CustomerRepository.GetAll();
            var customers = GetAll(dtCustomers);
            return customers;
        }

        public Customer Get(int customerId)
        {
            var dtCustomer = CustomerRepository.Get(customerId);
            var customer = Get(dtCustomer);
            return customer;
        }

        public int Add(Customer customer)
        {
            var customerDT = Mapper.Map<Customer, CustomerDT>(customer);
            return CustomerRepository.Add(customerDT);
        }

        public int Update(Customer customer)
        {
            var customerDT = Mapper.Map<Customer, CustomerDT>(customer);
            return CustomerRepository.Update(customerDT);
        }

        public int Delete(int customerId)
        {
            return CustomerRepository.Delete(customerId);
        }

        public IEnumerable<Technology> GetTechnologyList()
        {
            var dtTechnologyList = CustomerRepository.GetTechnologyList();
            var technologyList = GetTechnologyList(dtTechnologyList);
            return technologyList;
        }

        private List<Customer> GetAll(DataTable dtCustomers)
        {
            var customers = new List<Customer>();

            if (dtCustomers != null && dtCustomers.Rows.Count > 0)
            {
                for (int i = 0; i < dtCustomers.Rows.Count; i++)
                {
                    var dr = dtCustomers.Rows[i];

                    var customer = new Customer {
                        ID = Convert.ToInt32(dr["ID"]),
                        OwnerId = dr["OwnerId"].ToString(),
                        FirstName = dr["FirstName"].ToString(),
                        LastName = dr["LastName"].ToString(),
                        Email = dr["Email"].ToString(),
                        RegistrationDate = Convert.ToDateTime(dr["RegistrationDate"]),
                        TechnologyList = JsonConvert.DeserializeObject<IEnumerable<Technology>>(dr["TechnologyList"].ToString())
                    };

                    customers.Add(customer);
                }
            }

            return customers;
        }

        private Customer Get(DataTable dtCustomer)
        {
            Customer customer = null;

            if (dtCustomer != null && dtCustomer.Rows.Count > 0)
            {
                var dr = dtCustomer.Rows[0];

                customer = new Customer {
                    ID = Convert.ToInt32(dr["ID"]),
                    OwnerId = dr["OwnerId"].ToString(),
                    FirstName = dr["FirstName"].ToString(),
                    LastName = dr["LastName"].ToString(),
                    Email = dr["Email"].ToString(),
                    Gender = dr["Gender"].ToString(),
                    RegistrationDate = Convert.ToDateTime(dr["RegistrationDate"]),
                    TechnologyList = JsonConvert.DeserializeObject<IEnumerable<Technology>>(dr["TechnologyList"].ToString())
                };
            }

            return customer;
        }

        private IEnumerable<Technology> GetTechnologyList(DataTable dtTechnologyList)
        {
            var technologyList = new List<Technology>() { };

            if (dtTechnologyList != null && dtTechnologyList.Rows.Count > 0)
            {
                DataRow dr;
                Technology technology = null;

                for (int i = 0; i < dtTechnologyList.Rows.Count; i++)
                {
                    dr = dtTechnologyList.Rows[i];
                    technology = new Technology
                    {
                        TechnologyId = Convert.ToInt32(dr["TechnologyId"]),
                        TechnologyName = dr["TechnologyName"].ToString()
                    };

                    technologyList.Add(technology);
                }
            }

            return technologyList;
        }

        public int AddTechnology(Technology technology)
        {
            var technologyDT = Mapper.Map<Technology, TechnologyDT>(technology);
            return CustomerRepository.AddTechnology(technologyDT);

        }

        public int DeleteTechnology(string name)
        {
            return CustomerRepository.DeleteTechnology(name);
        }
    }
}
