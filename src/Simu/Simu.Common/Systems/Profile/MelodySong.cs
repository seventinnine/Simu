using Simu.Common.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simu.Common.Systems.Profile
{
    public class MelodySong
    {
        public readonly static Dictionary<string, MelodySong> IntelligenceForSongs = new()
        {
            { HYMN_JOY, new("Hymn to the Joy", 1)},
            { FRERE_JAQUES, new("Frère Jacques", 1)},
            { AMAZING_GRACE, new("Amazing Grace", 1)},
            { BRAHMS, new("Brahm's Lullaby", 2)},
            { HAPPY_BIRTHDAY, new("Happy Birthday to You", 2)},
            { GREENSLEEVES, new("Greensleeves", 2)},
            { JEOPARDY, new("Geothermy?", 3)},
            { MINUET, new("Minuet", 3)},
            { JOY_WORLD, new("Joy to the World", 3)},
            { PURE_IMAGINATION, new("Godly Imagination", 4)},
            { VIE_EN_ROSE, new("La Vie en Rose", 4)},
            { FIRE_AND_FLAME, new("Through the Campfire", 1)},
            { PACHBEL, new("Pachelbel", 1)}
        };

        public const string HYMN_JOY = "song_hymn_joy_perfect_completions";
        public const string FRERE_JAQUES = "song_frere_jacques_perfect_completions";
        public const string AMAZING_GRACE = "song_amazing_grace_perfect_completions";
        public const string BRAHMS = "song_brahms_perfect_completions";
        public const string HAPPY_BIRTHDAY = "song_happy_birthday_perfect_completions";
        public const string GREENSLEEVES = "song_greensleeves_perfect_completions";
        public const string JEOPARDY = "song_jeopardy_perfect_completions";
        public const string MINUET = "song_minuet_perfect_completions";
        public const string JOY_WORLD = "song_joy_world_perfect_completions";
        public const string PURE_IMAGINATION = "song_pure_imagination_perfect_completions";
        public const string VIE_EN_ROSE = "song_vie_en_rose_perfect_completions";
        public const string FIRE_AND_FLAME = "song_fire_and_flame_perfect_completions";
        public const string PACHBEL = "song_pachelbel_perfect_completions";

        public string DisplayName { get; set; } = string.Empty;
        public int Intelligence { get; set; }

        public MelodySong(string displayName, int intelligence)
        {
            DisplayName = displayName;
            Intelligence = intelligence;
        }

        public IEnumerable<FormattedString> ToFormattedString()
        {
            return new List<FormattedString>
            {
                new() { ColorCodeHex = ColorCodeHex.AQUA, Content = $"{DisplayName} ({Intelligence} {DisplayConstants.SYMBOL_INTELLIGENCE} (INT))"}

            };
        }
    }
}
