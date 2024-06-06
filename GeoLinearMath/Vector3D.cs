using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;

namespace GeoLinearMath;

/// <summary>
/// Represents a two-dimentional vector, with x, y, and z components.
/// </summary>
/// <typeparam name="T">The type of number that this is a vector of.</typeparam>
/// <seealso cref="Vector{T}"/>
public struct Vector3D<T> : IVector<Vector3D<T>, T>
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
	/// Represents the z component of this vector.
	/// </summary>
	public T Z;

	/// <summary>
	/// Constructs a three-dimensional vector with the same value for all three components.
	/// </summary>
	/// <param name="value">The value for all three components of the vector.</param>
	public Vector3D(T value)
	{
		X = value;
		Y = value;
		Z = value;
	}

	/// <summary>
	/// Constructs a three-dimensional vector with specified x, y and z components.
	/// </summary>
	/// <param name="x">The x component of the vector.</param>
	/// <param name="y">The y component of the vector.</param>
	/// <param name="z">The z component of the vector.</param>
	public Vector3D(T x, T y, T z)
	{
		X = x;
		Y = y;
		Z = z;
	}

	/// <summary>
	/// Constructs a three-dimensional vector by augmenting a two-dimensional vector.
	/// </summary>
	/// <param name="xy">The vector to augment.</param>
	/// <param name="z">The z component of the vector.</param>
	public Vector3D(Vector<T> xy, T z)
	{
		X = xy.X;
		Y = xy.Y;
		Z = z;
	}

	readonly IEnumerable<T> IVector<Vector3D<T>, T>.Components => [X, Y, Z];

	/// <inheritdoc/>
	public readonly T SquareMagnitude => X * X + Y * Y + Z * Z;

	/// <inheritdoc/>
	public readonly T TaxicabMagnitude => T.Abs(X) + T.Abs(Y) + T.Abs(Z);

	static int IVector<Vector3D<T>, T>.Dimension => 3;

	static Vector3D<T> IAdditiveIdentity<Vector3D<T>, Vector3D<T>>.AdditiveIdentity => new Vector3D<T>(T.AdditiveIdentity, T.AdditiveIdentity, T.AdditiveIdentity);
	static T IMultiplicativeIdentity<Vector3D<T>, T>.MultiplicativeIdentity => T.MultiplicativeIdentity;

	/// <inheritdoc/>
	public static Vector3D<T> Origin => new Vector3D<T>(T.Zero, T.Zero, T.Zero);

	/// <summary>
	/// Gets the unit vector (1, 0, 0).
	/// </summary>
	public static Vector3D<T> ToPositiveX => new Vector3D<T>(T.One, T.Zero, T.Zero);

	/// <summary>
	/// Gets the unit vector (0, 1, 0).
	/// </summary>
	public static Vector3D<T> ToPositiveY => new Vector3D<T>(T.Zero, T.One, T.Zero);

	/// <summary>
	/// Gets the unit vector (0, 0, 1).
	/// </summary>
	public static Vector3D<T> ToPositiveZ => new Vector3D<T>(T.Zero, T.Zero, T.One);

	/// <summary>
	/// Gets the unit vector (-1, 0, 0).
	/// </summary>
	public static Vector3D<T> ToNegativeX => new Vector3D<T>(-T.One, T.Zero, T.Zero);

	/// <summary>
	/// Gets the unit vector (0, -1, 0).
	/// </summary>
	public static Vector3D<T> ToNegativeY => new Vector3D<T>(T.Zero, -T.One, T.Zero);

	/// <summary>
	/// Gets the unit vector (0, 0, -1).
	/// </summary>
	public static Vector3D<T> ToNegativeZ => new Vector3D<T>(T.Zero, T.Zero, -T.One);

	/// <summary>
	/// Gets the six unit vectors that are along an axis.
	/// </summary>
	public static Vector3D<T>[] AxisUnitVectors =>
	[
		ToPositiveX,
		ToPositiveY,
		ToPositiveZ,
		ToNegativeX,
		ToNegativeY,
		ToNegativeZ
	];

	static IEnumerable<Vector3D<T>> IVector<Vector3D<T>, T>.AxisUnitVectors => AxisUnitVectors;

	/// <summary>
	/// Creates a three-dimensional vector with a specified span containing the three components.
	/// </summary>
	/// <param name="components">A span containing three elements, which are the components of the vector.</param>
	/// <returns>The created vector.</returns>
	/// <exception cref="ArgumentException"><paramref name="components"/> does not contain three elements.</exception>
	public static Vector3D<T> Create(ReadOnlySpan<T> components)
	{
		if (components.Length != 3)
		{
			throw new ArgumentException($"Expected 3 components for a three-dimensional vector, got {components.Length}.", nameof(components));
		}
		Vector3D<T> result;
		result.X = components[0];
		result.Y = components[1];
		result.Z = components[2];
		return result;
	}

	/// <inheritdoc cref="Vector{T}.CreateChecked{TOther}(Vector{TOther})"/>
	public static Vector3D<T> CreateChecked<TOther>(Vector3D<TOther> other)
		where TOther : INumber<TOther>
	{
		return new Vector3D<T>(T.CreateChecked(other.X), T.CreateChecked(other.Y), T.CreateChecked(other.Z));
	}

	/// <inheritdoc cref="Vector{T}.CreateSaturating{TOther}(Vector{TOther})"/>
	public static Vector3D<T> CreateSaturating<TOther>(Vector3D<TOther> other)
		where TOther : INumber<TOther>
	{
		return new Vector3D<T>(T.CreateSaturating(other.X), T.CreateSaturating(other.Y), T.CreateSaturating(other.Z));
	}

	/// <inheritdoc cref="Vector{T}.CreateTruncating{TOther}(Vector{TOther})"/>
	public static Vector3D<T> CreateTruncating<TOther>(Vector3D<TOther> other)
		where TOther : INumber<TOther>
	{
		return new Vector3D<T>(T.CreateTruncating(other.X), T.CreateTruncating(other.Y), T.CreateTruncating(other.Z));
	}

	/// <inheritdoc/>
	public readonly bool IsInBox(Vector3D<T> min, Vector3D<T> max)
	{
		return X >= min.X && Y >= min.Y && Z >= min.X
			&& X <= max.X && Y <= max.Y && Z >= min.Z;
	}

	#region Operations

	/// <inheritdoc/>
	public static Vector3D<T> Clamp(Vector3D<T> value, Vector3D<T> min, Vector3D<T> max)
	{
		return new Vector3D<T>(T.Clamp(value.X, min.X, max.X), T.Clamp(value.Y, min.Y, max.Y), T.Clamp(value.Z, min.Z, max.Z));
	}

	/// <inheritdoc/>
	public static Vector3D<T> operator +(Vector3D<T> value) => new Vector3D<T>(+value.X, +value.Y, +value.Z);

	/// <inheritdoc/>
	public static Vector3D<T> operator -(Vector3D<T> value) => new Vector3D<T>(-value.X, -value.Y, -value.Z);

	/// <inheritdoc/>
	public static Vector3D<T> operator +(Vector3D<T> left, Vector3D<T> right)
	{
		return new Vector3D<T>(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
	}

	/// <inheritdoc/>
	public static Vector3D<T> operator -(Vector3D<T> left, Vector3D<T> right)
	{
		return new Vector3D<T>(left.X - right.X, left.Y - right.Y, left.Z + right.Z);
	}

	/// <inheritdoc cref="Vector{T}.operator *(Vector{T}, T)"/>
	public static Vector3D<T> operator *(Vector3D<T> left, T right)
	{
		return new Vector3D<T>(left.X * right, left.Y * right, left.Z * right);
	}

	/// <inheritdoc cref="Vector{T}.operator *(T, Vector{T})"/>
	public static Vector3D<T> operator *(T left, Vector3D<T> right)
	{
		return new Vector3D<T>(left * right.X, left * right.Y, left * right.Z);
	}

	/// <inheritdoc cref="Vector{T}.operator /(Vector{T}, T)"/>
	public static Vector3D<T> operator /(Vector3D<T> left, T right)
	{
		return new Vector3D<T>(left.X / right, left.Y / right, left.Z / right);
	}

	/// <inheritdoc cref="Vector{T}.operator /(T, Vector{T})"/>
	public static Vector3D<T> operator /(T left, Vector3D<T> right)
	{
		return left * right / right.SquareMagnitude;
	}

	/// <inheritdoc cref="Vector{T}.Reciprocal(Vector{T})"/>
	public static Vector3D<T> Reciprocal(Vector3D<T> value)
	{
		return value / value.SquareMagnitude;
	}

	/// <inheritdoc/>
	public static T Dot(Vector3D<T> left, Vector3D<T> right)
	{
		return left.X * right.X + left.Y * right.Y + left.Z * right.Z;
	}

	#endregion

	#region Equality

	/// <inheritdoc cref="Vector{T}.GetHashCode()"/>
	public override readonly int GetHashCode() => HashCode.Combine(X, Y, Z);

	/// <summary>
	/// Determines if an object is a vector of the same type and equal to the current vector.
	/// </summary>
	/// <param name="obj">The object to check.</param>
	/// <returns><see langword="true"/> if <paramref name="obj"/> is a <see cref="Vector3D{T}"/> instance of the same type, and equal to the current vector; otherwise, <see langword="false"/>.</returns>
	public override readonly bool Equals([NotNullWhen(true)] object? obj) => obj is Vector3D<T> vector && Equals(vector);

	/// <summary>
	/// Determines if the current vector is equal to another vector of the same type.
	/// </summary>
	/// <param name="other">A vector to compare with the current vector.</param>
	/// <returns><see langword="true"/> if the current vector is equal to <paramref name="other"/>; otherwise, <see langword="false"/>.</returns>
	public readonly bool Equals(Vector3D<T> other) => this == other;

	/// <inheritdoc/>
	public static bool operator ==(Vector3D<T> left, Vector3D<T> right) => left.X == right.X && left.Y == right.Y && left.Z == right.Z;

	/// <inheritdoc/>
	public static bool operator !=(Vector3D<T> left, Vector3D<T> right) => !(left == right);

	#endregion

	#region String conversions

	/// <inheritdoc cref="Vector{T}.ToString()"/>
	public override readonly string ToString() => ToString(null, null);

	/// <inheritdoc cref="Vector{T}.ToString(string?)"/>
	public readonly string ToString(string? format) => ToString(format, null);
	
	/// <inheritdoc cref="Vector{T}.ToString(IFormatProvider?)"/>
	public readonly string ToString(IFormatProvider? formatProvider) => ToString(null, formatProvider);

	/// <inheritdoc cref="Vector{T}.ToString(string?, IFormatProvider?)"/>
	public readonly string ToString(string? format, IFormatProvider? formatProvider)
	{
		if (!string.IsNullOrEmpty(format)) throw new NotImplementedException();

		return string.Create(formatProvider, $"({X}, {Y}, {Z})");
	}

	/// <inheritdoc/>
	public readonly bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
	{
		return VectorUtil.TryFormat(destination, out charsWritten, [X, Y, Z], format, provider);
	}

	/// <summary>
	/// Parses a three-dimensional vector from a string.
	/// </summary>
	/// <param name="s">The string to parse.</param>
	/// <param name="provider">An object that provides formatting info for the string representation, or <see langword="null"/> to use the the formatting info associated with the culture used by the current thread.</param>
	/// <returns>The parsed vector.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="s"/> is <see langword="null"/>.</exception>
	/// <exception cref="FormatException"><paramref name="s"/> is not in the correct format, or one of the components of the vector is not representable by <typeparamref name="T"/>.</exception>
	public static Vector3D<T> Parse(string s, IFormatProvider? provider = null)
	{
		ArgumentNullException.ThrowIfNull(s);
		return Parse(s.AsSpan(), provider);
	}

	/// <summary>
	/// Parses a three-dimensional vector from a sequence of characters.
	/// </summary>
	/// <param name="s">The sequence of characters to parse.</param>
	/// <param name="provider">An object that provides formatting info for the string representation, or <see langword="null"/> to use the the formatting info associated with the culture used by the current thread.</param>
	/// <returns>The parsed vector.</returns>
	/// <exception cref="FormatException"><paramref name="s"/> is not in the correct format, or one of the components of the vector is not representable by <typeparamref name="T"/>.</exception>
	public static Vector3D<T> Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null)
	{
		if (!TryParse(s, provider, out Vector3D<T> result))
		{
			throw new FormatException();
		}
		return result;
	}

	/// <summary>
	/// Tries to parse a three-dimensional vector from a string.
	/// </summary>
	/// <param name="s">The string to parse.</param>
	/// <param name="provider">An object that provides formatting info for the string representation, or <see langword="null"/> to use the the formatting info associated with the culture used by the current thread.</param>
	/// <param name="result">The parsed vector, if it was successfully parsed; otherwise, an undefined value.</param>
	/// <returns><see langword="true"/> if the vector was successfully parsed; otherwise, <see langword="false"/>.</returns>
	public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out Vector3D<T> result)
	{
		return TryParse(s.AsSpan(), provider, out result);
	}

	/// <summary>
	/// Tries to parse a three-dimensional vector from a sequence of characters.
	/// </summary>
	/// <param name="s">The sequence of characters to parse.</param>
	/// <param name="provider">An object that provides formatting info for the string representation, or <see langword="null"/> to use the the formatting info associated with the culture used by the current thread.</param>
	/// <param name="result">The parsed vector, if it was successfully parsed; otherwise, an undefined value.</param>
	/// <returns><see langword="true"/> if the vector was successfully parsed; otherwise, <see langword="false"/>.</returns>
	public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Vector3D<T> result)
	{
		VectorFormatInfo vectorFormatInfo = VectorFormatInfo.GetInstance(provider);

		s = VectorUtil.RemoveOpeningAndClosing(s, vectorFormatInfo);

		ReadOnlySpan<char> separator = vectorFormatInfo.Separator;

		result = default;

		int separator1Index = s.IndexOf(separator);
		if (separator1Index == -1) return false;

		ReadOnlySpan<char> xSpan = s[..separator1Index];

		s = s[(separator1Index + 1)..];
		int separator2Index = s.IndexOf(separator);
		if (separator2Index == -1) return false;

		ReadOnlySpan<char> ySpan = s[..separator2Index];
		ReadOnlySpan<char> zSpan = s[(separator2Index + 1)..];

		if (zSpan.IndexOf(separator) != -1) return false;

		return T.TryParse(xSpan, provider, out result.X!)
			&& T.TryParse(ySpan, provider, out result.Y!)
			&& T.TryParse(zSpan, provider, out result.Z!);
	}

	#endregion
}
