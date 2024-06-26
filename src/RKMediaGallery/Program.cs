﻿using Avalonia;
using System;
using Microsoft.Extensions.DependencyInjection;
using Projektanker.Icons.Avalonia;
using Projektanker.Icons.Avalonia.FontAwesome;
using RKMediaGallery.Services;
using RKMediaGallery.Services.RecentlyOpened;
using RKMediaGallery.Views;
using RolandK.AvaloniaExtensions.DependencyInjection;
using RolandK.AvaloniaExtensions.ExceptionHandling;
using RolandK.InProcessMessaging;

namespace RKMediaGallery;

public static class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static int Main(string[] args)
    {
        try
        {
            BuildAvaloniaApp()
                .StartWithClassicDesktopLifetime(args);
            return 0;
        }
        catch (Exception ex)
        {
            GlobalErrorReporting.TryShowBlockingGlobalExceptionDialogInAnotherProcess(
                ex, 
                ".RKMediaGallery",
                "RKMediaGallery.ExceptionViewer");
            throw;
        }
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    // ReSharper disable once MemberCanBePrivate.Global
    public static AppBuilder BuildAvaloniaApp()
    {
        IconProvider.Current
            .Register<FontAwesomeIconProvider>();
        
        return AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace()
            .UseDependencyInjection(services =>
            {
                var inProcessMessenger = new InProcessMessenger();
                services.AddSingleton<IInProcessMessageSubscriber>(_ => inProcessMessenger);
                services.AddSingleton<IInProcessMessagePublisher>(_ => inProcessMessenger);

                services.AddSingleton<IRecentlyOpenedFilesService>(
                    _ => new RecentlyOpenedFilesService(".RKMediaGallery", 5));

                services.AddViewModels();
                services.AddTransient<MainWindowViewModel>();
            });
    }
}