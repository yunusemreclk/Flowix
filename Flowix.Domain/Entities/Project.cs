using Flowix.Domain.Common;
using Flowix.Domain.Enums;
using Flowix.Domain.Exceptions;

namespace Flowix.Domain.Entities;

public sealed class Project : AuditableEntity
{
    private readonly List<IssueStatus> _statuses = [];
    private readonly List<ProjectMember> _members = [];
    private readonly List<Issue> _issues = [];

    public string Name { get; private set; } = string.Empty;
    public string Key { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public Guid OwnerId { get; private set; }
    public int NextIssueNumber { get; private set; } = 1;

    public IReadOnlyCollection<IssueStatus> Statuses => _statuses;
    public IReadOnlyCollection<ProjectMember> Members => _members;
    public IReadOnlyCollection<Issue> Issues => _issues;

    private Project()
    {
    }

    public static Project Create(string name, string key, string? description, Guid ownerId)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Proje adı boş olamaz.");
        }

        if (string.IsNullOrWhiteSpace(key))
        {
            throw new DomainException("Proje anahtarı boş olamaz.");
        }

        var project = new Project
        {
            Name = name.Trim(),
            Key = key.Trim().ToUpperInvariant(),
            Description = description,
            OwnerId = ownerId
        };

        project._statuses.Add(IssueStatus.Create(project.Id, "To Do", StatusCategory.ToDo, 0));
        project._statuses.Add(IssueStatus.Create(project.Id, "In Progress", StatusCategory.InProgress, 1));
        project._statuses.Add(IssueStatus.Create(project.Id, "Done", StatusCategory.Done, 2));

        project._members.Add(ProjectMember.Create(project.Id, ownerId, ProjectRole.Owner));

        return project;
    }

    public void AddMember(Guid userId, ProjectRole role)
    {
        if (_members.Any(m => m.UserId == userId))
        {
            throw new DomainException("Kullanıcı zaten projenin üyesi.");
        }

        _members.Add(ProjectMember.Create(Id, userId, role));
    }

    public void RemoveMember(Guid userId)
    {
        var member = _members.FirstOrDefault(m => m.UserId == userId)
            ?? throw new DomainException("Kullanıcı projenin üyesi değil.");

        if (member.UserId == OwnerId)
        {
            throw new DomainException("Proje sahibi üyelikten çıkarılamaz.");
        }

        _members.Remove(member);
    }

    public int GetNextIssueNumber()
    {
        var number = NextIssueNumber;
        NextIssueNumber++;
        return number;
    }

    public void AddStatus(string name, StatusCategory category, string? color = null)
    {
        var order = _statuses.Count == 0 ? 0 : _statuses.Max(s => s.Order) + 1;
        _statuses.Add(IssueStatus.Create(Id, name, category, order, color));
    }

    public void RemoveStatus(Guid statusId)
    {
        var status = _statuses.FirstOrDefault(s => s.Id == statusId)
            ?? throw new DomainException("Kolon bulunamadı.");

        if (_statuses.Count <= 1)
        {
            throw new DomainException("Projede en az bir kolon olmalı.");
        }

        _statuses.Remove(status);
    }

    public void Rename(string name, string? description)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Proje adı boş olamaz.");
        }

        Name = name.Trim();
        Description = description;
    }
}
