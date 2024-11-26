using System;
using System.Collections.Generic;
using System.Numerics;

namespace GeoLinearMath;

/// <summary>
/// Represents any vector of a constant dimension.
/// </summary>
/// <typeparam name="TSelf">The type that implements this interface.</typeparam>
/// <typeparam name="T">The type of the components of this vector type.</typeparam>
public interface IVector<TSelf, T> : IQuantity<T>,
	IAdditiveIdentity<TSelf, TSelf>,
	IAdditionOperators<TSelf, TSelf, TSelf>,
	ISubtractionOperators<TSelf, TSelf, TSelf>,
	IUnaryNegationOperators<TSelf, TSelf>,
	IUnaryPlusOperators<TSelf, TSelf>,

	IMultiplicativeIdentity<TSelf, T>,
	IMultiplicativeInverse<TSelf, TSelf>,
	IMultiplyOperators<TSelf, T, TSelf>,
	IDivisionOperators<TSelf, T, TSelf>,

	IEqualityOperators<TSelf, TSelf, bool>,
	IEquatable<TSelf>,

	ISpanParsable<TSelf>,
	ISpanFormattable

	where TSelf : IVector<TSelf, T>
	where T : INumber<T>
{
	/// <summary>
	/// Gets the dimension of this vector type.
	/// </summary>
	abstract static int Dimension { get; }

	/// <summary>
	/// Gets the vector whose magnitude is zero.
	/// </summary>
	abstract static TSelf Origin { get; }

	/// <summary>
	/// Gets all unit vectors that are along an axis, in arbitrary order.
	/// </summary>
	abstract static IEnumerable<TSelf> AxisUnitVectors { get; }

	/// <summary>
	/// Creates a vector with a specified span containing the components.
	/// </summary>
	/// <param name="components">The components of the vector.</param>
	/// <returns>The created vector.</returns>
	/// <exception cref="ArgumentException"><paramref name="components"/> is not a supported length.</exception>
	abstract static TSelf Create(params ReadOnlySpan<T> components);

	/// <summary>
	/// Gets the components of the vector, in order.
	/// </summary>
	IEnumerable<T> Components { get; }

	/// <summary>
	/// Gets the magnitude of this vector in taxicab distance (also known as Manhattan distance), in an unchecked context.
	/// </summary>
	/// <remarks>
	/// <para>This is equal to the amount of unit-sized steps along an axis needed to take to get from <see cref="Origin"/> to this vector.</para>
	/// </remarks>
	T TaxicabMagnitudeUnchecked { get; }

	/// <summary>
	/// Gets the magnitude of this vector in taxicab distance (also known as Manhattan distance), in a checked context.
	/// </summary>
	/// <remarks>
	/// <para>This is equal to the amount of unit-sized steps along an axis needed to take to get from <see cref="Origin"/> to this vector.</para>
	/// </remarks>
	/// <exception cref="OverflowException">The result is not representable by <typeparamref name="T"/>.</exception>
	T TaxicabMagnitude => TaxicabMagnitudeUnchecked;

	/// <summary>
	/// Tests whether this vector is in a rectangular box defined by two other vectors.
	/// </summary>
	/// <param name="min">The lower corner of the box (inclusive).</param>
	/// <param name="max">The upper corner of the box (inclusive).</param>
	/// <returns><see langword="true"/> if this vector is at or between <paramref name="max"/> and <paramref name="max"/>; otherwise, false.</returns>
	bool IsInBox(TSelf min, TSelf max);

	/// <summary>
	/// Clamps a vector to a box defined by two other vectors.
	/// </summary>
	/// <param name="value">The value to clamp.</param>
	/// <param name="min">The lower corner of the box to which <paramref name="value"/> should clamp (inclusive).</param>
	/// <param name="max">The upper corner of the box to which <paramref name="value"/> should clamp (inclusive).</param>
	/// <returns>The result of clamping <paramref name="value"/> in the box.</returns>
	static abstract TSelf Clamp(TSelf value, TSelf min, TSelf max);

	/// <summary>
	/// Calculates the dot product, or inner product, of two vectors, in an unchecked context.
	/// </summary>
	/// <param name="left">The vector to multiply by <paramref name="right"/>.</param>
	/// <param name="right">The vector to multiply by <paramref name="left"/>.</param>
	/// <returns>The dot product.</returns>
	/// <exception cref="OverflowException">The result is not representable by <typeparamref name="T"/>.</exception>
	static abstract T DotUnchecked(TSelf left, TSelf right);

	/// <summary>
	/// Calculates the dot product, or inner product, of two vectors, in a checked context.
	/// </summary>
	/// <param name="left">The vector to multiply by <paramref name="right"/>.</param>
	/// <param name="right">The vector to multiply by <paramref name="left"/>.</param>
	/// <returns>The dot product.</returns>
	/// <exception cref="OverflowException">The result is not representable by <typeparamref name="T"/>.</exception>
	static virtual T Dot(TSelf left, TSelf right) => TSelf.DotUnchecked(left, right);
}
