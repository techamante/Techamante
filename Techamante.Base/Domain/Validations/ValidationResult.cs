namespace Techamante.Domain.Validations
{
    public class ValidationResult
    {
        public bool IsSuccess { get; set; }

        public ErrorMessage ErrorMessage { get; set; }
    }
}