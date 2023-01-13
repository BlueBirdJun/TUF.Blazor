using System.Threading.Tasks;

using Knus.Common.Models;

namespace Knus.Common.Services
{
    public interface IWebProxyService<T>
    {
        string CallPath { get; set; }
        HttpMethods HttpMethodValue { get; set; }
        string SendValue { get; set; }
        bool XmlYN { get; set; }

        Task<ApiEntityModel<T>> AsyncCallData();
        void Dispose();
    }
}