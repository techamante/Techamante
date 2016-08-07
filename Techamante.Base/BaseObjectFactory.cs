using System;
using AutoMapper;
using SimpleInjector;
using SimpleInjector.Packaging;

namespace Techamante.Base
{
    public  class BaseObjectFactory
    {
        private  readonly Container _container;


        private BaseObjectFactory(Container container)
        {
            this._container = container;
        }


        public  void Load(params IPackage[] packages)
        {
            packages.ForEach(package => package.RegisterServices(_container));
        }

        public  T Get<T>() where T : class => _container.GetInstance<T>();


        public  void Get<T>(Action<T> action) where T : class
        {
            var obj = Get<T>();
            action(obj);
        }


        public  T1 Map<T1, T2>(T2 t2) => Mapper.Map<T1>(t2);

        private static BaseObjectFactory _instanceBaseObjectFactory;

        public static void Initialize(Container container)
        {
            if (_instanceBaseObjectFactory == null)
            {
                _instanceBaseObjectFactory = new BaseObjectFactory(container);
            }
        }

        public static BaseObjectFactory Instance
        {
            get
            {
                if (_instanceBaseObjectFactory == null) throw new NullReferenceException("Baseobjectfactory is not initialized");
                return _instanceBaseObjectFactory;
            }
        } 

    }
}
