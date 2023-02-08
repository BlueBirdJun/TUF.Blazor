using Knus.Common.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Newtonsoft.Json;
using TUF.Front.Client.Components.Board;
using TUF.Front.Client.Components.Dialogs;
using TUF.Shared.Dtos.Boards;

namespace TUF.Front.Client.Areas.Boards
{
    public partial class Main
    {
        #region inject
        #endregion

        #region parameter
        [Parameter]
        public string Kind { get; set; }
        #endregion

        #region property

        protected async Task WiteButton()
        {

            //ApiProvider<string> api = new ApiProvider<string>();
            //api.BaseAddress = Config["ApiBaseUrl"];  + "/api/v1/test";
            //api.Apimeta = new Daniel.Common.Models.ApiMetaData()
            //{
            //    httpmethod = HttpMethods.GET,
            //    Title = "",
            //    UrlPath = "/api/v1/test"
            //};
            //api.JwtKey = await tokenservice.GetLocalToken();
            ////api.SendValue = JsonConvert.SerializeObject(boarddata);
            //var rt = await api.AsyncCallData();
            //return;
            DialogOptions disableBackdropClick = new DialogOptions() { DisableBackdropClick = true };
            var parameters = new DialogParameters();
            BoardInfoDto.Request r= new BoardInfoDto.Request();
            parameters.Add("ContentText","fff");
            DialogService.Show<CreateOrUpdate>("게시판", parameters, disableBackdropClick);
        }
        
        #endregion

    }
}
