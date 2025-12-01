using Backend.API.IAM.Domain.Model.Aggregates;
using Backend.API.IAM.Interfaces.REST.Resources;

namespace Backend.API.IAM.Interfaces.REST.Transform;

public static class UserResourceFromEntityAssembler
{
    public static UserResource ToResourceFromEntity(User user)
    {
        return new UserResource(user.Id, user.Username);
    }
}