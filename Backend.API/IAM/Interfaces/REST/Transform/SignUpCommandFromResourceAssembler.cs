using Backend.API.IAM.Domain.Model.Commands;
using Backend.API.IAM.Interfaces.REST.Resources;

namespace Backend.API.IAM.Interfaces.REST.Transform;

public static class SignUpCommandFromResourceAssembler
{
    public static SignUpCommand ToCommandFromResource(SignUpResource resource)
    {
        return new SignUpCommand(resource.Username, resource.Password);
    }
}