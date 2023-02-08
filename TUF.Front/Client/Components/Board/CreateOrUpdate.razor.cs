using Knus.Common.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using Newtonsoft.Json;
using TUF.Front.Client.Components.Common;
using TUF.Front.Client.Shared;
using TUF.Shared.Authorization;
using TUF.Shared.Bodys.Boards;
using TUF.Shared.Dtos.Boards;
using TUF.Shared.Dtos.Member;

namespace TUF.Front.Client.Components.Board;

public partial class CreateOrUpdate
{
    #region inject
    protected BoardDto.Request boarddata = new();

    #endregion

    #region parameter
    private CustomValidation? _customValidation;
    [CascadingParameter]
    protected Task<AuthenticationState> AuthState { get; set; } = default!;

    #endregion

    #region property

    #endregion
    #region method 
    private async Task Submitaction()
    {
        var user = (await AuthState).User;
        boarddata.LastModifiedBy = user.GetUserId();
        boarddata.LastModifiedOn = DateTime.Now;
        boarddata.CreatedOn = DateTime.Now;
        boarddata.CreatedBy = user.GetUserId();
        boarddata.Bkey = "B001";
        boarddata.UseYn= true;
        ApiProvider<BoardDto> api = new ApiProvider<BoardDto>();
        api.SendValue = JsonConvert.SerializeObject(boarddata);

        var rt = await apihelper.ExecuteCall(api, BoardMeta.CreateMeta);
        if (rt.Success)
        {
            Snackbar.Add("작성완료", Severity.Success);
            MudDialog.Close(DialogResult.Ok(true));
        }
         
        //DialogOptions disableBackdropClick = new DialogOptions() { DisableBackdropClick = true };
        //var parameters = new DialogParameters();
        //ApiProvider<BoardDto> api = new ApiProvider<BoardDto>();
        //api.BaseAddress = Config["ApiBaseUrl"];
        //api.Apimeta = BoardMeta.CreateMeta;        
        //api.JwtKey =await tokenservice.GetLocalToken();
        //api.SendValue = JsonConvert.SerializeObject(boarddata);
        //var rt = await api.AsyncCallData();
    }
    #endregion
}
