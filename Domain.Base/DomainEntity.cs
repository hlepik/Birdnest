using Contracts.Domain.Base;

namespace Domain.Base;

public class DomainEntity : DomainEntity<string>, IDomainEntity
{
}

public class DomainEntity<TKey> : DomainEntityId<TKey>, IDomainEntity<TKey>
    where TKey : IEquatable<TKey>
{
}