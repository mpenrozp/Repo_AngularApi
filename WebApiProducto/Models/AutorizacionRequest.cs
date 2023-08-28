using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace WebApiProducto.Models
{
    public class AutorizacionRequest
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
    public class UserValidator : AbstractValidator<AutorizacionRequest>
    {
        public UserValidator()
        {
            Include(new UserValidatorUserName());
            Include(new UserValidatorPassword());
        }
    }
    public class UserValidatorUserName : AbstractValidator<AutorizacionRequest>
    {
        public UserValidatorUserName()
        {
            RuleFor(user => user.UserName).Cascade(CascadeMode.Stop)
                                          .NotEmpty().WithMessage("Debe indicar el nombre del usuario")
                                          .Length(2, 5).WithMessage("{PropertyName} tiene {TotalLength} letras. Debe tener una longitud entre {MinLength} y {MaxLength} letras.");
        }
    }
    public class UserValidatorPassword : AbstractValidator<AutorizacionRequest>
    {
        public UserValidatorPassword()
        {
            RuleFor(user => user.Password).Cascade(CascadeMode.Stop)
                                          .NotEmpty().WithMessage("Debe indicar la password del usuario")
                                          .Length(2, 5).WithMessage("{PropertyName} tiene {TotalLength} letras. Debe tener una longitud entre {MinLength} y {MaxLength} letras.");
        }
    }
}