﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using ProjectSample.Core.Domain;

namespace ProjectSample.UnitTests.Application.Common.Services
{
    public class WhenGettingCurrentCustomerAndThereIsACurrentUser : CurrentCustomerServiceContext
    {
        private Mock<Customer> _mockCustomer;
        
        protected override void Setup()
        {
            _mockCustomer = CreateDependency<Customer>();
            var mockUser = CreateDependency<User>();
            mockUser.SetupGet(x => x.Customer).Returns(_mockCustomer.Object);
            CurrentUserServiceMock.Setup(x => x.CurrentUser()).Returns(mockUser.Object);

            Result = Service.CurrentCustomer();
        }

        [Test]
        public void should_return_customer()
        {
            Assert.AreSame(Result, _mockCustomer.Object);
        }
    }
}



