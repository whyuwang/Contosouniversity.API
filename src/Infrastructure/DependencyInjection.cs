using Infrastructure.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, string? connectionString)
    {
        services.AddDbContext<ApplicationDbContext>(options
            => options.UseSqlServer(connectionString));
        return services;
    }
}
