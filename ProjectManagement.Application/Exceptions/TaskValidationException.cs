namespace ProjectManagement.Application.Exceptions;

public class TaskValidationException : Exception
{
    public TaskValidationException(string message)
        : base(message)
    {
    }
}
