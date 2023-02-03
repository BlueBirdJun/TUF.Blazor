using Daniel.Common.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUF.Shared.Dtos.Boards;

public class BoardDto : DtoBase<BoardDto.Response, BoardCommentDto.Request>
{
    public class Request: Body
    {
        public readonly ApiMetaData CreateMeta = new ApiMetaData
        {
            Title = "게시판작성",
            httpmethod = Knus.Common.Services.HttpMethods.POST,
            UrlPath = "/api/Board/board"
        };
        public readonly ApiMetaData UpdateMeta = new ApiMetaData
        {
            Title = "게시판작성",
            httpmethod = Knus.Common.Services.HttpMethods.PUT,
            UrlPath = "/api/Board/board"
        };
    }

    public class Response : Body { }

    public class Body : AudiTableEntity
    {
        [MaxLength(200,ErrorMessage ="200자까지")]
        [Required(ErrorMessage = "아이디 필수")]
        public string Subject { get; set; }

        public string Contents { get; set; }

        public string ContentsHtml { get; set; }
        [MaxLength(10)]
        [Required(ErrorMessage = "필수")]
        public string Bkey { get; set; }
        public int? ReadCount { get; set; }
        [MaxLength(30)]
        public string UserIpAddr { get; set; }
        [MaxLength(100)]
        public string BoardPassword { get; set; }
        public bool UseYn { get; set; }
    }
}
