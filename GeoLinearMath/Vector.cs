using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace GeoLinearMath;

/// <summary>
/// Represents a two-dimentional vector, with x and y components.
/// </summary>
/// <typeparam name="T">The type of number that this is a vector of.</typeparam>
/// <seealso cref="Vector3D{T}"/>
public struct Vector<T> : IVector<Vector<T>, T>
	where T : INumber<T>
{
	/// <summary>
	/// Represents the x component of this vector.
	/// </summary>
	public T X;

	/// <summary>
	/// Represents the y component of this vector.
	/// </summary>
	public T Y;

	/// <summary>
	/// Constructs a two-dimensional vector with the same value for both components.
	/// </summary>
	/// <param name="value">The value for both components of the vector.</param>
	public Vector(T value)
	{
		X = value;
		Y = value;
	}

	/// <summary>
	/// Constructs a two-dimensional vector with specified x and y components.
	/// </summary>
	/// <param name="x">The x component of the vector.</param>
	/// <param name="y">The y component of the vector.</param>
	public Vector(T x, T y)
	{
		X = x;
		Y = y;
	}

	readonly IEnumerable<T> IVector<Vector<T>, T>.Components => [X, Y];

	/// <inheritdoc/>
	public readonly T SquareMagnitude => X * X + Y * Y;

	/// <inheritdoc/>
	public readonly T TaxicabMagnitude => T.Abs(X) + T.Abs(Y);

	static int IVector<Vector<T>, T>.Dimension => 2;

	static Vector<T> IAdditiveIdentity<Vector<T>, Vector<T>>.AdditiveIdentity => new Vector<T>(T.AdditiveIdentity, T.AdditiveIdentity);
	static T IMultiplicativeIdentity<Vector<T>, T>.MultiplicativeIdentity => T.MultiplicativeIdentity;

	/// <inheritdoc/>
	public static Vector<T> Origin => new Vector<T>(T.Zero, T.Zero);

	/// <summary>
	/// Gets the unit vector (1, 0).
	/// </summary>
	public static Vector<T> ToPositiveX => new Vector<T>(T.One, T.Zero);

	/// <summary>
	/// Gets the unit vector (0, 1).
	/// </summary>
	public static Vector<T> ToPositiveY => new Vector<T>(T.Zero, T.One);

	/// <summary>
	/// Gets the unit vector (-1, 0).
	/// </summary>
	public static Vector<T> ToNegativeX => new Vector<T>(-T.One, T.Zero);

	/// <summary>
	/// Gets the unit vector (0, -1).
	/// </summary>
	public static Vector<T> ToNegativeY => new Vector<T>(T.Zero, -T.One);

	/// <summary>
	/// Gets the four unit vectors that are along an axis.
	/// </summary>
	public static Vector<T>[] AxisUnitVectors =>
	[
		ToPositiveX,
		ToPositiveY,
		ToNegativeX,
		ToNegativeY
	];

	static IEnumerable<Vector<T>> IVector<Vector<T>, T>.AxisUnitVectors => AxisUnitVectors;

	/// <summary>
	/// Creates a two-dimensional vector with a specified span containing the two components.
	/// </summary>
	/// <param name="components">A span containing two elements, which are the components of the vector.</param>
	/// <returns>The created vector.</returns>
	/// <exception cref="ArgumentException"><paramref name="components"/> does not contain two elements.</exception>
	public static Vector<T> Create(ReadOnlySpan<T> components)
	{
		if (components.Length != 2)
		{
			throw new ArgumentException($"Expected 2 components for a two-dimensional vector, got {components.Length}.", nameof(components));
		}
		Vector<T> result;
		result.X = components[0];
		result.Y = components[1];
		return result;
	}

	/// <summary>
	/// Creates a vector of the current type from a vector of another type, throwing an overflow exception for any component values that fall outside the representable range of <typeparamref name="T"/>.
	/// </summary>
	/// <typeparam name="TOther">The type of the other vector.</typeparam>
	/// <param name="other">The vector used to create a vector of the current type.</param>
	/// <returns>The created vector.</returns>
	/// <exception cref="NotSupportedException">There is no conversion defined from <typeparamref name="TOther"/> to <typeparamref name="T"/>.</exception>
	/// <exception cref="OverflowException">The value of one of the components of <paramref name="other"/> is not representable by <typeparamref name="T"/>.</exception>
	public static Vector<T> CreateChecked<TOther>(Vector<TOther> other)
		where TOther : INumber<TOther>
	{
		return new Vector<T>(T.CreateChecked(other.X), T.CreateChecked(other.Y));
	}

	/// <summary>
	/// Creates a vector of the current type from a vector of another type, saturating any component values that fall outside the representable range of <typeparamref name="T"/>.
	/// </summary>
	/// <typeparam name="TOther">The type of the other vector.</typeparam>
	/// <param name="other">The vector used to create a vector of the current type.</param>
	/// <returns>The created vector, with any values that fall outside the representable of <typeparamref name="T"/> saturated.</returns>
	/// <exception cref="NotSupportedException">There is no conversion defined from <typeparamref name="TOther"/> to <typeparamref name="T"/>.</exception>
	public static Vector<T> CreateSaturating<TOther>(Vector<TOther> other)
		where TOther : INumber<TOther>
	{
		return new Vector<T>(T.CreateSaturating(other.X), T.CreateSaturating(other.Y));
	}

	/// <summary>
	/// Creates a vector of the current type from a vector of another type, truncating any component values that fall outside the representable range of <typeparamref name="T"/>.
	/// </summary>
	/// <typeparam name="TOther">The type of the other vector.</typeparam>
	/// <param name="other">The vector used to create a vector of the current type.</param>
	/// <returns>The created vector, with any values that fall outside the representable of <typeparamref name="T"/> truncated.</returns>
	/// <exception cref="NotSupportedException">There is no conversion defined from <typeparamref name="TOther"/> to <typeparamref name="T"/>.</exception>
	public static Vector<T> CreateTruncating<TOther>(Vector<TOther> other)
		where TOther : INumber<TOther>
	{
		return new Vector<T>(T.CreateTruncating(other.X), T.CreateTruncating(other.Y));
	}

	/// <inheritdoc/>
	public readonly bool IsInBox(Vector<T> min, Vector<T> max)
	{
		return X >= min.X && Y >= min.Y
			&& X <= max.X && Y <= max.Y;
	}

	#region Operations

	/// <inheritdoc/>
	public static Vector<T> Clamp(Vector<T> value, Vector<T> min, Vector<T> max)
	{
		return new Vector<T>(T.Clamp(value.X, min.X, max.X), T.Clamp(value.Y, min.Y, max.Y));
	}

	/// <summary>
	/// Gets a vector that has the components of another vector swapped (i.e. X becomes Y and Y becomes X).
	/// </summary>
	/// <param name="vector">The vector for which to get the swizzled vector.</param>
	/// <returns>The swizzled vector.</returns>
	public static Vector<T> Swizzle(Vector<T> vector) => new(vector.Y, vector.X);

	/// <inheritdoc/>
	public static Vector<T> operator +(Vector<T> value) => new Vector<T>(+value.X, +value.Y);

	/// <inheritdoc/>
	public static Vector<T> operator -(Vector<T> value) => new Vector<T>(-value.X, -value.Y);

	/// <inheritdoc/>
	public static Vector<T> operator +(Vector<T> left, Vector<T> right)
	{
		return new Vector<T>(left.X + right.X, left.Y + right.Y);
	}

	/// <inheritdoc/>
	public static Vector<T> operator -(Vector<T> left, Vector<T> right)
	{
		return new Vector<T>(left.X - right.X, left.Y - right.Y);
	}

	// TODO
	//public static ComplexNumber<T> operator *(Vector<T> left, Vector<T> right)
	//{
	//	return new ComplexNumber<T>(Dot(left, right), Determinant(left, right));
	//}

	/// <summary>
	/// Multiplies a vector by a scalar value.
	/// </summary>
	/// <param name="left">The vector to scale.</param>
	/// <param name="right">The value to scale the vector by.</param>
	/// <returns>The value of <paramref name="left"/> scaled by <paramref name="right"/>.</returns>
	public static Vector<T> operator *(Vector<T> left, T right)
	{
		return new Vector<T>(left.X * right, left.Y * right);
	}

	/// <summary>
	/// Multiplies a vector by a scalar value.
	/// </summary>
	/// <param name="left">The value to scale the vector by.</param>
	/// <param name="right">The vector to scale.</param>
	/// <returns>The value of <paramref name="right"/> scaled by <paramref name="left"/>.</returns>
	public static Vector<T> operator *(T left, Vector<T> right)
	{
		return new Vector<T>(left * right.X, left * right.Y);
	}

	/// <summary>
	/// Divides a vector by a scalar value.
	/// </summary>
	/// <param name="left">The vector to scale.</param>
	/// <param name="right">The inverse value to scale the vector by.</param>
	/// <returns>The value of <paramref name="left"/> scaled by the inverse of <paramref name="right"/>.</returns>
	public static Vector<T> operator /(Vector<T> left, T right)
	{
		return new Vector<T>(left.X / right, left.Y / right);
	}

	/// <summary>
	/// Divides a scalar value by a vector in VGA.
	/// </summary>
	/// <param name="left">The value which <paramref name="right"/> divides.</param>
	/// <param name="right">The value which divides <paramref name="left"/>.</param>
	/// <returns>The quotient of <paramref name="left"/> divided by <paramref name="right"/>.</returns>
	public static Vector<T> operator /(T left, Vector<T> right)
	{
		return left * right / right.SquareMagnitude;
	}

	/// <summary>
	/// Calculates the multiplicative inverse of a vector in VGA.
	/// </summary>
	/// <param name="value">The value of which to get the multiplicative inverse.</param>
	/// <returns>The multiplicative inverse of <paramref name="value"/>.</returns>
	public static Vector<T> Reciprocal(Vector<T> value)
	{
		return value / value.SquareMagnitude;
	}

	/// <inheritdoc/>
	public static T Dot(Vector<T> left, Vector<T> right)
	{
		return left.X * right.X + left.Y * right.Y;
	}

	/// <summary>
	/// Calculates the determinant of a 2×2 matrix defined by two vectors.
	/// </summary>
	/// <param name="a">The first vector of the matrix.</param>
	/// <param name="b">The second vector of the matrix.</param>
	/// <returns>The determinant.</returns>
	public static T Determinant(Vector<T> a, Vector<T> b)
	{
		return a.X * b.Y - a.Y * b.X;
	}

	#endregion

	#region Equality

	/// <summary>
	/// Computes a hash code for this vector.
	/// </summary>
	/// <returns>The hash code.</returns>
	public override readonly int GetHashCode() => HashCode.Combine(X, Y);

	/// <summary>
	/// Determines if an object is a vector of the same type and equal to the current vector.
	/// </summary>
	/// <param name="obj">The object to check.</param>
	/// <returns><see langword="true"/> if <paramref name="obj"/> is a <see cref="Vector{T}"/> instance of the same type, and equal to the current vector; otherwise, <see langword="false"/>.</returns>
	public override readonly bool Equals([NotNullWhen(true)] object? obj) => obj is Vector<T> vector && Equals(vector);

	/// <summary>
	/// Determines if the current vector is equal to another vector of the same type.
	/// </summary>
	/// <param name="other">A vector to compare with the current vector.</param>
	/// <returns><see langword="true"/> if the current vector is equal to <paramref name="other"/>; otherwise, <see langword="false"/>.</returns>
	public readonly bool Equals(Vector<T> other) => this == other;

	/// <inheritdoc/>
	public static bool operator ==(Vector<T> left, Vector<T> right) => left.X == right.X && left.Y == right.Y;

	/// <inheritdoc/>
	public static bool operator !=(Vector<T> left, Vector<T> right) => !(left == right);

	#endregion

	#region String conversions

	/// <summary>
	/// Gets the string representation of the current vector, using this thread's current culture.
	/// </summary>
	/// <returns>The string representation.</returns>
	public override readonly string ToString() => ToString(null, null);

	/// <summary>
	/// Gets the string representation of the current vector, using a specified format for the vector's components, and using this thread's current culture.
	/// </summary>
	/// <param name="format">A numeric format string.</param>
	/// <returns>The string representation.</returns>
	public readonly string ToString(string? format) => ToString(format, null);

	/// <summary>
	/// Gets the string representation of the current vector, using a specified culture.
	/// </summary>
	/// <param name="formatProvider">An object that provides formatting info for the string representation, or <see langword="null"/> to use the the formatting info associated with the culture used by the current thread.</param>
	/// <returns>The string representation.</returns>
	public readonly string ToString(IFormatProvider? formatProvider) => ToString(null, formatProvider);

	/// <summary>
	/// Gets the string representation of the current vector, using a specified format for the vector's components, and using a specified culture.
	/// </summary>
	/// <param name="format">A numeric format string.</param>
	/// <param name="formatProvider">An object that provides formatting info for the string representation, or <see langword="null"/> to use the the formatting info associated with the culture used by the current thread.</param>
	/// <returns>The string representation.</returns>
	public readonly string ToString(string? format, IFormatProvider? formatProvider)
	{
		DefaultInterpolatedStringHandler handler = new(4, 2, formatProvider);
		handler.AppendFormatted(this, format);
		return handler.ToStringAndClear();
	}

	/// <inheritdoc/>
	public readonly bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
	{
		return VectorUtil.TryFormat(destination, out charsWritten, [X, Y], format, provider);
	}

	/// <summary>
	/// Parses a two-dimensional vector from a string.
	/// </summary>
	/// <param name="s">The string to parse.</param>
	/// <param name="provider">An object that provides formatting info for the string representation, or <see langword="null"/> to use the the formatting info associated with the culture used by the current thread.</param>
	/// <returns>The parsed vector.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="s"/> is <see langword="null"/>.</exception>
	/// <exception cref="FormatException"><paramref name="s"/> is not in the correct format, or one of the components of the vector is not representable by <typeparamref name="T"/>.</exception>
	public static Vector<T> Parse(string s, IFormatProvider? provider = null)
	{
		ArgumentNullException.ThrowIfNull(s);
		return Parse(s.AsSpan(), provider);
	}

	/// <summary>
	/// Parses a two-dimensional vector from a sequence of characters.
	/// </summary>
	/// <param name="s">The sequence of characters to parse.</param>
	/// <param name="provider">An object that provides formatting info for the string representation, or <see langword="null"/> to use the the formatting info associated with the culture used by the current thread.</param>
	/// <returns>The parsed vector.</returns>
	/// <exception cref="FormatException"><paramref name="s"/> is not in the correct format, or one of the components of the vector is not representable by <typeparamref name="T"/>.</exception>
	public static Vector<T> Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null)
	{
		if (!TryParse(s, provider, out Vector<T> result))
		{
			throw new FormatException();
		}
		return result;
	}

	/// <summary>
	/// Tries to parse a two-dimensional vector from a string.
	/// </summary>
	/// <param name="s">The string to parse.</param>
	/// <param name="provider">An object that provides formatting info for the string representation, or <see langword="null"/> to use the the formatting info associated with the culture used by the current thread.</param>
	/// <param name="result">The parsed vector, if it was successfully parsed; otherwise, an undefined value.</param>
	/// <returns><see langword="true"/> if the vector was successfully parsed; otherwise, <see langword="false"/>.</returns>
	public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out Vector<T> result)
	{
		return TryParse(s.AsSpan(), provider, out result);
	}

	/// <summary>
	/// Tries to parse a two-dimensional vector from a sequence of characters.
	/// </summary>
	/// <param name="s">The sequence of characters to parse.</param>
	/// <param name="provider">An object that provides formatting info for the string representation, or <see langword="null"/> to use the the formatting info associated with the culture used by the current thread.</param>
	/// <param name="result">The parsed vector, if it was successfully parsed; otherwise, an undefined value.</param>
	/// <returns><see langword="true"/> if the vector was successfully parsed; otherwise, <see langword="false"/>.</returns>
	public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Vector<T> result)
	{
		VectorFormatInfo vectorFormatInfo = VectorFormatInfo.GetInstance(provider);

		s = VectorUtil.RemoveOpeningAndClosing(s, vectorFormatInfo);

		ReadOnlySpan<char> separator = vectorFormatInfo.Separator;

		result = default;

		int separatorIndex = s.IndexOf(separator);
		if (separatorIndex == -1) return false;

		ReadOnlySpan<char> xSpan = s[..separatorIndex];
		ReadOnlySpan<char> ySpan = s[(separatorIndex + 1)..];

		if (ySpan.IndexOf(separator) != -1) return false;

		return T.TryParse(xSpan, provider, out result.X!)
			&& T.TryParse(ySpan, provider, out result.Y!);
	}

	#endregion
}
