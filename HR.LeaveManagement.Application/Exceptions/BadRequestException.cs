using FluentValidation;
using FluentValidation.Results;

namespace HR.LeaveManagement.Application.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string message) : base(message)
        {

        }

        public BadRequestException(string message, ValidationResult validationResult) : base(message)
        {
            ValidationErrores = new();

            foreach(var error in validationResult.Errors)
            {
                ValidationErrores.Add(error.ErrorMessage);
            }
        }

        public List<string> ValidationErrores { get; set; }
    }
}

