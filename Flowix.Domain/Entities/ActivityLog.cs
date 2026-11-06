using Flowix.Domain.Common;

namespace Flowix.Domain.Entities;

public sealed class ActivityLog : BaseEntity
{
    public Guid ProjectId { get; private set; }
    public Guid? IssueId { get; private set; }
    public Guid UserId { get; private set; }
    public string Action { get; private set; } = string.Empty;
    public string? Details { get; private set; }
    public DateTimeOffset OccurredAt { get; private set; }

    private ActivityLog()
    {
    }

    public static ActivityLog Create(Guid projectId, Guid userId, string action, Guid? issueId = null, string? details = null)
    {
        return new ActivityLog
        {
            ProjectId = projectId,
            IssueId = issueId,
            UserId = userId,
            Action = action,
            Details = details,
            OccurredAt = DateTimeOffset.UtcNow
        };
    }
}
