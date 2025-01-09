namespace UI.Core;

public static class OneOfExtensions
{
    public static T0 GetT0OrDefault<T0, T1>(this OneOf<T0, T1> oneOf, T0 defaultValue)
    {
        return oneOf.Match(
            t0 => t0,
            _ => defaultValue
        );
    }

    public static T1 GetT1OrDefault<T0, T1>(this OneOf<T0, T1> oneOf, T1 defaultValue)
    {
        return oneOf.Match(
            _ => defaultValue,
            t1 => t1
        );
    }

    public static T0 GetT0OrDefault<T0, T1, T2>(this OneOf<T0, T1, T2> oneOf, T0 defaultValue)
    {
        return oneOf.Match(
            t0 => t0,
            _ => defaultValue,
            _ => defaultValue
        );
    }
}