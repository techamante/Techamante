using System;
using SimpleInjector.Packaging;
using System.Collections.Generic;

namespace Techamante.Core.Interfaces
{
    public interface IObjectFactory
    {
        T Get<T>() where T : class;
        object Get(Type type);
        IEnumerable<T> GetAll<T>() where T : class;
        IEnumerable<object> GetAll(Type type);
        void Get<T>(Action<T> action) where T : class;
        void Load(params IPackage[] packages);
        T1 Map<T1, T2>(T2 t2);
    }
}