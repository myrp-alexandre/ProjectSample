﻿using ProjectSample.Application.Common.Services;
using ProjectSample.Areas.Account.Models.Register;
using ProjectSample.Areas.Account.Services.Models;
using ProjectSample.Core.Domain;
using ProjectSample.Infrastructure.DataAccess;
using ProjectSample.Infrastructure.NHibernate.Security.Queries;
using ProjectSample.Infrastructure.Security.Domain;
using ProjectSample.Infrastructure.Security.Services;

namespace ProjectSample.Areas.Account.Services.Impl
{
    public class RegistrationService : IRegistrationService
    {
        private readonly ICryptoService _cryptoService;
        private readonly ICurrentCustomerService _currentCustomerService;
        private readonly IRepository _repository;

        public RegistrationService(IRepository repository, ICurrentCustomerService currentCustomerService, ICryptoService cryptoService)
        {
            _repository = repository;
            _currentCustomerService = currentCustomerService;
            _cryptoService = cryptoService;
        }

        public RegistrationResult Register(RegisterFields fields)
        {
            if (_repository.Query(new FindUserByUsernameQuery(fields.Email)) != null)
            {
                return RegistrationResult.DuplicateUsername;
            }

            var user = new User
            {
                Role = Role.User,
                Customer = _currentCustomerService.CurrentCustomer(),
                PasswordHash = _cryptoService.HashPassword(fields.Password),
                UserName = fields.Email
            };

            _repository.Save(user);
            return RegistrationResult.Success(user);
        }
    }
}