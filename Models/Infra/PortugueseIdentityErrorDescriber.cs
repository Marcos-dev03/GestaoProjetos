using Microsoft.AspNetCore.Identity;

namespace Gestão_de_projetos.Models.Infra
{
	public class PortugueseIdentityErrorDescriber : IdentityErrorDescriber
	{
		public override IdentityError PasswordTooShort(int length)
		{
			return new IdentityError
			{
				Code = nameof(PasswordTooShort),
				Description = $"A senha deve possuir pelo menos {length} caracteres."
			};
		}

		public override IdentityError PasswordRequiresNonAlphanumeric()
		{
			return new IdentityError
			{
				Code = nameof(PasswordRequiresNonAlphanumeric),
				Description = "A senha deve possuir pelo menos um caractere especial."
			};
		}

		public override IdentityError PasswordRequiresDigit()
		{
			return new IdentityError
			{
				Code = nameof(PasswordRequiresDigit),
				Description = "A senha deve possuir pelo menos um número."
			};
		}

		public override IdentityError PasswordRequiresLower()
		{
			return new IdentityError
			{
				Code = nameof(PasswordRequiresLower),
				Description = "A senha deve possuir pelo menos uma letra minúscula."
			};
		}

		public override IdentityError PasswordRequiresUpper()
		{
			return new IdentityError
			{
				Code = nameof(PasswordRequiresUpper),
				Description = "A senha deve possuir pelo menos uma letra maiúscula."
			};
		}

		public override IdentityError DuplicateUserName(string userName)
		{
			return new IdentityError
			{
				Code = nameof(DuplicateUserName),
				Description = $"O nome de usuário '{userName}' já está sendo utilizado."
			};
		}

		public override IdentityError DuplicateEmail(string email)
		{
			return new IdentityError
			{
				Code = nameof(DuplicateEmail),
				Description = $"O e-mail '{email}' já está sendo utilizado."
			};
		}
	}
}