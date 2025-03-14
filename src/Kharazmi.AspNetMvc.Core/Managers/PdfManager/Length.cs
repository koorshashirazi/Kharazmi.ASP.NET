﻿using System.Globalization;

namespace Kharazmi.AspNetMvc.Core.Managers.PdfManager
{
    public sealed class Length
    {
        private readonly double _length;
        private readonly object _unitOfLength;

        private Length(double length, string unitOfLength)
        {
            _length = length;
            _unitOfLength = unitOfLength;
        }

        public string SettingString =>
            string.Format("{0}{1}", _length.ToString("0.##", CultureInfo.InvariantCulture), _unitOfLength);

        public static Length Zero()
        {
            return new Length(0, "");
        }

        public static Length Millimeters(double length)
        {
            return new Length(length, "mm");
        }

        public static Length Centimeters(double length)
        {
            return new Length(length, "cm");
        }

        public static Length Inches(double length)
        {
            return new Length(length, "in");
        }
    }
}