using System.Collections.Generic;
using System.Text;

namespace RestClientApi.Models.AccountModel
{
    public class ErrorLoginBadRequestModel
    {
        public List<string> Username { get; set; }
        public List<string> Password { get; set; }

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

            if (Password != null)
            {
                foreach (var errors in Password)
                {
                    string_builder.AppendLine($"{counter}.Пароль: {errors}.");
                    counter++;
                }
            }

            return string_builder.ToString();
        }
    }
}
