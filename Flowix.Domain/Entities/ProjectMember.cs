using Flowix.Domain.Common;
using Flowix.Domain.Enums;

namespace Flowix.Domain.Entities;

public sealed class ProjectMember : BaseEntity
{
    public Guid ProjectId { get; private set; }
    public Guid UserId { get; private set; }
    public ProjectRole Role { get; private set; }

    private ProjectMember()
    {
    }

    internal static ProjectMember Create(Guid projectId, Guid userId, ProjectRole role)
    {
        return new ProjectMember
        {
            ProjectId = projectId,
            UserId = userId,
            Role = role
        };
    }

    public void ChangeRole(ProjectRole role) => Role = role;
}
