namespace Contracts.Domain.Base;

public interface IDomainEntityId : IDomainEntityId<string>
{
}

public interface IDomainEntityId<TKey>
    where TKey : IEquatable<TKey>

{
    TKey Id { get; set; }
}