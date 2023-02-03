using Daniel.Common.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUF.Shared.Dtos.Boards
{
    public class BoardCommentDto : DtoBase<BoardCommentDto.Response, BoardCommentDto.Request>
    {
        public class Request: Body
        {
            public readonly ApiMetaData CreateMeta = new ApiMetaData
            {
                Title = "코멘트",
                httpmethod = Knus.Common.Services.HttpMethods.POST,
                UrlPath = "/api/tokens/Login"
            };
            public readonly ApiMetaData UpdateMeta = new ApiMetaData
            {
                Title = "코멘트",
                httpmethod = Knus.Common.Services.HttpMethods.POST,
                UrlPath = "/api/tokens/Login"
            };

            
        }
        public record Response(Body body); 

        public class Body: AudiTableEntity
        {
            public string Bkey { get; set; }
            [Required(ErrorMessage = "필수")]
            public int BoardId { get; set; }
            [Required(ErrorMessage = "필수")]
            [MaxLength(500, ErrorMessage = "500자!")]
            public string Comment { get; set; }
            public bool? UseYn { get; set; }

            public int GrpNo { get; set; }
            public int GrpOrd { get; set; }
            public int Depth { get; set; }

            [MaxLength(30)]
            public string UserIpAddr { get; set; }
        }
    }
}
