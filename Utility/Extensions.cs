﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace StudentEmploymentPortal.Utility
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            var displayAttribute = enumValue
                .GetType()
                .GetMember(enumValue.ToString())
                .FirstOrDefault()
                ?.GetCustomAttributes(typeof(DisplayAttribute), false)
                .OfType<DisplayAttribute>()
                .FirstOrDefault();

            return displayAttribute?.Name ?? enumValue.ToString();
        }
    }
}
