namespace API.Security;

public sealed record CurrentUser : ICurrentUser
{
    public Guid Id { get; }

    public CurrentUser(IHttpContextAccessor accessor)
    {
        var id = accessor.HttpContext?.User.GetAuthenticatedUserId();
        
        if (id is null) 
            throw new UnauthorizedAccessException();

        Id = id.Value;
    }
}
