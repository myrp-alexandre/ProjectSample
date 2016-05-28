﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProjectSample.Areas.Account.Models.Register;
using ProjectSample.Areas.Account.Services.Models;
using ProjectSample.Core.Application;
using ProjectSample.Core.Domain;
using ProjectSample.Core.Infrastructure.DataAccess;
using ProjectSample.Core.Infrastructure.NHibernate.Identity.Queries;
using ProjectSample.Core.Infrastructure.Security;

namespace ProjectSample.Areas.Account.Services.Impl
{
    public class RegistrationService : IRegistrationService
    {
        private readonly IRepository _repository;
        private readonly ICurrentUserService _currentUserService;
        private readonly ICryptoService _cryptoService;

        public RegistrationService(IRepository repository, ICurrentUserService currentUserService, ICryptoService cryptoService)
        {
            _repository = repository;
            _currentUserService = currentUserService;
            _cryptoService = cryptoService;
        }

        public RegistrationResult Register(RegisterFields fields)
        {
            if (_repository.Query(new FindUserByUsernameQuery(fields.Email)) != null)
            {
                return RegistrationResult.DuplicateUsername;
            }

            var user = new User()
            {
                Role = Role.User,
                Customer = _currentUserService.ActiveCustomer(),
                PasswordHash = _cryptoService.HashPassword(fields.Password),
                UserName = fields.Email
            };

            _repository.Save(user);
            return RegistrationResult.Success;
        }
    }
}