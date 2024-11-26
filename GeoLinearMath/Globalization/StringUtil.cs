using System;
using System.Globalization;

namespace GeoLinearMath.Globalization;

internal static class StringUtil
{
	// Generic version of the methods found in CompositeFormatInfo's subclasses
	public static TFormatInfo GetFormatInfo<TFormatInfo>(IFormatProvider? formatProvider) where TFormatInfo : ICompositeFormatInfo<TFormatInfo>
	{
		if (formatProvider is null) return TFormatInfo.Current;
		if (formatProvider == CultureInfo.InvariantCulture) return TFormatInfo.Invariant;

		if (formatProvider is TFormatInfo vectorFormatInfo1)
		{
			return vectorFormatInfo1;
		}

		object? result = formatProvider.GetFormat(typeof(TFormatInfo));
		if (result is TFormatInfo vectorFormatInfo2)
		{
			return vectorFormatInfo2;
		}

		return TFormatInfo.GetInvariant(NumberFormatInfo.GetInstance(formatProvider));
	}

	internal static bool TryFormatVector<T>(Span<char> destination, out int charsWritten, ReadOnlySpan<T> components, ReadOnlySpan<char> format, IFormatProvider? formatProvider)
		where T : ISpanFormattable
	{
		charsWritten = 0;

		VectorFormatInfo vectorFormatInfo = VectorFormatInfo.GetInstance(formatProvider);

		if (!TryFormatString(ref destination, ref charsWritten, vectorFormatInfo.Opening)) return false;

		for (int i = 0; i < components.Length; i++)
		{
			// Write comma
			if (i != 0)
			{
				if (!TryFormatString(ref destination, ref charsWritten, vectorFormatInfo.Separator)) return false;
			}

			// Write component
			if (!TryFormatValue(ref destination, ref charsWritten, components[i], format, vectorFormatInfo.NumberFormat)) return false;
		}

		return TryFormatString(ref destination, ref charsWritten, vectorFormatInfo.Closing);
	}

	internal static bool TryFormatString(ref Span<char> destination, ref int charsWritten, string value)
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

	internal static bool TryFormatValue<T>(ref Span<char> destination, ref int charsWritten, T value, ReadOnlySpan<char> format, IFormatProvider? formatProvider)
		where T : ISpanFormattable
	{
		if (value is null) return true;

		bool success = value.TryFormat(destination, out int valueCharsWritten, format, formatProvider);
		destination = destination[valueCharsWritten..];
		charsWritten += valueCharsWritten;
		return success;
	}

	internal static ReadOnlySpan<char> RemoveOpeningAndClosing(ReadOnlySpan<char> chars, VectorFormatInfo vectorFormatInfo)
	{
		string opening = vectorFormatInfo.Opening;
		string closing = vectorFormatInfo.Closing;
		if (!chars.StartsWith(opening) || !chars.EndsWith(closing))
		{
			return default;
		}
		return chars[opening.Length..^closing.Length];
	}
}
