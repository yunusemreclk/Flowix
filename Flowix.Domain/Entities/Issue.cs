using Flowix.Domain.Common;
using Flowix.Domain.Enums;
using Flowix.Domain.Exceptions;

namespace Flowix.Domain.Entities;

public sealed class Issue : AuditableEntity
{
    private readonly List<Comment> _comments = [];
    private readonly List<Attachment> _attachments = [];
    private readonly List<IssueLabel> _issueLabels = [];

    public Guid ProjectId { get; private set; }
    public int Number { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public IssueType Type { get; private set; }
    public IssuePriority Priority { get; private set; }
    public Guid StatusId { get; private set; }
    public Guid? AssigneeId { get; private set; }
    public Guid ReporterId { get; private set; }
    public decimal BoardOrder { get; private set; }
    public Guid? ParentIssueId { get; private set; }
    public DateOnly? DueDate { get; private set; }
    public byte[] RowVersion { get; private set; } = [];

    public IReadOnlyCollection<Comment> Comments => _comments;
    public IReadOnlyCollection<Attachment> Attachments => _attachments;
    public IReadOnlyCollection<IssueLabel> IssueLabels => _issueLabels;

    private Issue()
    {
    }

    public static Issue Create(
        Guid projectId,
        int number,
        string title,
        string? description,
        IssueType type,
        IssuePriority priority,
        Guid statusId,
        Guid reporterId,
        decimal boardOrder,
        Guid? parentIssueId = null)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new DomainException("İş başlığı boş olamaz.");
        }

        if (parentIssueId.HasValue && parentIssueId.Value == Guid.Empty)
        {
            throw new DomainException("Geçersiz üst görev.");
        }

        return new Issue
        {
            ProjectId = projectId,
            Number = number,
            Title = title.Trim(),
            Description = description,
            Type = type,
            Priority = priority,
            StatusId = statusId,
            ReporterId = reporterId,
            BoardOrder = boardOrder,
            ParentIssueId = parentIssueId
        };
    }

    public void ChangeStatus(Guid statusId, decimal boardOrder)
    {
        StatusId = statusId;
        BoardOrder = boardOrder;
    }

    public void AssignTo(Guid? userId) => AssigneeId = userId;

    public void UpdateDetails(string title, string? description, IssueType type, IssuePriority priority, DateOnly? dueDate)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new DomainException("İş başlığı boş olamaz.");
        }

        Title = title.Trim();
        Description = description;
        Type = type;
        Priority = priority;
        DueDate = dueDate;
    }

    public void SetParent(Guid? parentIssueId)
    {
        if (parentIssueId.HasValue && parentIssueId.Value == Id)
        {
            throw new DomainException("Bir iş kendi alt görevi olamaz.");
        }

        ParentIssueId = parentIssueId;
    }

    public Comment AddComment(Guid authorId, string content)
    {
        var comment = Comment.Create(Id, authorId, content);
        _comments.Add(comment);
        return comment;
    }

    public Attachment AddAttachment(Guid uploadedBy, string fileName, string filePath)
    {
        var attachment = Attachment.Create(Id, uploadedBy, fileName, filePath);
        _attachments.Add(attachment);
        return attachment;
    }

    public void AddLabel(Guid labelId)
    {
        if (_issueLabels.Any(il => il.LabelId == labelId))
        {
            throw new DomainException("Etiket zaten eklenmiş.");
        }

        _issueLabels.Add(IssueLabel.Create(Id, labelId));
    }

    public void RemoveLabel(Guid labelId)
    {
        var issueLabel = _issueLabels.FirstOrDefault(il => il.LabelId == labelId)
            ?? throw new DomainException("Etiket bu işte bulunamadı.");

        _issueLabels.Remove(issueLabel);
    }
}
