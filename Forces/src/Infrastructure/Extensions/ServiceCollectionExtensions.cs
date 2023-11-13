using Forces.Application.Interfaces.Repositories;
using Forces.Application.Interfaces.Services;
using Forces.Application.Interfaces.Serialization.Serializers;
using Forces.Application.Interfaces.Services.Storage;
using Forces.Infrastructure.Services;
using Forces.Application.Interfaces.Services.Storage.Provider;
using Forces.Application.Serialization.JsonConverters;
using Forces.Application.Serialization.Options;
using Forces.Application.Serialization.Serializers;
using Forces.Infrastructure.Repositories;
using Forces.Infrastructure.Services.Storage;
using Forces.Infrastructure.Services.Storage.Provider;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;
using Forces.Application.Models;
using Forces.Application.Helper.NotificationsHelper;
using Forces.Domain.Contracts;

namespace Forces.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddInfrastructureMappings(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services
                .AddTransient(typeof(IRepositoryAsync<,>), typeof(RepositoryAsync<,>))
                .AddTransient<IProductRepository, ProductRepository>()
                .AddTransient<IForceRepository, ForceRepository>()
                .AddTransient<IBrandRepository, BrandRepository>()
                .AddTransient<IBaseRepository, BaseRepository>()
                .AddTransient<IAirCraftRepository, AirCraftRepository>()
                .AddTransient<IAirKindRepository, AirKindRepository>()
                .AddTransient<IDepoRepository, DepoRepository>()
                .AddTransient<IHQRepository, HQRepository>()
                .AddTransient<IItemRepository, ItemRepository>()
                .AddTransient<IBaseSectionRepository, BaseSectionRepository>()
                .AddTransient<IMprRequestRepository, MprRequestRepository>()
                .AddTransient<IDocumentRepository, DocumentRepository>()
                .AddTransient<IVehicleRequestRepository, VehicleRequestRepository>()
                .AddTransient<INotifire<VehicleRequest>, VehicleRequestNotification>()
                .AddTransient(typeof(INotificationService <,>), typeof(NotificationService <,>))
                .AddTransient<IDocumentTypeRepository, DocumentTypeRepository>()
                .AddTransient<IVoteCodeRepository, VoteCodeRepository>()
                .AddTransient<IRoomRepository, RoomRepository>()
                .AddTransient<IOfficeRepository, OfficeRepository>()
                .AddTransient<IInventoryRepository, InventoryRepository>()
                .AddTransient<IInventoryItemRepository, InventoryItemRepository>()
                .AddTransient<IHouseRepository, HouseRepository>()
                .AddTransient<IBuildingRepository, BuildingRepository>()
                .AddTransient<IPersonRepository, PersonRepository>()

                .AddTransient(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
        }

        public static IServiceCollection AddExtendedAttributesUnitOfWork(this IServiceCollection services)
        {
            return services
                .AddTransient(typeof(IExtendedAttributeUnitOfWork<,,>), typeof(ExtendedAttributeUnitOfWork<,,>));
        }

        public static IServiceCollection AddServerStorage(this IServiceCollection services)
            => AddServerStorage(services, null);

        public static IServiceCollection AddServerStorage(this IServiceCollection services, Action<SystemTextJsonOptions> configure)
        {
            return services
                .AddScoped<IJsonSerializer, SystemTextJsonSerializer>()
                .AddScoped<IStorageProvider, ServerStorageProvider>()
                .AddScoped<IServerStorageService, ServerStorageService>()
                .AddScoped<ISyncServerStorageService, ServerStorageService>()
                .Configure<SystemTextJsonOptions>(configureOptions =>
                {
                    configure?.Invoke(configureOptions);
                    if (!configureOptions.JsonSerializerOptions.Converters.Any(c => c.GetType() == typeof(TimespanJsonConverter)))
                        configureOptions.JsonSerializerOptions.Converters.Add(new TimespanJsonConverter());
                });
        }
    }
}