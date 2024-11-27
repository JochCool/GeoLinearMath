using System;
using System.Globalization;
using System.Numerics;

namespace GeoLinearMath.Globalization;

/// <summary>
/// Provides information for formatting complex numbers.
/// </summary>
public class ComplexNumberFormatInfo : CompositeFormatInfo, ICompositeFormatInfo<ComplexNumberFormatInfo>
{
	string @operator = " + ";
	string imaginaryUnit = "i";

	/// <summary>
	/// Gets or initializes the sequence of characters representing the addition of the real and the complex part of a number (defaults to <c> + </c>).
	/// </summary>
	/// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
	public string Operator
	{
		get => @operator;
		init => @operator = value ?? throw new ArgumentNullException(nameof(value));
	}

	/// <summary>
	/// Gets or initializes the sequence of characters representing the imaginary unit (defaults to <c>i</c>).
	/// </summary>
	/// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
	public string ImaginaryUnit
	{
		get => imaginaryUnit;
		init => imaginaryUnit = value ?? throw new ArgumentNullException(nameof(value));
	}

	/// <summary>
	/// Gets a <see cref="ComplexNumberFormatInfo"/> instance with the format for the invariant culture.
	/// </summary>
	public static ComplexNumberFormatInfo Invariant { get; } = new() { NumberFormat = CultureInfo.InvariantCulture.NumberFormat };

	/// <summary>
	/// Gets a <see cref="ComplexNumberFormatInfo"/> instance with the format associated with the culture used by the current thread.
	/// </summary>
	public static ComplexNumberFormatInfo Current => new() { NumberFormat = CultureInfo.CurrentCulture.NumberFormat };

	static ComplexNumberFormatInfo ICompositeFormatInfo<ComplexNumberFormatInfo>.GetInvariant(NumberFormatInfo? numberFormatInfo) => new() { NumberFormat = numberFormatInfo };

	/// <summary>
	/// Gets a <see cref="ComplexNumberFormatInfo"/> instance associated with a specified format provider. 
	/// </summary>
	/// <param name="formatProvider">The format provider used to get formatting info, or <see langword="null"/> to get <see cref="Current"/>.</param>
	/// <returns>The <see cref="ComplexNumberFormatInfo"/> instance associated with <paramref name="formatProvider"/>. This may be the same instance.</returns>
	public static ComplexNumberFormatInfo GetInstance(IFormatProvider? formatProvider)
	{
		return ICompositeFormatInfo<ComplexNumberFormatInfo>.Resolve(formatProvider);
	}

	internal bool TryFormat<T>(Span<char> destination, out int charsWritten, ComplexNumber<T> number, ReadOnlySpan<char> format)
		where T : INumber<T>
	{
		// simple case for real numbers
		if (T.IsZero(number.Imaginary))
		{
			return number.Real.TryFormat(destination, out charsWritten, format, NumberFormat);
		}

		charsWritten = 0;

		if (!T.IsZero(number.Real))
		{
			if (!TryFormatValue(ref destination, ref charsWritten, number.Real, format, NumberFormat)) return false;
			if (!TryFormatString(ref destination, ref charsWritten, Operator)) return false;
		}

		if (!TryFormatValue(ref destination, ref charsWritten, number.Imaginary, format, NumberFormat)) return false;
		if (!TryFormatString(ref destination, ref charsWritten, ImaginaryUnit)) return false;

		return true;
	}
}
