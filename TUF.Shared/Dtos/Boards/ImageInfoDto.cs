using Daniel.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUF.Shared.Dtos.Boards;

public class ImageInfoDto : DtoBase<BoardCommentDto.Response, BoardCommentDto.Request>
{
    public class Request
    {
        public readonly ApiMetaData CreateMeta = new ApiMetaData
        {
            Title = "로그인",
            httpmethod = Knus.Common.Services.HttpMethods.POST,
            UrlPath = "/api/tokens/Login"
        };
    }

    public record Response();
}
