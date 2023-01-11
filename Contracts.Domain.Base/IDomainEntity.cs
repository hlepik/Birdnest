using Contracts.Domain.Base;

public interface IDomainEntity : IDomainEntity<string>
{
}

public interface IDomainEntity<TKey> : IDomainEntityId<TKey>
    where TKey : IEquatable<TKey>
{
}