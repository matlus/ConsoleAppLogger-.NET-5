﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;

namespace ConsoleAppLogger
{
    internal sealed class ConfigurationProvider
    {
        private readonly IConfigurationRoot _configurationRoot;
        public ConfigurationProvider()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("appsettings.json");
            LoadEnvironmentSpecificAppSettings(configurationBuilder);
            configurationBuilder.AddEnvironmentVariables("Alive_");

            _configurationRoot = configurationBuilder.Build();
        }

        [ExcludeFromCodeCoverage]
        internal ConfigurationProvider(IConfigurationRoot configurationRoot)
        {
            _configurationRoot = configurationRoot;
        }

        private static void LoadEnvironmentSpecificAppSettings(ConfigurationBuilder configurationBuilder)
        {
            var aspNetCoreEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (aspNetCoreEnvironment != null)
            {
                configurationBuilder.AddJsonFile($"appsettings.{aspNetCoreEnvironment}.json");
            }
        }

        private string RetrieveConfigurationSettingValue(string key)
        {            
            return _configurationRoot[key];
        }

        public IConfiguration GetLoggingSection()
        {
            return _configurationRoot;
            ////return _configurationRoot.GetSection("Logging");
        }
    }
}
