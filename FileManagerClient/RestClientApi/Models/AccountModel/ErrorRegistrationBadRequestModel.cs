using System.Collections.Generic;
using System.Text;

namespace RestClientApi.Models.AccountModel
{
    public class ErrorRegistrationBadRequestModel
    {
        public List<string> Username { get; set; }
        public List<string> Email { get; set; }
        public List<string> Password { get; set; }
        public List<string> ConfirmPassword { get; set; }

        public override string ToString()
        {
            StringBuilder string_builder = new StringBuilder();
            string_builder.AppendLine("Список ошибок: ");
            int counter = 1;
            
            if (Username != null)
            {
                foreach (var errors in Username)
                {
                    string_builder.AppendLine($"{counter}.Никнейм: {errors}.");
                    counter++;
                }
            }

            if (Email != null)
            {
                foreach (var errors in Email)
                {
                    string_builder.AppendLine($"{counter}.Емаил: {errors}.");
                    counter++;
                }
            }

            if (Password != null)
            {
                foreach (var errors in Password)
                {
                    string_builder.AppendLine($"{counter}.Пароль: {errors}.");
                    counter++;
                }
            }

            if (ConfirmPassword != null)
            {
                foreach (var errors in ConfirmPassword)
                {
                    string_builder.AppendLine($"{counter}.Повторный пароль: {errors}.");
                    counter++;
                }
            }
            return string_builder.ToString();
        }
    }
}
