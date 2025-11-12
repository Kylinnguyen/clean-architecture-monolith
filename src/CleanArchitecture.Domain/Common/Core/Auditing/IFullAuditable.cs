namespace CleanArchitecture.Domain.Common.Core.Auditing;

public interface IFullAuditable : 
    ICreationAuditable,
    IModificationAuditable,
    IDeletionAuditable
{
}