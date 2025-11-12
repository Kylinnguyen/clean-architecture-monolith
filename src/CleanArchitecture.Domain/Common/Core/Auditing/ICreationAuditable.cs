namespace CleanArchitecture.Domain.Common.Core.Auditing;

public interface ICreationAuditable
{
    string? CreatedBy { get; set; }
    DateTimeOffset? CreatedTime { get; set; }
}