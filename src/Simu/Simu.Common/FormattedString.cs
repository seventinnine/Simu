using System;

namespace Simu.Common
{
    public class FormattedString : ICloneable
    {
        public string Content { get; set; } = "<none>";
        public string ColorCodeHex { get; set; } = Constants.ColorCodeHex.DEFAULT;
        public bool IsUnderline { get; set; }
        public bool IsBold { get; set; }
        public bool IsItalic { get; set; }
        public bool IsStrikethrough { get; set; }
        public bool IsCursed { get; set; }
        public bool IsReset { get; set; }
        public bool IsNewline { get; set; }


        public object Clone()
        {
            return new FormattedString
            {
                Content = Content,
                ColorCodeHex = ColorCodeHex,
                IsUnderline = IsUnderline,
                IsBold = IsBold,
                IsItalic = IsItalic,
                IsStrikethrough = IsStrikethrough,
                IsCursed = IsCursed,
                IsReset = IsReset,
                IsNewline = IsNewline
            };
        }

        public override string ToString()
        {
            return $"{ColorCodeHex} {Content}";
        }
    }
}
