using Simu.Common.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simu.Common
{
    public class FormattedStringConverter
    {
        public static string AsFormattedString(FormattedString s)
        {
            string res = $"color: #{s.ColorCodeHex}";

            if (s.IsUnderline && s.IsStrikethrough) res += "; text-decoration: underline line-through";
            else
            {
                if (s.IsUnderline) res += "; text-decoration: underline";
                if (s.IsStrikethrough) res += "; text-decoration: line-through";
            }
            if (s.IsBold) res += "; font-weight: bold";
            if (s.IsItalic) res += "; font-style: italic";
            if (s.IsCursed) res += "; font-weight: bold";

            return res;
        }

        private static readonly Dictionary<string, string> MinecraftColorCodeToEnum = new()
        {
            { "§0", ColorCodeHex.BLACK },
            { "§1", ColorCodeHex.DARK_BLUE },
            { "§2", ColorCodeHex.DARK_GREEN },
            { "§3", ColorCodeHex.DARK_AQUA },
            { "§4", ColorCodeHex.DARK_RED },
            { "§5", ColorCodeHex.DARK_PURPLE },
            { "§6", ColorCodeHex.GOLD },
            { "§7", ColorCodeHex.GRAY },
            { "§8", ColorCodeHex.DARK_GRAY },
            { "§9", ColorCodeHex.BLUE },
            { "§a", ColorCodeHex.GREEN },
            { "§b", ColorCodeHex.AQUA },
            { "§c", ColorCodeHex.RED },
            { "§d", ColorCodeHex.LIGHT_PURPLE },
            { "§e", ColorCodeHex.YELLOW },
            { "§f", ColorCodeHex.WHITE },
            { "§g", ColorCodeHex.MINECOOIN_GOLD }
        };

        private static void AssignColorCodeHex(string s, FormattedString cs)
        {
            if (MinecraftColorCodeToEnum.ContainsKey(s))
                cs.ColorCodeHex = MinecraftColorCodeToEnum[s];
        }

        private static void AssignFormatCode(string s, FormattedString cs)
        {
            switch (s)
            {
                case "§u":
                    cs.IsUnderline = true;
                    break;
                case "§l":
                    cs.IsBold = true;
                    break;
                case "§o":
                    cs.IsItalic = true;
                    break;
                case "§m":
                    cs.IsStrikethrough = true;
                    break;
                case "§k":
                    cs.IsCursed = true;
                    break;
                case "§r":
                    break;
                default:
                    break;
            };
        }

        public static IList<FormattedString> FromString(string s)
        {
            var res = new List<FormattedString>();
            var currFormattedString = new FormattedString();
            StringBuilder currString = new();
            var enumerator = s.GetEnumerator();

            while (enumerator.MoveNext())
            {
                if (enumerator.Current == '\n')
                {
                    if (currString.ToString() != string.Empty)
                    {
                        currFormattedString.Content = currString.ToString();
                        currString.Clear();
                        res.Add(currFormattedString);
                        currFormattedString = (FormattedString)currFormattedString.Clone();
                    }
                    res.Add(new FormattedString() { IsNewline = true });
                }
                else if (enumerator.Current == '§') // color/format code starts
                {
                    enumerator.MoveNext();
                    string code = "§" + enumerator.Current;

                    if ((MinecraftColorCodeToEnum.ContainsKey(code) || code == "§r") && currString.ToString() != string.Empty) // is color code or reset => terminate
                    {
                        currFormattedString.Content = currString.ToString();
                        currString.Clear();
                        res.Add(currFormattedString);
                        currFormattedString = new();
                        AssignColorCodeHex(code, currFormattedString);
                    }
                    else
                    {
                        AssignColorCodeHex(code, currFormattedString);
                        AssignFormatCode(code, currFormattedString);
                    }
                }
                else
                {
                    currString.Append(enumerator.Current);
                }
            }

            // terminate remainder
            currFormattedString.Content = currString.ToString();
            if (currFormattedString.Content != string.Empty)
                res.Add(currFormattedString);

            return res;
        }
    }
}
