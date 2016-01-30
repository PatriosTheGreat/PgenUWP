using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GenerationCore
{
    public interface IServicesManager
    {
        void AddServiceAsync(ServiceInformation service);

        void RemoveServiceAsync(string serviceToken);

        Task<IEnumerable<ServiceInformation>> LoadServicesAsync();

        event Action ServicesUpdated;
    }
}
