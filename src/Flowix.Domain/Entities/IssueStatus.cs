using Flowix.Domain.Common;
using Flowix.Domain.Enums;
using Flowix.Domain.Exceptions;

namespace Flowix.Domain.Entities;

public sealed class IssueStatus : BaseEntity
{
    public Guid ProjectId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public StatusCategory Category { get; private set; }
    public int Order { get; private set; }
    public string? Color { get; private set; }

    private IssueStatus()
    {
    }

    internal static IssueStatus Create(Guid projectId, string name, StatusCategory category, int order, string? color = null)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Kolon adı boş olamaz.");
        }

        return new IssueStatus
        {
            ProjectId = projectId,
            Name = name.Trim(),
            Category = category,
            Order = order,
            Color = color
        };
    }

    public void Rename(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Kolon adı boş olamaz.");
        }

        Name = name.Trim();
    }

    public void Reorder(int order) => Order = order;

    public void ChangeColor(string? color) => Color = color;
}
