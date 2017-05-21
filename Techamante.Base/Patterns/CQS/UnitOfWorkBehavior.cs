using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Techamante.Core.Interfaces;
using Techamante.Data.Interfaces;
using Techamante.Domain;

namespace Techamante.Patterns.CQS
{
    public class UnitOfWorkBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {

        private readonly IObjectFactory _objectFactory;

        private readonly IUnitOfWorkAsync _uow;

        public UnitOfWorkBehavior(IObjectFactory objectFactory, IUnitOfWorkAsync uow)
        {
            _objectFactory = objectFactory;
            _uow = uow;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next)
        {
            using (_objectFactory.BeginScope())
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    try
                    {

                        _uow.BeginTransaction();
                        var result = await next();
                        await DomainEvents.PublishAsync();
                        _uow.Commit();
                        scope.Complete();
                        return result;
                    }
                    catch (Exception ex)
                    {
                        _uow.Rollback();
                        throw ex;
                    }
                }

            }
        }
    }
}
