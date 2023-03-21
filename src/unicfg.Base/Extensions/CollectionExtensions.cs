namespace unicfg.Base.Extensions;

public static class CollectionExtensions
{
    public static ImmutableArray<TValue> ToImmutableArrayOfValues<TKey, TValue>(this IDictionary<TKey, TValue> @this)
    {
        var builder = ImmutableArray.CreateBuilder<TValue>(@this.Count);
        builder.AddRange(@this.Values);
        return builder.MoveToImmutable();
    }
}