﻿// Copyright (c) Wiesław Šoltés. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using Autofac;
using Avalonia;
using Avalonia.Logging.Serilog;
using Core2D.Avalonia.Direct2D.Modules;
using Core2D.Avalonia.Modules;
using Core2D.Interfaces;
using Serilog;

namespace Core2D.Avalonia.Direct2D
{
    /// <summary>
    /// Encapsulates a Avalonia program.
    /// </summary>
    class Program
    {
        /// <summary>
        /// Program entry point.
        /// </summary>
        /// <param name="args">The program arguments.</param>
        static void Main(string[] args)
        {
            InitializeLogging();

            var builder = new ContainerBuilder();

            builder.RegisterModule<LocatorModule>();
            builder.RegisterModule<CoreModule>();
            builder.RegisterModule<DependenciesModule>();
            builder.RegisterModule<AppModule>();
            builder.RegisterModule<ViewModule>();
            builder.RegisterModule<Direct2DModule>();

            using (IContainer container = builder.Build())
            {
                using (var log = container.Resolve<ILog>())
                {
                    var app = new App();
                    AppBuilder.Configure(app)
                        .UseWin32()
                        .UseDirect2D1()
                        .SetupWithoutStarting();
                    app.Start(container.Resolve<IServiceProvider>());
                }
            }
        }

        /// <summary>
        /// Initialize the Serilog logger.
        /// </summary>
        static void InitializeLogging()
        {
#if DEBUG
            SerilogLogger.Initialize(new LoggerConfiguration()
                .MinimumLevel.Warning()
                .WriteTo.Trace(outputTemplate: "{Area}: {Message}")
                .CreateLogger());
#endif
        }
    }
}
