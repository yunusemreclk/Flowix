using Flowix.Domain.Common;
using Flowix.Domain.Exceptions;

namespace Flowix.Domain.Entities;

public sealed class Label : BaseEntity
{
    public Guid ProjectId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Color { get; private set; } = "#808080";

    private Label()
    {
    }

    public static Label Create(Guid projectId, string name, string? color = null)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Etiket adı boş olamaz.");
        }

        return new Label
        {
            ProjectId = projectId,
            Name = name.Trim(),
            Color = string.IsNullOrWhiteSpace(color) ? "#808080" : color
        };
    }

    public void Rename(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Etiket adı boş olamaz.");
        }

        Name = name.Trim();
    }

    public void ChangeColor(string color) => Color = color;
}
