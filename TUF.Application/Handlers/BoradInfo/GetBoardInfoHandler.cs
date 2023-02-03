using Knus.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TUF.Application.Handlers.Boards.CreateUpdateHandler;


namespace TUF.Application.Handlers.BoradInfo;

public class GetBoardInfoHandler
{
    public class Query : IRequest<Result>
    {
        public string groupcode { get; set; }
    }
    public class Result : BaseDtoGeneric<List<DTO.Boards.BoardInfoDto.Response>, Query>
    {
    }

}
