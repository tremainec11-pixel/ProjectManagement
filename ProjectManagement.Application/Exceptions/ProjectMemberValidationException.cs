namespace ProjectManagement.Application.Exceptions;

public class ProjectMemberValidationException : Exception
{
    public ProjectMemberValidationException(string message)
        : base(message)
    {
    }
}
