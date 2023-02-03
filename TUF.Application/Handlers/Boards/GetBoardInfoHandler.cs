using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Knus.Common.Models;
using Microsoft.Extensions.Logging;
using TUF.Database.DbContexts;

using Knus.Common.Helpers;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Mapster;

namespace TUF.Application.Handlers.Boards;

public class GetBoardInfoHandler
{
    public class Query :IRequest<Result>
    {
    }
    public class Result:BaseDtoGeneric<List<DTO.Boards.BoardInfoDto.Response>, Query> {
        
    
    }

    public class Handler : IRequestHandler<Query, Result>
    {
        private readonly ILogger<GetBoardInfoHandler> _logger;
        private readonly IApplicationDbContext _Appctx;
        public Handler (ILogger<GetBoardInfoHandler> logger,
          IApplicationDbContext Appctx)
        {
            _logger = logger;
            _Appctx = Appctx;
        }
        public async Task<Result> Handle(Query req, CancellationToken cancellationToken)
        {
            Result dt = new Result();
            try
            {
                var sing = await _Appctx.BoardInfo.AsNoTracking().ToListAsync();
                List<DTO.Boards.BoardInfoDto.Response> lst = new List<DTO.Boards.BoardInfoDto.Response>();
                foreach(var board in sing)
                {
                    lst.Add(board.Adapt<DTO.Boards.BoardInfoDto.Response>());
                }
                dt.OutPutValue = lst;
                dt.Success = true;
            }
            catch (Exception exc)
            {
                dt.Success = false;
                dt.HasError = true;
                dt.Message = exc.Message;
                dt.SystemMessage = ExcetionHelper.ExceptionMessage(exc);
            }
            return dt;
        }
    }
}
