﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TicTacToe.Services.Computer;
using TicTacToe.Services.Computer.Contracts;
using TicTacToe.Services.Multiplayer;
using TicTacToe.Services.Multiplayer.Contracts;
using TicTacToe.Services.Multiplayer.Contracts.Models;

namespace TicTacToe.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        services.AddScoped<IComputerService, ComputerService>();
        services.AddScoped<IComputerModeDBService, ComputerModeDBService>();
        services.AddScoped<IMultiplayerService, MultiplayerService>();
        services.AddScoped<IMultiplayerModeDBService, MultiplayerModeDBService>();

        return services;
    }
}