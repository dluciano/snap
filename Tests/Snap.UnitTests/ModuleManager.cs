using System;
using System.Reflection;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Snap.DataAccess;

namespace Snap.Tests
{
    public class ModuleManager :
        ServiceCollection,
        IServiceProvider,
        IDisposable
    {
        private ServiceProvider _provider;
        private readonly SqliteConnection _connection = new SqliteConnection("DataSource=:memory:");

        public ModuleManager Configure(Func<IServiceCollection, IServiceCollection> configure)
        {
            configure(this);
            return this;
        }

        public ModuleManager Configure(Action<IServiceCollection> config)
        {
            config?.Invoke(this);
            return this;
        }

        public ModuleManager Build()
        {
            _provider = this.BuildServiceProvider();
            return this;
        }

        public ModuleManager ConfigureInMemorySql()
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
    }
}
