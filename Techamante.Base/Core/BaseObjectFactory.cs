using System;
using AutoMapper;
using SimpleInjector;
using SimpleInjector.Packaging;
using System.Collections.Generic;
using Techamante.Core.Interfaces;
using Techamante.Core.Extensions;

namespace Techamante.Core
{
    public class BaseObjectFactory : IObjectFactory
    {
        private readonly Container _container;

        public Container Container => _container;

        public BaseObjectFactory(Container container)
        {
            this._container = container;
        }

        public void Load(params IPackage[] packages) => packages.ForEach(package => package.RegisterServices(_container));

        public T Get<T>() where T : class => _container.GetInstance<T>();

        public IEnumerable<T> GetAll<T>() where T : class => _container.GetAllInstances<T>();

        public T1 Map<T1, T2>(T2 t2) => Mapper.Map<T1>(t2);

        public void Get<T>(Action<T> action) where T : class => action(Get<T>());

        public static BaseObjectFactory Instance
        {
            get
            {
                if (_instanceBaseObjectFactory == null) throw new NullReferenceException("Baseobjectfactory is not initialized");
                return _instanceBaseObjectFactory;
            }
        }

        private static BaseObjectFactory _instanceBaseObjectFactory;

        public static void Initialize(Container container)
        {
            if (_instanceBaseObjectFactory == null)
            {
                _instanceBaseObjectFactory = new BaseObjectFactory(container);
            }
        }

        public object Get(Type type)
        {
            try
            {
                return _container.GetInstance(type);
            }
            catch (ActivationException e)
            {
                throw new AppException("Type not found");
            }
        }

        public IEnumerable<object> GetAll(Type type) => _container.GetAllInstances(type);

    }
}
