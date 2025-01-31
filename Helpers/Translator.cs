using Microsoft.AspNetCore.Identity;

namespace TechnicalConditions.Helpers
{
    public static class Translator
    {
        public static string ToRussian(this IdentityError identityError)
        {
			return identityError.Code switch
			{
				"PasswordRequiresNonAlphanumeric" => "Пароль должен включать один спец. символ (не буква и не цифра).",
				"PasswordRequiresLower" => "Пароль должен содержать букву в нижнем регистре.",
				"PasswordRequiresUpper" => "Пароль должен содержать букву в верхнем регистре.",
                "DuplicateUserName" => "",
                "DuplicateEmail" => "Email уже зарегистрирован",
                _ => identityError.Description
			};
        }
	}
}