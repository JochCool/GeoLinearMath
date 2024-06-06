namespace GeoLinearMath;

/// <summary>
/// Defines a mechanism for calculating a multiplicative inverse of a value (which, when multiplied with the current value, yields 1).
/// </summary>
/// <typeparam name="TSelf">The type that implements this interface.</typeparam>
/// <typeparam name="TResult">The type of the result of the reciprocal operation.</typeparam>
public interface IMultiplicativeInverse<TSelf, TResult>
{
	/// <summary>
	/// Calculates the multiplicative inverse of a value.
	/// </summary>
	/// <param name="value">The value for which to calculate the multiplicative inverse.</param>
	/// <returns>The multiplicative inverse of <paramref name="value"/>.</returns>
	static abstract TResult Reciprocal(TSelf value);
}
