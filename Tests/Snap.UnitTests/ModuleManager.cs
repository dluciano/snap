using System;
using System.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Snap.DataAccess;

namespace Snap.UnitTests
{
    public class ModuleManager :
        ServiceCollection,
        IServiceProvider,
        IDisposable
    {
        private ServiceProvider _provider = null;
        private readonly SqliteConnection _connection = new SqliteConnection("DataSource=:memory:");

        public IServiceProvider Configure(Func<IServiceCollection, IServiceCollection> configure) =>
            _provider = configure(this).BuildServiceProvider();

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
