﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate;
using ProjectSample.Core.Infrastructure.DataAccess;
using ProjectSample.Core.Infrastructure.Queries;

namespace ProjectSample.Core.Infrastructure.NHibernate.DataAccess.Impl
{
    public class NhRepository : Repository
    {
        public ISession Session { get; }

        public NhRepository(IUnitOfWork unitOfWork, ISession session) : base(unitOfWork)
        {
            Session = session;
        }

        public override T Find<T>(object id)
        {
            return Session.Get<T>(id);
        }

        public override IEnumerable<T> FindAll<T>()
        {
            return Session.CreateCriteria(typeof(T)).Future<T>();
        }

        public override TResult Query<TResult>(QueryObject<TResult> query)
        {
            return query.Execute(this);
        }

        protected override void SaveCore<T>(T entity)
        {
            Session.SaveOrUpdate(entity);
        }

        protected override void DeleteCore<T>(T entity)
        {
            Session.Delete(entity);
        }
    }
}