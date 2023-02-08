using Daniel.Common.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUF.Shared.Bodys.Boards;

public class BoardMeta
{
    public static readonly ApiMetaData CreateMeta = new ApiMetaData
    {
        Title = "게시판작성",
        httpmethod = Knus.Common.Services.HttpMethods.POST,
        UrlPath = "/api/Board/board"
    };
    public static readonly ApiMetaData UpdateMeta = new ApiMetaData
    {
        Title = "게시판수정",
        httpmethod = Knus.Common.Services.HttpMethods.PUT,
        UrlPath = "/api/Board/board"
    };

    public class Body : AudiTableEntity
    {
        [MaxLength(200, ErrorMessage = "200자까지")]
        [Required(ErrorMessage = "아이디 필수")]
        public string Subject { get; set; }

        public string Contents { get; set; }
        [Required(ErrorMessage = "내용 필수")]
        public string ContentsHtml { get; set; }

        public string Bkey { get; set; }
        public int? ReadCount { get; set; }
        [MaxLength(30)]
        public string UserIpAddr { get; set; }
        [MaxLength(100)]
        public string BoardPassword { get; set; }
        public bool UseYn { get; set; }
    }
}
