using Knus.Common.Helpers;
using Knus.Common.Interfaces;
using Knus.Common.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUF.Database.DbContexts;

namespace TUF.Application.Handlers.Commons;

public class SeedHandler
{
    public class Query : IRequest<Result>
    {
    }
    public class Result : BaseDto
    {
    }

    public class Handler : IRequestHandler<Query, Result>
    {
        private readonly ILogger<SeedHandler> _logger;
        private readonly IApplicationDbContext _Appctx;
        public Handler(ILogger<SeedHandler> logger,
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

                _Appctx.BoardInfo.Add(new Database.TUFDB.BoardInfo()
                {
                    GroupCode = "Board",
                    Bkey = "B001",
                    BoardName = "일상/유머",
                    BoardDesc = "일상/유머",
                    EditorYn = true,
                    CommentYn = true,
                    ImageYn = true,
                    Expiredate = DateTime.Parse("2025-01-01"),
                    UseYn = true,
                    sort = 0,
                    CreatedOn = DateTime.Now,
                    CreatedBy="Daniel",
                    LastModifiedOn = DateTime.Now,
                    LastModifiedBy = "Daniel"
                });
                _Appctx.BoardInfo.Add(new Database.TUFDB.BoardInfo()
                {
                    GroupCode = "Board",
                    Bkey = "B002",
                    BoardName = "자유게시판",
                    BoardDesc = "자유게시판",
                    EditorYn = true,
                    CommentYn = true,
                    ImageYn = true,
                    Expiredate = DateTime.Parse("2025-01-01"),
                    UseYn = true,
                    sort = 0,
                    CreatedOn = DateTime.Now,
                    CreatedBy = "Daniel",
                    LastModifiedOn = DateTime.Now,
                    LastModifiedBy = "Daniel"
                });
                _Appctx.BoardInfo.Add(new Database.TUFDB.BoardInfo()
                {
                    GroupCode = "Board",
                    Bkey = "B003",
                    BoardName = "기타",
                    BoardDesc = "기타",
                    EditorYn = true,
                    CommentYn = true,
                    ImageYn = true,
                    Expiredate = DateTime.Parse("2025-01-01"),
                    UseYn = true,
                    sort = 0,
                    CreatedOn = DateTime.Now,
                    CreatedBy = "Daniel",
                    LastModifiedOn = DateTime.Now,
                    LastModifiedBy= "Daniel"
                });

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
