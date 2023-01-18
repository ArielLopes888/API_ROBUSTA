using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using FluentValidation;

namespace Domain.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(x => x)
                .NotEmpty()
                .WithMessage("O usuário não pode ser vazio")

                .NotNull()
                .WithMessage("O usuário não pode ser nulo");


            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("O nome não pode ser vazio")

                .NotNull()
                .WithMessage("O nome não pode ser nulo")

                .MinimumLength(3)
                .WithMessage("O nome deve ter no mínimo 3 caracteres.")

                .MaximumLength(80)
                .WithMessage("O nome deve ter no máximo 80 caracteres.");


            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("O Email não pode ser vazio")

                .NotNull()
                .WithMessage("O Email não pode ser nulo")

                .MinimumLength(10)
                .WithMessage("O Email deve ter no mínimo 10 caracteres.")

                .MaximumLength(180)
                .WithMessage("O Email deve ter no máximo 180 caracteres.")

                .Matches(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$")
                .WithMessage("O e-mail informado não é válido");
                 

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("A senha não pode ser vazia")

                .NotNull()
                .WithMessage("A senha não pode ser nula")

                .MinimumLength(10)
                .WithMessage("A senha deve ter no mínimo 10 caracteres.")

                .MaximumLength(80)
                .WithMessage("A senha deve ter no máximo 80 caracteres.");


        }
    }
}
