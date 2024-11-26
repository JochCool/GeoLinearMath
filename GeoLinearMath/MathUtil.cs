using System;
using System.Numerics;

namespace GeoLinearMath;

/// <summary>
/// Provides generic methods for common mathematical operations.
/// </summary>
public static class MathUtil
{
	/// <summary>
	/// Approximates the square root of a number in an unchecked context.
	/// </summary>
	/// <typeparam name="T">The type of number.</typeparam>
	/// <param name="value">The value of which to calculate the square root.</param>
	/// <returns>An approximation of the square root of <paramref name="value"/>.</returns>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is negative.</exception>
	public static T SqrtUnchecked<T>(T value) where T : INumber<T>
	{
		ArgumentOutOfRangeException.ThrowIfNegative(value);

		throw new NotImplementedException("Square roots will be implemented in a future version of GeoLinearMath.");
	}

	/// <summary>
	/// Approximates the square root of a number in a checked context.
	/// </summary>
	/// <typeparam name="T">The type of number.</typeparam>
	/// <param name="value">The value of which to calculate the square root.</param>
	/// <returns>An approximation of the square root of <paramref name="value"/>.</returns>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is negative.</exception>
	/// <exception cref="OverflowException">A value is not representable by <typeparamref name="T"/>.</exception>
	public static T Sqrt<T>(T value) where T : INumber<T>
	{
		ArgumentOutOfRangeException.ThrowIfNegative(value);

		throw new NotImplementedException("Square roots will be implemented in a future version of GeoLinearMath.");
	}
}
