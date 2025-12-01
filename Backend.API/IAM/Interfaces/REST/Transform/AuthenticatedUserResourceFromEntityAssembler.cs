using Backend.API.IAM.Domain.Model.Aggregates;
using Backend.API.IAM.Interfaces.REST.Resources;

namespace Backend.API.IAM.Interfaces.REST.Transform;

public static class AuthenticatedUserResourceFromEntityAssembler
{
    public static AuthenticatedUserResource ToResourceFromEntity(
        User user, string token)
    {
        return new AuthenticatedUserResource(user.Id, user.Username, token);
    }
}