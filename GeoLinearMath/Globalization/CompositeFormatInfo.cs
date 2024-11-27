using System;
using System.Globalization;

namespace GeoLinearMath.Globalization;

public class CompositeFormatInfo : IFormatProvider
{
	/// <summary>
	/// Gets or initializes the number format info to be used for the components of the value.
	/// </summary>
	public NumberFormatInfo? NumberFormat { get; init; }

	/// <inheritdoc/>
	public object? GetFormat(Type? formatType)
	{
		if (GetType().IsAssignableTo(formatType))
		{
			return this;
		}

		return NumberFormat?.GetFormat(formatType);
	}

	// Stringifying helper methods

	private protected static bool TryFormatString(ref Span<char> destination, ref int charsWritten, string value)
	{
		int length = value.Length;
		if (length > destination.Length)
		{
			return false;
		}

		value.CopyTo(destination);
		destination = destination[length..];
		charsWritten += length;
		return true;
	}

	private protected static bool TryFormatValue<T>(ref Span<char> destination, ref int charsWritten, T value, ReadOnlySpan<char> format, IFormatProvider? formatProvider)
		where T : ISpanFormattable
	{
		if (value is null) return true;

		bool success = value.TryFormat(destination, out int valueCharsWritten, format, formatProvider);
		destination = destination[valueCharsWritten..];
		charsWritten += valueCharsWritten;
		return success;
	}
}

// Implemented by the subclasses of the above
// Used to avoid code duplication for resolving an IFormatProvider
internal interface ICompositeFormatInfo<TSelf> where TSelf : ICompositeFormatInfo<TSelf>
{
	static abstract TSelf Current { get; }

	static abstract TSelf Invariant { get; }

	static abstract TSelf GetInvariant(NumberFormatInfo? numberFormatInfo);

	private protected static TSelf Resolve(IFormatProvider? formatProvider)
	{
		if (formatProvider is null) return TSelf.Current;
		if (formatProvider == CultureInfo.InvariantCulture) return TSelf.Invariant;

		if (formatProvider is TSelf result1)
		{
			return result1;
		}

		object? result = formatProvider.GetFormat(typeof(TSelf));
		if (result is TSelf result2)
		{
			return result2;
		}

		return TSelf.GetInvariant(NumberFormatInfo.GetInstance(formatProvider));
	}
}
