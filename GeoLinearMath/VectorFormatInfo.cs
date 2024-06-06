using System;
using System.Globalization;
using System.Text;

namespace GeoLinearMath;

/// <summary>
/// Provides information for formatting vectors.
/// </summary>
public class VectorFormatInfo : IFormatProvider
{
	string opening = "(";
	string separator = ", ";
	string closing = ")";

	/// <summary>
	/// Gets or initializes the number format info to be used for the components of the vector.
	/// </summary>
	public NumberFormatInfo? NumberFormat { get; init; }

	/// <summary>
	/// Gets or initializes the sequence of characters indicating the start of the vector (defaults to <c>(</c>).
	/// </summary>
	/// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
	public string Opening
	{
		get => opening;
		init => opening = value ?? throw new ArgumentNullException(nameof(value));
	}

	/// <summary>
	/// Gets or initializes the sequence of characters separating the components of the vector (defaults to <c>, </c>).
	/// </summary>
	/// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
	public string Separator
	{
		get => separator;
		init => separator = value ?? throw new ArgumentNullException(nameof(value));
	}

	/// <summary>
	/// Gets or initializes the sequence of characters indicating the end of the vector (defaults to <c>)</c>).
	/// </summary>
	/// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
	public string Closing
	{
		get => closing;
		init => closing = value ?? throw new ArgumentNullException(nameof(value));
	}

	/// <summary>
	/// Gets a <see cref="VectorFormatInfo"/> instance with the format for the invariant culture.
	/// </summary>
	public static VectorFormatInfo Invariant { get; } = new() { NumberFormat = CultureInfo.InvariantCulture.NumberFormat };

	/// <summary>
	/// Gets a <see cref="VectorFormatInfo"/> instance with the format associated with the culture used by the current thread.
	/// </summary>
	public static VectorFormatInfo Current => new() { NumberFormat = CultureInfo.CurrentCulture.NumberFormat };

	/// <inheritdoc/>
	public object? GetFormat(Type? formatType)
	{
		if (formatType == typeof(VectorFormatInfo))
		{
			return this;
		}
		
		return NumberFormat?.GetFormat(formatType);
	}

	/// <summary>
	/// Gets a <see cref="VectorFormatInfo"/> instance associated with a specified format provider. 
	/// </summary>
	/// <param name="formatProvider">The format provider used to get formatting info, or <see langword="null"/> to get <see cref="Current"/>.</param>
	/// <returns>The <see cref="VectorFormatInfo"/> instance associated with <paramref name="formatProvider"/>. This may be the same instance.</returns>
	public static VectorFormatInfo GetInstance(IFormatProvider? formatProvider)
	{
		if (formatProvider is null) return Current;
		if (formatProvider == CultureInfo.InvariantCulture) return Invariant;

		if (formatProvider is VectorFormatInfo vectorFormatInfo1)
		{
			return vectorFormatInfo1;
		}

		object? result = formatProvider.GetFormat(typeof(VectorFormatInfo));
		if (result is VectorFormatInfo vectorFormatInfo2)
		{
			return vectorFormatInfo2;
		}

		return new() { NumberFormat = NumberFormatInfo.GetInstance(formatProvider) };
	}
}
