using Daniel.Common.Interfaces;
using Knus.Common.Services;
using TUF.Shared.Dtos;

namespace TUF.Front.Client.Services
{
    public interface ITokenService:ITransient
    {
        Task<LoginDto> GetTokenAsync(LoginDto.Request request);
        Task<LoginDto> RefreshAsync(string  refreshtoken);
    }

    public class TokenService:ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<LoginDto> GetTokenAsync(LoginDto.Request request)
        {  
           ApiProvider<LoginDto> api = new ApiProvider<LoginDto>();
           api.BaseAddress = _configuration["ApiBaseUrl"];
           api.Apimeta = request.metadata;
           api.SendValue = Newtonsoft.Json.JsonConvert.SerializeObject(request);
           var rt = await api.AsyncCallData();
            if (!rt.Success)
            {
                rt.OutValue.Success = false;
                rt.OutValue.Message = rt.Message;
            }
           return rt.OutValue;
        }

        public async Task<LoginDto> RefreshAsync(string refreshtoken)
        {
            return new LoginDto();
        }

    }
}
