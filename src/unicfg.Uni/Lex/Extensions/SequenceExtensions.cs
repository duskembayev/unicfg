namespace unicfg.Uni.Lex.Extensions;

public static class SequenceExtensions
{
    public static Index AsIndex(this in SequencePosition @this)
    {
        return new Index(@this.GetInteger());
    }

    public static Range AsRange(this in SequencePosition @this, int length)
    {
        if (length <= 0)
        {
            throw new InvalidOperationException();
        }

        var index = @this.GetInteger();
        return new Range(index, index + length);
    }

    public static Range AsRange(this in SequencePosition @this, in SequencePosition end)
    {
        return new Range(@this.AsIndex(), end.AsIndex());
    }

    public static SequencePosition Next(this in SequencePosition @this)
    {
        return new SequencePosition(@this.GetObject(), @this.GetInteger() + 1);
    }
}
