using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Todo.Api.Models;
using Todo.Api.Services.Interfaces;

namespace Todo.UnitTest
{
    [TestFixture]
    public class CustomerServiceTest
    {
        private List<Customer> customers;
        private List<Technology> technologies;

        [OneTimeSetUp]
        public void RepositorySetUp()
        {
            customers = new List<Customer>() {
                new Customer(){ ID = 100, FirstName = "FirstName_1", LastName = "LastName_1"  },
                new Customer(){ ID = 101, FirstName = "FirstName_2", LastName = "LastName_2"  },
                new Customer(){ ID = 102, FirstName = "FirstName_3", LastName = "LastName_3"  }
            };

            technologies = new List<Technology>() {
                new Technology() { TechnologyName = "Azure" },
                new Technology() { TechnologyName = "SQL" },
                new Technology() { TechnologyName = "CSharp" },
                new Technology() { TechnologyName = "Angular" }
            };
        }

        [Test]
        public void GetCustomerAll_Success()
        {
            // Arrange
            var mockService = new Mock<ICustomerService>();
            mockService.Setup(x => x.GetAll()).Returns(() =>
            {
                return customers;
            });

            // Act
            var actual = mockService.Object.GetAll();

            // Assert
            Assert.AreEqual(customers.Count(), actual.Count());
        }

        [Test]
        public void GetCustomer_Success()
        {
            // Arrange
            var customer = new Customer
            {
                ID = 100,
                FirstName = "Solly",
                LastName = "Fathi",
                TechnologyList = technologies.Take(2)
            };

            var mockService = new Mock<ICustomerService>();
            mockService.Setup(x => x.Get(customer.ID)).Returns(() =>
            {
                return customer;
            });

            // Act
            mockService.Object.Add(customer);
            var actual = mockService.Object.Get(customer.ID);

            // Assert
            Assert.AreEqual(customer, actual);
        }

        [Test]
        public void GetCustomer_NotFound_Success()
        {
            // Arrange
            var customerId = 1000;
            var mockService = new Mock<ICustomerService>();

            mockService.Setup(x => x.Get(customerId)).Returns(() =>
            {
                return null;
            });

            // Act
            var actual = mockService.Object.Get(customerId);

            // Assert
            mockService.Verify(m => m.Get(customerId), Times.AtLeastOnce());
            Assert.AreEqual(null, actual);
        }

        [Test]
        public void AddCustomer_Success()
        {
            // Arrange
            var customer = new Customer
            {
                ID = 100,
                FirstName = "Solly",
                LastName = "Fathi",
                TechnologyList = technologies.Take(2).ToArray()
            };

            var mockService = new Mock<ICustomerService>();
            mockService.Setup(x => x.Add(It.IsAny<Customer>()));
            mockService.Setup(x => x.Get(customer.ID)).Returns(() =>
            {
                return customer;
            });

            // Act
            mockService.Object.Add(customer);
            var actual = mockService.Object.Get(customer.ID);

            // Assert
            mockService.Verify(m => m.Add(customer), Times.AtLeastOnce());
            Assert.AreEqual(customer, actual);
        }

        [Test]
        public void AddCustomer_CustomerIsNull_Failure_Throws()
        {
            string errorMessage = "Customer cannot be null";

            // Arrange
            var customer = It.IsAny<Customer>();

            // Act and Assert
            Assert.That(() =>
                AddCustomerThrowException(customer, errorMessage),
                Throws.Exception.TypeOf<Exception>().And.Message.EqualTo(errorMessage));
        }

        [Test]
        public void AddCustomer_CustomerFirstNameIsEmpty_Failure_Throws()
        {
            string errorMessage = "First name cannot be empty";

            // Arrange
            var customer = new Customer
            {
                ID = 100,
                LastName = "Fathi",
                TechnologyList = technologies.Take(2)
            };

            // Act and Assert
            Assert.That(() =>
                AddCustomerThrowException(customer, errorMessage),
                Throws.Exception.TypeOf<Exception>().And.Message.EqualTo(errorMessage));
        }

        [Test]
        public void UpdateCustomer_Success()
        {
            // Arrange
            var customer = new Customer
            {
                ID = 100,
                FirstName = "Solly",
                LastName = "Fathi",
                TechnologyList = technologies.Take(2)
            };

            var mockService = new Mock<ICustomerService>();
            mockService.Setup(x => x.Update(customer));

            // Act
            var actual = mockService.Object.Update(customer);

            // Assert
            mockService.Verify(m => m.Update(customer), Times.AtLeastOnce());
            Assert.That(customer.FirstName, Is.EqualTo("Solly"));
        }

        [Test]
        public void DeleteCustomer_Success()
        {
            // Arrange
            var customer = new Customer
            {
                ID = 100,
                FirstName = "Solly",
                LastName = "Fathi",
                TechnologyList = technologies.Take(2)
            };

            var mockService = new Mock<ICustomerService>();
            mockService.Setup(x => x.Delete(customer.ID)).Returns(() =>
            {
                return 0;
            });

            // Act
            mockService.Object.Delete(customer.ID);
            var actual = mockService.Object.Get(customer.ID);

            // Assert
            mockService.Verify(m => m.Delete(customer.ID));
            mockService.Verify(m => m.Get(customer.ID));
            Assert.AreEqual(null, actual);
        }

        private void AddCustomerThrowException(Customer customer, string errorMessage)
        {
            var mockService = new Mock<ICustomerService>();
            mockService.Object.Add(customer);
            throw new Exception(errorMessage);
        }
    }
}
