using Daniel.Common.Interfaces;
using Daniel.Common.Models;
using Knus.Common.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Newtonsoft.Json;
using TUF.Front.Client.Components.Common;
using TUF.Front.Client.Services;
using TUF.Shared.Dtos;
using static TUF.Front.Client.Common.TufApi;

namespace TUF.Front.Client.Shared;
public interface IApiHelper
{
    Task<ApiBaseEntityModel<T>> ExecuteCall<T>(ApiProvider<T> apiProvider, ApiMetaData meta, CustomValidation? customValidation = null);
}
public class ApiHelper : IApiHelper
{
    private readonly IConfiguration Config;
    private readonly ISnackbar snackbar;
    private readonly NavigationManager navigationManager;
    private readonly JwtAuthenticationService jwtservice;
    private readonly ITokenService tokenservice;

    public ApiHelper(IConfiguration configuration, ISnackbar isnackbar, ITokenService tokenService,
        NavigationManager _navigationManager, JwtAuthenticationService _jwtservice)
    {
        Config = configuration;
        snackbar = isnackbar;
        tokenservice = tokenService;
        navigationManager= _navigationManager;
        jwtservice= _jwtservice;
    }
    public async Task<ApiBaseEntityModel<T>> ExecuteCall<T>
        (ApiProvider<T> apiProvider, ApiMetaData meta, CustomValidation? customValidation = null)
    {
        ApiBaseEntityModel<T> rt = new ApiBaseEntityModel<T>();
        customValidation?.ClearErrors();
        try
        {
            apiProvider.Apimeta = meta;
            apiProvider.BaseAddress = Config["ApiBaseUrl"];
            apiProvider.JwtKey = await tokenservice.GetLocalToken();
            rt = await apiProvider.AsyncCallData();
            if(!rt.Success)
            {
                var r= JsonConvert.DeserializeObject<ApiError>(rt.ErrorMessage);
                if(r.ApiEnum == ApiEnum.UnAuthorized)
                {
                    snackbar.Add("로그인세션 만료", Severity.Warning);
                    await jwtservice.Logout();
                }

            }
            
        }
        catch (ApiException<HttpValidationProblemDetails> ex)
        {
            if (ex.Result.Errors is not null)
            {
                customValidation?.DisplayErrors(ex.Result.Errors);
            }
            else
            {
                snackbar.Add("Something went wrong!", Severity.Error);
            }
        }
        catch (ApiException<ErrorResult> ex)
        {
            snackbar.Add(ex.Result.Exception, Severity.Error);
        }
        catch (Exception ex)
        {

        }
        return rt;
    }
}

 
