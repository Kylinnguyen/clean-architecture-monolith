namespace CleanArchitecture.Domain.Common.Core.Auditing;

public interface IDeletionAuditable 
{
    string DeletedBy { get; set; }
    bool IsDeleted { get; set; }
    DateTimeOffset? DeletedTime { get; set; }
}