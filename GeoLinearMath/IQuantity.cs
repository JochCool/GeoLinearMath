using System;
using System.Numerics;

namespace GeoLinearMath;

/// <summary>
/// Represents a quantity that has a magnitude.
/// </summary>
public interface IQuantity<TResult>
	where TResult : INumber<TResult>
{
	/// <summary>
	/// Gets the square of the magnitude of this quantity in an unchecked context.
	/// </summary>
	TResult SquareMagnitudeUnchecked { get; }

	/// <summary>
	/// Gets the square of the magnitude of this quantity in a checked context.
	/// </summary>
	/// <exception cref="OverflowException">The result is not representable by <typeparamref name="TResult"/>.</exception>
	TResult SquareMagnitude => SquareMagnitudeUnchecked;

	/// <summary>
	/// Gets the magnitude (or absolute value) of this quantity in an unchecked context.
	/// </summary>
	/// <remarks>
	/// <para>It is typically cheaper to calculate the square of the magnitude instead, so consider using <see cref="SquareMagnitudeUnchecked"/> for comparisons.</para>
	/// </remarks>
	TResult MagnitudeUnchecked => MathUtil.SqrtUnchecked(SquareMagnitudeUnchecked);

	/// <summary>
	/// Gets the magnitude (or absolute value) of this quantity in a checked context.
	/// </summary>
	/// <exception cref="OverflowException">The result is not representable by <typeparamref name="TResult"/>.</exception>
	TResult Magnitude => MathUtil.Sqrt(Magnitude);
}
