using Microsoft.AspNetCore.Components;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.Components.Admin;

public partial class InputNullableBoolean
{
	public sbyte ValueAsSignedByte
	{
		get => CurrentValue switch
		{
			true => 1,
			false => 0,
			_ => -1,
		};
		set => CurrentValue = value switch
		{
			1 => true,
			0 => false,
			_ => null,
		};
	}

	[Parameter] public string False { get; set; } = "False";
	[Parameter] public string True { get; set; } = "True";

	protected override bool TryParseValueFromString(string? value, out bool? result, out string validationErrorMessage)
	{
		if (value == False)
		{
			result = false;
			validationErrorMessage = string.Empty;
		}
		else if (value == True)
		{
			result = true;
			validationErrorMessage = string.Empty;
		}
		else if (value == "Unknown")
		{
			result = null;
			validationErrorMessage = string.Empty;
		}
		else
		{
			result = null;
			validationErrorMessage = ($"{value} is not a supported value for {nameof(InputNullableBoolean)}.";
		}

		return validationErrorMessage == string.Empty;
	}
}
