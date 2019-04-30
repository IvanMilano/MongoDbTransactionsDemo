using CQRSlite.Caching;
using CQRSlite.Commands;
using CQRSlite.Domain;
using CQRSlite.Events;
using CQRSlite.Messages;
using CQRSlite.Queries;
using CQRSlite.Routing;

using Microsoft.Extensions.DependencyInjection;

using MongoDbTransactionsDemo.Domain.UseCases.RegisterUser.CommandHandlers;
using MongoDbTransactionsDemo.Infrastructure.Contracts;
using MongoDbTransactionsDemo.Infrastructure.DataAccess;
using MongoDbTransactionsDemo.Infrastructure.EventStore;

using System.Linq;
using System.Reflection;

namespace MongoDbTransactionsDemo.Api
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddSingleton<IMongoDbContext>(x => new MongoDbContext());
            services.AddSingleton<Router>(new Router());
            services.AddSingleton<ICommandSender>(y => y.GetService<Router>());
            services.AddSingleton<IEventPublisher>(y => y.GetService<Router>());
            services.AddSingleton<IHandlerRegistrar>(y => y.GetService<Router>());
            services.AddSingleton<IQueryProcessor>(y => y.GetService<Router>());
            services.AddSingleton<IEventStore, MongoDbEventStore>();
            services.AddSingleton<ICache, MemoryCache>();
            services.AddScoped<IRepository>(y => new CacheRepository(new Repository(y.GetService<IEventStore>()), y.GetService<IEventStore>(), y.GetService<ICache>()));
            services.AddScoped<ISession, Session>();

            services.Scan(scan => scan
                .FromAssemblies(typeof(UserCommandHandler).GetTypeInfo().Assembly)
                .AddClasses(classes => classes.Where(x => {
                    var allInterfaces = x.GetInterfaces();
                    return
                        allInterfaces.Any(y => y.GetTypeInfo().IsGenericType && y.GetTypeInfo().GetGenericTypeDefinition() == typeof(IHandler<>)) ||
                        allInterfaces.Any(y => y.GetTypeInfo().IsGenericType && y.GetTypeInfo().GetGenericTypeDefinition() == typeof(ICancellableHandler<>)) ||
                        allInterfaces.Any(y => y.GetTypeInfo().IsGenericType && y.GetTypeInfo().GetGenericTypeDefinition() == typeof(IQueryHandler<,>)) ||
                        allInterfaces.Any(y => y.GetTypeInfo().IsGenericType && y.GetTypeInfo().GetGenericTypeDefinition() == typeof(ICancellableQueryHandler<,>));
                }))
                .AsSelf()
                .WithTransientLifetime()
            );
            return services;
        }
    }
}
