﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenerationCore
{
    public sealed class TestServiceManager : IServicesManager
    {
        public event Action ServicesUpdated;

        public TestServiceManager()
        {
            _services = new List<ServiceInformation>
            {
                new ServiceInformation(
                    "VK",
                    new PasswordRestriction(SymbolsType.Digital)),
                new ServiceInformation(
                    "Facebook",
                    new PasswordRestriction(SymbolsType.UpcaseLatin)),
                new ServiceInformation(
                    "Twitter",
                    new PasswordRestriction(SymbolsType.LowcaseLatin)),
                new ServiceInformation(
                    "Instagram",
                    new PasswordRestriction(SymbolsType.LowcaseLatin | SymbolsType.Digital)),
                new ServiceInformation(
                    "VK",
                    new PasswordRestriction(SymbolsType.Digital)),
                new ServiceInformation(
                    "Facebook",
                    new PasswordRestriction(SymbolsType.UpcaseLatin)),
                new ServiceInformation(
                    "Twitter",
                    new PasswordRestriction(SymbolsType.LowcaseLatin)),
                new ServiceInformation(
                    "Instagram",
                    new PasswordRestriction(SymbolsType.LowcaseLatin | SymbolsType.Digital))
            };
        }

        public void SaveServiceAsync(ServiceInformation service)
        {
            _services.Add(service);
        }

        public void DeleteServiceAsync(string serviceToken)
        {
            _services.Remove(_services.Single(x => x.UniqueToken == serviceToken));
        }

        public Task<IEnumerable<ServiceInformation>> LoadServicesAsync()
        {
            return Task.FromResult((IEnumerable<ServiceInformation>)_services);
        }

        private readonly IList<ServiceInformation> _services;
    }
}
