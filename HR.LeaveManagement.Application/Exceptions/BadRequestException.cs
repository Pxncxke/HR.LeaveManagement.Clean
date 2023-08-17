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
            ValidationErrores = validationResult.ToDictionary();
        }

        public IDictionary<string, string[]> ValidationErrores { get; set; }
    }
}

