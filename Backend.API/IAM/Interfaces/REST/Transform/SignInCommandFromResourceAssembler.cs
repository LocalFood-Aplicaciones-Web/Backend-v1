using Backend.API.IAM.Domain.Model.Commands;
using Backend.API.IAM.Interfaces.REST.Resources;

namespace Backend.API.IAM.Interfaces.REST.Transform;

public static class SignInCommandFromResourceAssembler
{
    public static SignInCommand ToCommandFromResource(SignInResource resource)
    {
        return new SignInCommand(resource.Username, resource.Password);
    }
}