﻿using ProjectSample.Core.Domain;

namespace ProjectSample.Core.Application
{
    public interface ICurrentUserService
    {
        Customer ActiveCustomer();
    }
}