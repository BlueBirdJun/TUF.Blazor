using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUF.Infrastructure.Identity.Tokens;

public record RefreshTokenRequest(string Token, string RefreshToken);