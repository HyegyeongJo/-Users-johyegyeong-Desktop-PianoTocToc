using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToryCare
{
	public static class Utils
	{
		public static string ToFirstCharUpperLowerCase(string input)
		{
			if (input == null)
			{
				return null;
			}
			else if (input.Length > 1)
			{
				return char.ToUpper(input[0]) + input.ToLower().Substring(1);
			}
			return input.ToUpper();
		}
	}
}