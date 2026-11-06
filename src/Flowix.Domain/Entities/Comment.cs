using Flowix.Domain.Common;
using Flowix.Domain.Exceptions;

namespace Flowix.Domain.Entities;

public sealed class Comment : AuditableEntity
{
    public Guid IssueId { get; private set; }
    public Guid AuthorId { get; private set; }
    public string Content { get; private set; } = string.Empty;

    private Comment()
    {
    }

    internal static Comment Create(Guid issueId, Guid authorId, string content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            throw new DomainException("Yorum içeriği boş olamaz.");
        }

        return new Comment
        {
            IssueId = issueId,
            AuthorId = authorId,
            Content = content.Trim()
        };
    }

    public void Edit(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            throw new DomainException("Yorum içeriği boş olamaz.");
        }

        Content = content.Trim();
    }
}
