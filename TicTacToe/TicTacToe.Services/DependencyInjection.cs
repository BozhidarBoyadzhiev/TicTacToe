using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TicTacToe.Services.Computer;
using TicTacToe.Services.Computer.Contracts;

namespace TicTacToe.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        services.AddScoped<IComputerService, ComputerService>();

        return services;
    }
}