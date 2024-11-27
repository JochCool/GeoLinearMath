using System;
using System.Globalization;

namespace GeoLinearMath.Globalization;

/// <summary>
/// Provides information for formatting vectors.
/// </summary>
public class VectorFormatInfo : CompositeFormatInfo, ICompositeFormatInfo<VectorFormatInfo>
{
	string opening = "(";
	string separator = ", ";
	string closing = ")";

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

	static VectorFormatInfo ICompositeFormatInfo<VectorFormatInfo>.GetInvariant(NumberFormatInfo? numberFormatInfo) => new() { NumberFormat = numberFormatInfo };

	/// <summary>
	/// Gets a <see cref="VectorFormatInfo"/> instance associated with a specified format provider. 
	/// </summary>
	/// <param name="formatProvider">The format provider used to get formatting info, or <see langword="null"/> to get <see cref="Current"/>.</param>
	/// <returns>The <see cref="VectorFormatInfo"/> instance associated with <paramref name="formatProvider"/>. This may be the same instance.</returns>
	public static VectorFormatInfo GetInstance(IFormatProvider? formatProvider)
	{
		return ICompositeFormatInfo<VectorFormatInfo>.Resolve(formatProvider);
	}

	internal bool TryFormat<T>(Span<char> destination, out int charsWritten, ReadOnlySpan<T> components, ReadOnlySpan<char> format)
		where T : ISpanFormattable
	{
		charsWritten = 0;

		if (!TryFormatString(ref destination, ref charsWritten, Opening)) return false;

		for (int i = 0; i < components.Length; i++)
		{
			// Write comma
			if (i != 0)
			{
				if (!TryFormatString(ref destination, ref charsWritten, Separator)) return false;
			}

			// Write component
			if (!TryFormatValue(ref destination, ref charsWritten, components[i], format, NumberFormat)) return false;
		}

		return TryFormatString(ref destination, ref charsWritten, Closing);
	}

	// The length of resultComponents determines the size of the vector
	// If the method returns true, contains the components of the vector
	// If the method returns false, its contents are undefined
	internal bool TryParse<T>(ReadOnlySpan<char> chars, Span<T> resultComponents)
		where T : ISpanParsable<T>
	{
		ReadOnlySpan<char> opening = Opening;
		ReadOnlySpan<char> closing = Closing;
		if (!chars.StartsWith(opening) || !chars.EndsWith(closing))
		{
			return false;
		}
		chars = chars[opening.Length..^ closing.Length];

		ReadOnlySpan<char> separator = Separator;
		int separatorsCount = resultComponents.Length - 1;
		for (int componentIndex = 0; componentIndex < separatorsCount; componentIndex++)
		{
			int separatorIndex = chars.IndexOf(separator);
			if (separatorIndex == -1) return false;

			ReadOnlySpan<char> componentSpan = chars[..separatorIndex];
			chars = chars[(separatorIndex + separator.Length)..];

			bool success = T.TryParse(componentSpan, NumberFormat, out resultComponents[componentIndex]!);
			if (!success) return false;
		}

		return T.TryParse(chars, NumberFormat, out resultComponents[^1]!);
	}
}
