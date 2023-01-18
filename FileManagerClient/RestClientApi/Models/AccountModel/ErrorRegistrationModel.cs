using System.Collections.Generic;
using System.Text;

namespace RestClientApi.Models.AccountModel
{
    public class ErrorRegistrationModel
    {
        public bool succeeded { get; set; }
        public List<Error> errors { get; set; }

        public override string ToString()
        {
            StringBuilder string_builder = new StringBuilder();
            string_builder.AppendLine("Список ошибок: ");
            int counter = 1;
            foreach(var errors in errors)
            {
                string_builder.AppendLine($"{counter}.Код ошибки: {errors.code}. Описание ошибки: {errors.description}");
                counter++;
            }
            return string_builder.ToString();
        }
    }
    public class Error
    {
        public string code { get; set; }
        public string description { get; set; }
    }
}
