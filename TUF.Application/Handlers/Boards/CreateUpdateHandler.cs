using Knus.Common.Helpers;
using Knus.Common.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TUF.Database.DbContexts;


namespace TUF.Application.Handlers.Boards;

public class CreateUpdateHandler
{
    public class Query : IRequest<Result>
    {
        public BoardDto.Request parameter { get; set; }
    }
    public class Result:BaseDtoGeneric<DTO.Boards.BoardDto.Response,Query>
    { }

    public class Handler : IRequestHandler<Query, Result>
    {
        private readonly ILogger<CreateUpdateHandler> _logger;
        private readonly IApplicationDbContext _Appctx;
        public Handler(ILogger<CreateUpdateHandler> logger,
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
                DB.Board adddata = new Database.TUFDB.Board();
                adddata = req.parameter.Adapt<DB.Board>();
                
                adddata.LastModifiedOn = DateTime.Now;
                if (adddata.Id!=0)
                {
                    var sing= await _Appctx.Board.AsNoTracking().Where(p => p.Id == adddata.Id && p.CreatedBy == p.CreatedBy).SingleOrDefaultAsync();
                    if (sing != null)
                    {
                        adddata.CreatedBy = sing.CreatedBy;
                        _Appctx.Board.Update(adddata);
                    }
                    else
                        dt.ContentState = Daniel.Common.Enums.ContentState.NotContent;                    
                }
                else
                {
                    adddata.CreatedOn = DateTime.Now;
                    _Appctx.Board.Add(adddata);
                }
                dt.OutPutValue = adddata.Adapt<DTO.Boards.BoardDto.Response>();
                await _Appctx.SaveChangesAsync(cancellationToken);
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
