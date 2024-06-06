using System;

namespace GeoLinearMath;

internal static class VectorUtil
{
	internal static bool TryFormat<T>(Span<char> destination, out int charsWritten, ReadOnlySpan<T> components, ReadOnlySpan<char> format, IFormatProvider? formatProvider)
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
			T component = components[i];
			if (component is null) continue;
			bool success = component.TryFormat(destination, out int componentCharsWritten, format, vectorFormatInfo.NumberFormat);
			destination = destination[componentCharsWritten..];
			charsWritten += componentCharsWritten;

			if (!success) return false;
		}

		return TryFormatString(ref destination, ref charsWritten, vectorFormatInfo.Closing);
	}

	private static bool TryFormatString(ref Span<char> destination, ref int charsWritten, string value)
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

	internal static ReadOnlySpan<char> RemoveOpeningAndClosing(ReadOnlySpan<char> chars, VectorFormatInfo vectorFormatInfo)
	{
		string opening = vectorFormatInfo.Opening;
		string closing = vectorFormatInfo.Closing;
		if (!chars.StartsWith(opening) || !chars.EndsWith(closing))
		{
			return default;
		}
		return chars[opening.Length..closing.Length];
	}
}
