using Flowix.Domain.Common;
using Flowix.Domain.Exceptions;

namespace Flowix.Domain.Entities;

public sealed class Attachment : BaseEntity
{
    public Guid IssueId { get; private set; }
    public Guid UploadedBy { get; private set; }
    public string FileName { get; private set; } = string.Empty;
    public string FilePath { get; private set; } = string.Empty;
    public DateTimeOffset UploadedAt { get; private set; }

    private Attachment()
    {
    }

    internal static Attachment Create(Guid issueId, Guid uploadedBy, string fileName, string filePath)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            throw new DomainException("Dosya adı boş olamaz.");
        }

        return new Attachment
        {
            IssueId = issueId,
            UploadedBy = uploadedBy,
            FileName = fileName.Trim(),
            FilePath = filePath,
            UploadedAt = DateTimeOffset.UtcNow
        };
    }
}
