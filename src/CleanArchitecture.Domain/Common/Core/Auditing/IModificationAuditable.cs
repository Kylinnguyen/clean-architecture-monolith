namespace CleanArchitecture.Domain.Common.Core.Auditing;

public interface IModificationAuditable
{
    string? LastModifiedBy { get; set; }
    DateTimeOffset? LastModifiedTime { get; set; }
}