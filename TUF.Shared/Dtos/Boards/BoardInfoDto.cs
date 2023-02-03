

using Daniel.Common.Models;

namespace TUF.Shared.Dtos.Boards;

public class BoardInfoDto : DtoBase< List<BoardInfoDto.Response>, BoardInfoDto.Request>
{
    public class Request
    {
        public readonly ApiMetaData CreateMeta = new ApiMetaData
        {
            Title = "게시판정보",
            httpmethod = Knus.Common.Services.HttpMethods.GET,
            UrlPath = "/api/Board/boardinfo"
        };
    }

    public class Response: Body
    {
    }

    public class Body :AudiTableEntity
    {
        public string GroupCode { get; set; }
    
        public string Bkey { get; set; }
         
        public string BoardName { get; set; }
         
        public string BoardDesc { get; set; }
        public bool? EditorYn { get; set; }

        public bool? CommentYn { get; set; }
        public bool? ImageYn { get; set; }
        public DateTime? Expiredate { get; set; }
        public bool? UseYn { get; set; }
        public int sort { get; set; }
    }
}
