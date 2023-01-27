using System.Threading.Tasks;

namespace Knus.Common.Services
{
    public interface IApiProvider<T>
    {
        bool Debug { get; set; }
        //HttpMethods HttpMethodValue { get; set; }
        string ParameterValue { get; set; }
        //string QueryPath { get; set; }
        string SendValue { get; set; }

        Task<ApiBaseEntityModel<T>> AsyncCallData();
        ///void Dispose();
    }
}