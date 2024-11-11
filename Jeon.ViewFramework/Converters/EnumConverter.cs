﻿using System;
using System.Globalization;
using System.Windows;

namespace Jeon.ViewFramework.Converters
{
	public class EnumConverter : ConverterMarkupExtension<EnumConverter>
	{
		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var result = this.GetConvertedObject(value, targetType);
			return result;
		}

		public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return this.GetConvertedObject(value, targetType);
		}

		private object GetConvertedObject(object value, Type targetType)
		{
			try
			{
				return Enum.Parse(targetType, value.ToString());
			}
			catch (Exception)
			{
				return DependencyProperty.UnsetValue;
			}
		}
	}
}
