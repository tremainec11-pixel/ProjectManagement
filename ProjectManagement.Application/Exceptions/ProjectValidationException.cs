namespace ProjectManagement.Application.Exceptions;

public class ProjectValidationException : Exception
{
    public ProjectValidationException(string message)
        : base(message)
    {
    }
}
