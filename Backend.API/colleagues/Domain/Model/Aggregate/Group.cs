using System.ComponentModel.DataAnnotations.Schema;
using EntityFrameworkCore.CreatedUpdatedDate.Contracts;

namespace Backend.API.colleagues.Domain.Model.Aggregate;

/// <summary>
///     Group aggregate root entity.
/// </summary>
public class Group : IEntityWithCreatedUpdatedDate
{
    public Group()
    {
        Name = string.Empty;
        Description = string.Empty;
        Color = "#FFFFFF";
        Colleagues = new List<Colleague>();
    }

    public Group(string name, string description, string color, int userId) : this()
    {
        Name = name;
        Description = description;
        Color = color;
        UserId = userId;
    }

    public int Id { get; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public string Color { get; private set; }
    
    // Tenant / Dueño
    public int UserId { get; private set; }

    // Relación inversa (Un grupo tiene muchos colegas)
    public ICollection<Colleague> Colleagues { get; }

    // Auditoría
    [Column("CreatedAt")] public DateTimeOffset? CreatedDate { get; set; }
    [Column("UpdatedAt")] public DateTimeOffset? UpdatedDate { get; set; }

    public void UpdateDetails(string name, string description, string color)
    {
        Name = name;
        Description = description;
        Color = color;
    }
}