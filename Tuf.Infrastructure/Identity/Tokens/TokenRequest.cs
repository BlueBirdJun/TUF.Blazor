using Daniel.Common.Interfaces;
using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUF.Application.Identity.Tokens;

public record TokenRequest(string Email, string Password);

public class TokenRequestValidator : CustomValidator<TokenRequest>
{
    //public TokenRequestValidator(IStringLocalizer<TokenRequestValidator> T)
    //{
    //    RuleFor(p => p.Email).Cascade(CascadeMode.Stop)
    //        .NotEmpty()
    //        .EmailAddress()
    //            .WithMessage(T["Invalid Email Address."]);

    //    RuleFor(p => p.Password).Cascade(CascadeMode.Stop)
    //        .NotEmpty();
    //}
    public TokenRequestValidator()
    {
        RuleFor(p => p.Email).Cascade(CascadeMode.Stop)
            .NotEmpty()
            .EmailAddress()
                .WithMessage("Invalid Email Address.");

        RuleFor(p => p.Password).Cascade(CascadeMode.Stop)
            .NotEmpty();
    }
}
