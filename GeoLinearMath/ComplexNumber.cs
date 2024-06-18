using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace GeoLinearMath;

/// <summary>
/// Represents any complex number, expressed in the form <c>a + bi</c>.
/// </summary>
/// <typeparam name="T"></typeparam>
public struct ComplexNumber<T> : INumberBase<ComplexNumber<T>>,
	IQuantity<T>,
	IAdditionOperators<ComplexNumber<T>, T, ComplexNumber<T>>,
	ISubtractionOperators<ComplexNumber<T>, T, ComplexNumber<T>>,
	IMultiplyOperators<ComplexNumber<T>, T, ComplexNumber<T>>,
	IMultiplyOperators<ComplexNumber<T>, Vector<T>, Vector<T>>,
	IDivisionOperators<ComplexNumber<T>, T, ComplexNumber<T>>,
	IDivisionOperators<ComplexNumber<T>, Vector<T>, Vector<T>>,
	IMultiplicativeInverse<ComplexNumber<T>, ComplexNumber<T>>
	where T : INumber<T>
{
	/// <summary>
	/// Represents the real part of this number.
	/// </summary>
	public T Real;

	/// <summary>
	/// Represents the imaginary part of this number.
	/// </summary>
	public T Imaginary;

	/// <summary>
	/// Constructs a complex number with specified real and imaginary parts.
	/// </summary>
	/// <param name="real">The real part of the number.</param>
	/// <param name="imaginary">The complex part of the number.</param>
	public ComplexNumber(T real, T imaginary)
	{
		Real = real;
		Imaginary = imaginary;
	}

	/// <inheritdoc/>
	public readonly T SquareMagnitude => Real * Real + Imaginary * Imaginary;

	/// <inheritdoc/>
	public readonly T SquareMagnitudeChecked => checked(Real * Real + Imaginary * Imaginary);

	/// <inheritdoc/>
	public readonly T Magnitude => MathUtil.Sqrt(SquareMagnitude);

	/// <inheritdoc/>
	public readonly T MagnitudeChecked => MathUtil.SqrtChecked(SquareMagnitudeChecked);

	/// <summary>
	/// Gets the value representing zero.
	/// </summary>
	public static ComplexNumber<T> Zero => T.Zero;

	/// <summary>
	/// Gets the value representing one.
	/// </summary>
	public static ComplexNumber<T> One => T.One;

	static int INumberBase<ComplexNumber<T>>.Radix => T.Radix;

	static ComplexNumber<T> IAdditiveIdentity<ComplexNumber<T>, ComplexNumber<T>>.AdditiveIdentity
		=> new(T.AdditiveIdentity, T.AdditiveIdentity);

	static ComplexNumber<T> IMultiplicativeIdentity<ComplexNumber<T>, ComplexNumber<T>>.MultiplicativeIdentity
		=> T.MultiplicativeIdentity;

	/// <inheritdoc/>
	public static bool IsCanonical(ComplexNumber<T> value) => T.IsCanonical(value.Real) && T.IsCanonical(value.Imaginary);

	/// <inheritdoc/>
	public static bool IsImaginaryNumber(ComplexNumber<T> value) => T.IsZero(value.Real) && T.IsRealNumber(value.Imaginary);

	/// <inheritdoc/>
	public static bool IsComplexNumber(ComplexNumber<T> value) => !T.IsZero(value.Real) && !T.IsZero(value.Imaginary);

	/// <inheritdoc/>
	public static bool IsRealNumber(ComplexNumber<T> value) => T.IsRealNumber(value.Real) && T.IsZero(value.Imaginary);

	/// <inheritdoc/>
	public static bool IsPositive(ComplexNumber<T> value) => T.IsPositive(value.Real) && T.IsZero(value.Imaginary);

	/// <inheritdoc/>
	public static bool IsNegative(ComplexNumber<T> value) => T.IsNegative(value.Real) && T.IsZero(value.Imaginary);

	/// <inheritdoc/>
	public static bool IsZero(ComplexNumber<T> value) => T.IsZero(value.Real) && T.IsZero(value.Imaginary);

	/// <inheritdoc/>
	public static bool IsInteger(ComplexNumber<T> value) => T.IsInteger(value.Real) && T.IsZero(value.Imaginary);

	/// <inheritdoc/>
	public static bool IsEvenInteger(ComplexNumber<T> value) => T.IsEvenInteger(value.Real) && T.IsZero(value.Imaginary);

	/// <inheritdoc/>
	public static bool IsOddInteger(ComplexNumber<T> value) => T.IsOddInteger(value.Real) && T.IsZero(value.Imaginary);

	/// <inheritdoc/>
	public static bool IsNormal(ComplexNumber<T> value) => T.IsNormal(value.Real) && T.IsNormal(value.Imaginary);

	/// <inheritdoc/>
	public static bool IsSubnormal(ComplexNumber<T> value) => T.IsSubnormal(value.Real) || T.IsSubnormal(value.Imaginary);

	/// <inheritdoc/>
	public static bool IsNaN(ComplexNumber<T> value) => T.IsNaN(value.Real) || T.IsNaN(value.Imaginary);

	/// <inheritdoc/>
	public static bool IsFinite(ComplexNumber<T> value) => T.IsFinite(value.Real) && T.IsFinite(value.Imaginary);

	/// <inheritdoc/>
	public static bool IsInfinity(ComplexNumber<T> value) => T.IsInfinity(value.Real) || T.IsInfinity(value.Imaginary);

	/// <inheritdoc/>
	public static bool IsNegativeInfinity(ComplexNumber<T> value) => T.IsNegativeInfinity(value.Real) && T.IsZero(value.Imaginary);

	/// <inheritdoc/>
	public static bool IsPositiveInfinity(ComplexNumber<T> value) => T.IsPositiveInfinity(value.Real) && T.IsZero(value.Imaginary);

	#region Operations

	// This is an explicit interface implementation because, unlike all other methods in this struct, it is by default checked, not unchecked.
	// To avoid confusion with the names, there are instead the properties Magnitude and MagnitudeChecked.
	static ComplexNumber<T> INumberBase<ComplexNumber<T>>.Abs(ComplexNumber<T> value) => value.MagnitudeChecked;

	/// <inheritdoc/>
	public static ComplexNumber<T> operator ++(ComplexNumber<T> value)
	{
		value.Real++;
		return value;
	}

	/// <inheritdoc/>
	public static ComplexNumber<T> operator checked ++(ComplexNumber<T> value)
	{
		checked
		{
			value.Real++;
			return value;
		}
	}

	/// <inheritdoc/>
	public static ComplexNumber<T> operator --(ComplexNumber<T> value)
	{
		value.Real--;
		return value;
	}

	/// <inheritdoc/>
	public static ComplexNumber<T> operator checked --(ComplexNumber<T> value)
	{
		checked
		{
			value.Real--;
			return value;
		}
	}

	/// <inheritdoc/>
	public static ComplexNumber<T> operator +(ComplexNumber<T> value) => new(+value.Real, +value.Imaginary);

	/// <inheritdoc/>
	public static ComplexNumber<T> operator -(ComplexNumber<T> value) => new(-value.Real, -value.Imaginary);

	/// <inheritdoc/>
	public static ComplexNumber<T> operator checked -(ComplexNumber<T> value) => checked(new(-value.Real, -value.Imaginary));

	/// <summary>
	/// Calculates the complex conjugate of a value, in an unchecked context.
	/// </summary>
	/// <param name="value">The value for which to calculate the complex conjugate.</param>
	/// <returns>The complex conjugate of <paramref name="value"/>.</returns>
	public static ComplexNumber<T> Conjugate(ComplexNumber<T> value) => new(+value.Real, -value.Imaginary);

	/// <summary>
	/// Calculates the complex conjugate of a value, in a checked context.
	/// </summary>
	/// <param name="value">The value for which to calculate the complex conjugate.</param>
	/// <returns>The complex conjugate of <paramref name="value"/>.</returns>
	public static ComplexNumber<T> ConjugateChecked(ComplexNumber<T> value) => checked(new(+value.Real, -value.Imaginary));

	/// <inheritdoc/>
	public static ComplexNumber<T> operator +(ComplexNumber<T> left, ComplexNumber<T> right)
	{
		return new(left.Real + right.Real, left.Imaginary + right.Imaginary);
	}

	/// <inheritdoc/>
	public static ComplexNumber<T> operator checked +(ComplexNumber<T> left, ComplexNumber<T> right)
	{
		checked
		{
			return new(left.Real + right.Real, left.Imaginary + right.Imaginary);
		}
	}

	/// <inheritdoc/>
	public static ComplexNumber<T> operator +(ComplexNumber<T> left, T right)
	{
		return new(left.Real + right, left.Imaginary);
	}

	/// <inheritdoc/>
	public static ComplexNumber<T> operator checked +(ComplexNumber<T> left, T right)
	{
		checked
		{
			return new(left.Real + right, left.Imaginary);
		}
	}

	/// <inheritdoc cref="IAdditionOperators{TSelf, TOther, TResult}.operator +"/>
	public static ComplexNumber<T> operator +(T left, ComplexNumber<T> right)
	{
		return new(left + right.Real, right.Imaginary);
	}

	/// <inheritdoc cref="IAdditionOperators{TSelf, TOther, TResult}.operator checked +"/>
	public static ComplexNumber<T> operator checked +(T left, ComplexNumber<T> right)
	{
		checked
		{
			return new(left + right.Real, right.Imaginary);
		}
	}

	/// <inheritdoc/>
	public static ComplexNumber<T> operator -(ComplexNumber<T> left, ComplexNumber<T> right)
	{
		return new(left.Real - right.Real, left.Imaginary - right.Imaginary);
	}

	/// <inheritdoc/>
	public static ComplexNumber<T> operator checked -(ComplexNumber<T> left, ComplexNumber<T> right)
	{
		checked
		{
			return new(left.Real - right.Real, left.Imaginary - right.Imaginary);
		}
	}

	/// <inheritdoc/>
	public static ComplexNumber<T> operator -(ComplexNumber<T> left, T right)
	{
		return new(left.Real - right, left.Imaginary);
	}

	/// <inheritdoc/>
	public static ComplexNumber<T> operator checked -(ComplexNumber<T> left, T right)
	{
		checked
		{
			return new(left.Real - right, left.Imaginary);
		}
	}

	/// <inheritdoc cref="ISubtractionOperators{TSelf, TOther, TResult}.operator -"/>
	public static ComplexNumber<T> operator -(T left, ComplexNumber<T> right)
	{
		return new(left - right.Real, right.Imaginary);
	}

	/// <inheritdoc cref="ISubtractionOperators{TSelf, TOther, TResult}.operator checked -"/>
	public static ComplexNumber<T> operator checked -(T left, ComplexNumber<T> right)
	{
		checked
		{
			return new(left - right.Real, right.Imaginary);
		}
	}

	/// <inheritdoc/>
	public static ComplexNumber<T> operator *(ComplexNumber<T> left, ComplexNumber<T> right)
	{
		ComplexNumber<T> result;
		result.Real = left.Real * right.Real - left.Imaginary * right.Imaginary;
		result.Imaginary = left.Real * right.Imaginary + left.Imaginary * right.Real;
		return result;
	}

	/// <inheritdoc/>
	public static ComplexNumber<T> operator checked *(ComplexNumber<T> left, ComplexNumber<T> right)
	{
		checked
		{
			ComplexNumber<T> result;
			result.Real = left.Real * right.Real - left.Imaginary * right.Imaginary;
			result.Imaginary = left.Real * right.Imaginary + left.Imaginary * right.Real;
			return result;
		}
	}

	/// <inheritdoc/>
	public static ComplexNumber<T> operator *(ComplexNumber<T> left, T right)
	{
		ComplexNumber<T> result;
		result.Real = left.Real * right;
		result.Imaginary = left.Imaginary * right;
		return result;
	}

	/// <inheritdoc/>
	public static ComplexNumber<T> operator checked *(ComplexNumber<T> left, T right)
	{
		checked
		{
			ComplexNumber<T> result;
			result.Real = left.Real * right;
			result.Imaginary = left.Imaginary * right;
			return result;
		}
	}

	/// <inheritdoc cref="IMultiplyOperators{TSelf, TOther, TResult}.operator *"/>
	public static ComplexNumber<T> operator *(T left, ComplexNumber<T> right)
	{
		ComplexNumber<T> result;
		result.Real = left * right.Real;
		result.Imaginary = left * right.Imaginary;
		return result;
	}

	/// <inheritdoc cref="IMultiplyOperators{TSelf, TOther, TResult}.operator checked *"/>
	public static ComplexNumber<T> operator checked *(T left, ComplexNumber<T> right)
	{
		checked
		{
			ComplexNumber<T> result;
			result.Real = left * right.Real;
			result.Imaginary = left * right.Imaginary;
			return result;
		}
	}

	/// <summary>
	/// Multiplies a complex number by a vector in VGA in an unchecked context.
	/// </summary>
	/// <param name="left">The complex number to multiply by <paramref name="right"/>.</param>
	/// <param name="right">The vector to multiply by <paramref name="left"/>.</param>
	/// <returns>The product of <paramref name="left"/> and <paramref name="right"/>.</returns>
	public static Vector<T> operator *(ComplexNumber<T> left, Vector<T> right)
	{
		Vector<T> result;
		result.X = left.Real * right.X + left.Imaginary * right.Y;
		result.Y = left.Real * right.Y - left.Imaginary * right.X;
		return result;
	}

	/// <summary>
	/// Multiplies a complex number by a vector in VGA in a checked context.
	/// </summary>
	/// <param name="left">The complex number to multiply by <paramref name="right"/>.</param>
	/// <param name="right">The vector to multiply by <paramref name="left"/>.</param>
	/// <returns>The product of <paramref name="left"/> and <paramref name="right"/>.</returns>
	/// <exception cref="OverflowException">The result is not representable by <typeparamref name="T"/>.</exception>
	public static Vector<T> operator checked *(ComplexNumber<T> left, Vector<T> right)
	{
		checked
		{
			Vector<T> result;
			result.X = left.Real * right.X + left.Imaginary * right.Y;
			result.Y = left.Real * right.Y - left.Imaginary * right.X;
			return result;
		}
	}

	// Converse operations of the above are implemented in Vector<T> (so that it can implement the IMultiplyOperators interface).

	/// <inheritdoc/>
	public static ComplexNumber<T> operator /(ComplexNumber<T> left, ComplexNumber<T> right)
	{
		T denominator = right.SquareMagnitude;
		ComplexNumber<T> result;
		result.Real = (left.Real * right.Real + left.Imaginary * right.Imaginary) / denominator;
		result.Imaginary = (left.Imaginary * right.Real - left.Real * right.Imaginary) / denominator;
		return result;
	}

	/// <inheritdoc/>
	public static ComplexNumber<T> operator checked /(ComplexNumber<T> left, ComplexNumber<T> right)
	{
		checked
		{
			T denominator = right.SquareMagnitudeChecked;
			ComplexNumber<T> result;
			result.Real = (left.Real * right.Real + left.Imaginary * right.Imaginary) / denominator;
			result.Imaginary = (left.Imaginary * right.Real - left.Real * right.Imaginary) / denominator;
			return result;
		}
	}

	/// <inheritdoc/>
	public static ComplexNumber<T> operator /(ComplexNumber<T> left, T right)
	{
		ComplexNumber<T> result;
		result.Real = left.Real / right;
		result.Imaginary = left.Imaginary / right;
		return result;
	}

	/// <inheritdoc/>
	public static ComplexNumber<T> operator checked /(ComplexNumber<T> left, T right)
	{
		checked
		{
			ComplexNumber<T> result;
			result.Real = left.Real / right;
			result.Imaginary = left.Imaginary / right;
			return result;
		}
	}

	/// <inheritdoc cref="IDivisionOperators{TSelf, TOther, TResult}.operator /"/>
	public static ComplexNumber<T> operator /(T left, ComplexNumber<T> right)
	{
		T denominator = right.SquareMagnitude;
		ComplexNumber<T> result;
		result.Real = left * right.Real / denominator;
		result.Imaginary = -(left * right.Imaginary / denominator);
		return result;
	}

	/// <inheritdoc cref="IDivisionOperators{TSelf, TOther, TResult}.operator checked /"/>
	public static ComplexNumber<T> operator checked /(T left, ComplexNumber<T> right)
	{
		checked
		{
			T denominator = right.SquareMagnitude;
			ComplexNumber<T> result;
			result.Real = left * right.Real / denominator;
			result.Imaginary = -(left * right.Imaginary / denominator);
			return result;
		}
	}

	/// <summary>
	/// Divides a complex number by a vector in VGA in an unchecked context.
	/// </summary>
	/// <param name="left">The complex number to divide by <paramref name="right"/>.</param>
	/// <param name="right">The vector to divide by <paramref name="left"/>.</param>
	/// <returns>The quotient of <paramref name="left"/> divided by <paramref name="right"/>.</returns>
	public static Vector<T> operator /(ComplexNumber<T> left, Vector<T> right)
	{
		return left * right / right.SquareMagnitude;
	}

	/// <summary>
	/// Divides a complex number by a vector in VGA in a checked context.
	/// </summary>
	/// <param name="left">The complex number to divide by <paramref name="right"/>.</param>
	/// <param name="right">The vector to divide by <paramref name="left"/>.</param>
	/// <returns>The quotient of <paramref name="left"/> divided by <paramref name="right"/>.</returns>
	public static Vector<T> operator checked /(ComplexNumber<T> left, Vector<T> right)
	{
		checked
		{
			return left * right / right.SquareMagnitudeChecked;
		}
	}

	/// <inheritdoc/>
	public static ComplexNumber<T> Reciprocal(ComplexNumber<T> value)
	{
		T denominator = value.SquareMagnitude;
		ComplexNumber<T> result;
		result.Real = value.Real / denominator;
		result.Imaginary = -(value.Imaginary / denominator);
		return result;
	}

	/// <inheritdoc/>
	public static ComplexNumber<T> ReciprocalChecked(ComplexNumber<T> value)
	{
		checked
		{
			T denominator = value.SquareMagnitudeChecked;
			ComplexNumber<T> result;
			result.Real = value.Real / denominator;
			result.Imaginary = -(value.Imaginary / denominator);
			return result;
		}
	}

	public static ComplexNumber<T> MaxMagnitude(ComplexNumber<T> x, ComplexNumber<T> y)
	{
		throw new NotImplementedException();
	}

	public static ComplexNumber<T> MaxMagnitudeNumber(ComplexNumber<T> x, ComplexNumber<T> y)
	{
		throw new NotImplementedException();
	}

	public static ComplexNumber<T> MinMagnitude(ComplexNumber<T> x, ComplexNumber<T> y)
	{
		throw new NotImplementedException();
	}

	public static ComplexNumber<T> MinMagnitudeNumber(ComplexNumber<T> x, ComplexNumber<T> y)
	{
		throw new NotImplementedException();
	}

	/// <summary>
	/// Gets a <see cref="Vector{T}"/> instance with the same components (or: multiplies the vector (1, 0) by this complex number in VGA).
	/// </summary>
	/// <returns>A 2D vector whose x-component is <see cref="Real"/> and whose y-component is <see cref="Imaginary"/>.</returns>
	public readonly Vector<T> ToVector()
	{
		return new Vector<T>(Real, Imaginary);
	}

	#endregion

	#region Equality

	public override int GetHashCode() => HashCode.Combine(Real, Imaginary);

	public override bool Equals([NotNullWhen(true)] object? obj) => obj is ComplexNumber<T> complexNumber && Equals(complexNumber);

	public bool Equals(ComplexNumber<T> other) => this == other;

	public static bool operator ==(ComplexNumber<T> left, ComplexNumber<T> right) => left.Real == right.Real && left.Imaginary == right.Imaginary;

	public static bool operator !=(ComplexNumber<T> left, ComplexNumber<T> right) => !(left == right);

	#endregion

	#region Number conversions

	public static implicit operator ComplexNumber<T>(T real)
	{
		ComplexNumber<T> result;
		result.Real = real;
		result.Imaginary = T.Zero;
		return result;
	}

	public static explicit operator T(ComplexNumber<T> value)
	{
		return value.Real;
	}

	public static explicit operator checked T(ComplexNumber<T> value)
	{
		ThrowIfNotReal(value);
		return value.Real;
	}

	private static void ThrowIfNotReal(ComplexNumber<T> value)
	{
		if (!T.IsZero(value.Imaginary))
		{
			throw new InvalidCastException($"{value} has an imaginary part.");
		}
	}

	static bool INumberBase<ComplexNumber<T>>.TryConvertFromChecked<TOther>(TOther value, out ComplexNumber<T> result)
	{
		if (typeof(TOther) == typeof(T))
		{
			result = (T)(object)value;
			return true;
		}
		if (typeof(TOther) == typeof(ComplexNumber<T>))
		{
			result = (ComplexNumber<T>)(object)value;
			return true;
		}
		if (typeof(TOther) == typeof(Complex))
		{
			Complex complexValue = (Complex)(object)value;
			if (TryCreateTFromChecked(complexValue.Real, out result.Real!) && TryCreateTFromChecked(complexValue.Imaginary, out result.Imaginary!))
			{
				return true;
			}
		}
		if (TryCreateTFromChecked(value, out T? converted))
		{
			result = converted;
			return true;
		}

		result = default;
		return false;
	}

	private static bool TryCreateTFromChecked<TOther>(TOther value, [MaybeNullWhen(false)] out T result) where TOther : INumberBase<TOther>
	{
		return T.TryConvertFromChecked(value, out result) || TOther.TryConvertToChecked(value, out result);
	}

	static bool INumberBase<ComplexNumber<T>>.TryConvertFromSaturating<TOther>(TOther value, out ComplexNumber<T> result)
	{
		if (typeof(TOther) == typeof(T))
		{
			result = (T)(object)value;
			return true;
		}
		if (typeof(TOther) == typeof(ComplexNumber<T>))
		{
			result = (ComplexNumber<T>)(object)value;
			return true;
		}
		if (typeof(TOther) == typeof(Complex))
		{
			Complex complexValue = (Complex)(object)value;
			if (TryCreateTFromSaturating(complexValue.Real, out result.Real!) && TryCreateTFromSaturating(complexValue.Imaginary, out result.Imaginary!))
			{
				return true;
			}
		}
		if (TryCreateTFromSaturating(value, out T? converted))
		{
			result = converted;
			return true;
		}

		result = default;
		return false;
	}

	private static bool TryCreateTFromSaturating<TOther>(TOther value, [MaybeNullWhen(false)] out T result) where TOther : INumberBase<TOther>
	{
		return T.TryConvertFromSaturating(value, out result) || TOther.TryConvertToSaturating(value, out result);
	}

	static bool INumberBase<ComplexNumber<T>>.TryConvertFromTruncating<TOther>(TOther value, out ComplexNumber<T> result)
	{
		if (typeof(TOther) == typeof(T))
		{
			result = (T)(object)value;
			return true;
		}
		if (typeof(TOther) == typeof(ComplexNumber<T>))
		{
			result = (ComplexNumber<T>)(object)value;
			return true;
		}
		if (typeof(TOther) == typeof(Complex))
		{
			Complex complexValue = (Complex)(object)value;
			if (TryCreateTFromTruncating(complexValue.Real, out result.Real!) && TryCreateTFromTruncating(complexValue.Imaginary, out result.Imaginary!))
			{
				return true;
			}
		}
		if (TryCreateTFromTruncating(value, out T? converted))
		{
			result = converted;
			return true;
		}

		result = default;
		return false;
	}

	private static bool TryCreateTFromTruncating<TOther>(TOther value, [MaybeNullWhen(false)] out T result) where TOther : INumberBase<TOther>
	{
		return T.TryConvertFromTruncating(value, out result) || TOther.TryConvertToTruncating(value, out result);
	}

	static bool INumberBase<ComplexNumber<T>>.TryConvertToChecked<TOther>(ComplexNumber<T> value, [MaybeNullWhen(false)] out TOther result)
	{
		if (typeof(TOther) == typeof(T))
		{
			result = (TOther)(object)checked((T)value);
			return true;
		}
		if (typeof(TOther) == typeof(ComplexNumber<T>))
		{
			result = (TOther)(object)value;
			return true;
		}
		if (typeof(TOther) == typeof(Complex))
		{
			if (TryCreateTOtherFromChecked(value.Real, out double real) && TryCreateTOtherFromChecked(value.Imaginary, out double imaginary))
			{
				result = (TOther)(object)new Complex(real, imaginary);
				return true;
			}
		}
		if (TryCreateTOtherFromChecked(value.Real, out result))
		{
			ThrowIfNotReal(value);
			return true;
		}
		return false;
	}

	private static bool TryCreateTOtherFromChecked<TOther>(T value, [MaybeNullWhen(false)] out TOther result) where TOther : INumberBase<TOther>
	{
		return T.TryConvertToChecked(value, out result) || TOther.TryConvertFromChecked(value, out result);
	}

	static bool INumberBase<ComplexNumber<T>>.TryConvertToSaturating<TOther>(ComplexNumber<T> value, [MaybeNullWhen(false)] out TOther result)
	{
		if (typeof(TOther) == typeof(T))
		{
			result = (TOther)(object)(T)value;
			return true;
		}
		if (typeof(TOther) == typeof(ComplexNumber<T>))
		{
			result = (TOther)(object)value;
			return true;
		}
		if (typeof(TOther) == typeof(Complex))
		{
			if (TryCreateTOtherFromSaturating(value.Real, out double real) && TryCreateTOtherFromSaturating(value.Imaginary, out double imaginary))
			{
				result = (TOther)(object)new Complex(real, imaginary);
				return true;
			}
		}
		return TryCreateTOtherFromSaturating(value.Real, out result);
	}

	private static bool TryCreateTOtherFromSaturating<TOther>(T value, [MaybeNullWhen(false)] out TOther result) where TOther : INumberBase<TOther>
	{
		return T.TryConvertToSaturating(value, out result) || TOther.TryConvertFromSaturating(value, out result);
	}

	static bool INumberBase<ComplexNumber<T>>.TryConvertToTruncating<TOther>(ComplexNumber<T> value, [MaybeNullWhen(false)] out TOther result)
	{
		if (typeof(TOther) == typeof(T))
		{
			result = (TOther)(object)(T)value;
			return true;
		}
		if (typeof(TOther) == typeof(ComplexNumber<T>))
		{
			result = (TOther)(object)value;
			return true;
		}
		if (typeof(TOther) == typeof(Complex))
		{
			if (TryCreateTOtherFromTruncating(value.Real, out double real) && TryCreateTOtherFromTruncating(value.Imaginary, out double imaginary))
			{
				result = (TOther)(object)new Complex(real, imaginary);
				return true;
			}
		}
		return TryCreateTOtherFromTruncating(value.Real, out result);
	}

	private static bool TryCreateTOtherFromTruncating<TOther>(T value, [MaybeNullWhen(false)] out TOther result) where TOther : INumberBase<TOther>
	{
		return T.TryConvertToTruncating(value, out result) || TOther.TryConvertFromTruncating(value, out result);
	}

	#endregion

	#region String conversions

	public override readonly string ToString() => ToString(null, null);

	public readonly string ToString(string? format) => ToString(format, null);

	public readonly string ToString(IFormatProvider? formatProvider) => ToString(null, formatProvider);

	public readonly string ToString(string? format, IFormatProvider? formatProvider)
	{
		DefaultInterpolatedStringHandler handler = new(4, 2, formatProvider);
		handler.AppendFormatted(this, format);
		return handler.ToStringAndClear();
	}

	public readonly bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
	{
		if (T.IsZero(Imaginary))
		{
			return TryFormat(destination, out charsWritten, format, provider);
		}

		charsWritten = 0;

		ComplexNumberFormatInfo formatInfo = ComplexNumberFormatInfo.GetInstance(provider);

		if (!T.IsZero(Real))
		{
			if (!StringUtil.TryFormatValue(ref destination, ref charsWritten, Real, format, formatInfo.NumberFormat)) return false;
			if (!StringUtil.TryFormatString(ref destination, ref charsWritten, formatInfo.Operator)) return false;
		}

		if (!StringUtil.TryFormatValue(ref destination, ref charsWritten, Imaginary, format, formatInfo.NumberFormat)) return false;
		if (!StringUtil.TryFormatString(ref destination, ref charsWritten, formatInfo.ImaginaryUnit)) return false;

		return true;
	}

	public static ComplexNumber<T> Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider)
	{
		throw new NotImplementedException();
	}

	public static ComplexNumber<T> Parse(string s, NumberStyles style, IFormatProvider? provider)
	{
		throw new NotImplementedException();
	}

	public static ComplexNumber<T> Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
	{
		throw new NotImplementedException();
	}

	public static ComplexNumber<T> Parse(string s, IFormatProvider? provider)
	{
		throw new NotImplementedException();
	}

	public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out ComplexNumber<T> result)
	{
		throw new NotImplementedException();
	}

	public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out ComplexNumber<T> result)
	{
		throw new NotImplementedException();
	}

	public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out ComplexNumber<T> result)
	{
		throw new NotImplementedException();
	}

	public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out ComplexNumber<T> result)
	{
		throw new NotImplementedException();
	}

	#endregion
}
