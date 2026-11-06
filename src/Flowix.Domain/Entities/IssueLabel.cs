using Flowix.Domain.Common;

namespace Flowix.Domain.Entities;

public sealed class IssueLabel : BaseEntity
{
    public Guid IssueId { get; private set; }
    public Guid LabelId { get; private set; }

    private IssueLabel()
    {
    }

    internal static IssueLabel Create(Guid issueId, Guid labelId)
    {
        return new IssueLabel
        {
            IssueId = issueId,
            LabelId = labelId
        };
    }
}
