namespace CleanArchitecture.Domain.Common.Core.Auditing;

public interface IAuditable :
    ICreationAuditable,
    IModificationAuditable
{
}