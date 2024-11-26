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
}

// implemented by the subclasses of the above
internal interface ICompositeFormatInfo<TSelf>
{
	static abstract TSelf Current { get; }

	static abstract TSelf Invariant { get; }

	static abstract TSelf GetInvariant(NumberFormatInfo? numberFormatInfo);
}
