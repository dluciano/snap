using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Snap.DataAccess;

namespace Snap.DI
{
    public class SnapModuleManager :
        IServiceCollection,
        IServiceProvider,
        IDisposable
    {
        private ServiceProvider _provider;
        private readonly SqliteConnection _connection = new SqliteConnection("DataSource=:memory:");
        private readonly IServiceCollection _wrapper = new ServiceCollection();

        public int Count => _wrapper.Count;

        public bool IsReadOnly => _wrapper.IsReadOnly;

        public ServiceDescriptor this[int index] { get => _wrapper[index]; set => _wrapper[index] = value; }

        public SnapModuleManager()
        {

        }
        public SnapModuleManager(IServiceCollection wrapper)
        {
            _wrapper = wrapper;
        }

        public SnapModuleManager Configure(Action<IServiceCollection> config)
        {
            config?.Invoke(_wrapper);
            return this;
        }

        public SnapModuleManager Build()
        {
            _provider = this.BuildServiceProvider();
            return this;
        }

        public SnapModuleManager ConfigureInMemorySql()
        {
            this.AddDbContext<SnapDbContext>(options =>
            {
                _connection.Open();
                options.UseSqlite(_connection);
            });
            return this;
        }
        public void Dispose()
        {
            _connection.Close();
            _provider.Dispose();
        }

        public object GetService(Type serviceType) =>
            _provider.GetService(serviceType);

        public int IndexOf(ServiceDescriptor item) => _wrapper.IndexOf(item);

        public void Insert(int index, ServiceDescriptor item) => _wrapper.Insert(index, item);

        public void RemoveAt(int index) => _wrapper.RemoveAt(index);

        public void Add(ServiceDescriptor item) => _wrapper.Add(item);

        public void Clear() => _wrapper.Clear();

        public bool Contains(ServiceDescriptor item) => _wrapper.Contains(item);

        public void CopyTo(ServiceDescriptor[] array, int arrayIndex) => _wrapper.CopyTo(array, arrayIndex);

        public bool Remove(ServiceDescriptor item) => _wrapper.Remove(item);

        public IEnumerator<ServiceDescriptor> GetEnumerator() => _wrapper.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}
