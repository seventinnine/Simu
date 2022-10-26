using System.Text.Json.Serialization;

namespace Simu.Common
{

    public class Stats
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public SortedSet<StatTag> Tags { get; set; } = new();
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public decimal FlatAttackDamage { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public decimal FlatAbilityDamage { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public decimal IncreasedDamageMeleePercent { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public decimal IncreasedDamageRangedPercent { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public decimal IncreasedDamageMagicPercent { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public decimal MoreDamagePercent { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public decimal Health { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public decimal Defense { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public decimal TrueDefense { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public decimal Speed { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public decimal Strength { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public decimal Intelligence { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public decimal CritChancePercent { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public decimal CritDamagePercent { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public decimal AttackSpeedPercent { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public decimal Ferocity { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public decimal AbilityDamagePercent { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public decimal MagicFind { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public decimal PetLuck { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public decimal SeaCreatureChance { get; set; }

        public Stats MergeWith(Stats other)
        {
            Stats merged = new()
            {
                Tags = this.Tags,
                FlatAttackDamage = this.FlatAttackDamage + other.FlatAttackDamage,
                FlatAbilityDamage = this.FlatAbilityDamage + other.FlatAbilityDamage,
                IncreasedDamageMeleePercent = this.IncreasedDamageMeleePercent + other.IncreasedDamageMeleePercent,
                IncreasedDamageRangedPercent = this.IncreasedDamageRangedPercent + other.IncreasedDamageRangedPercent,
                IncreasedDamageMagicPercent = this.IncreasedDamageMagicPercent + other.IncreasedDamageMagicPercent,
                MoreDamagePercent = ((1 + this.MoreDamagePercent / 100) * (1 + other.MoreDamagePercent / 100) - 1) * 100,
                Health = this.Health + other.Health,
                Defense = this.Defense + other.Defense,
                TrueDefense = this.TrueDefense + other.TrueDefense,
                Speed = this.Speed + other.Speed,
                Strength = this.Strength + other.Strength,
                Intelligence = this.Intelligence + other.Intelligence,
                CritChancePercent = this.CritChancePercent + other.CritChancePercent,
                CritDamagePercent = this.CritDamagePercent + other.CritDamagePercent,
                AttackSpeedPercent = this.AttackSpeedPercent + other.AttackSpeedPercent,
                AbilityDamagePercent = this.AbilityDamagePercent + other.AbilityDamagePercent,
                MagicFind = this.MagicFind + other.MagicFind,
                PetLuck = this.PetLuck + other.PetLuck,
                SeaCreatureChance = this.SeaCreatureChance + other.SeaCreatureChance
            };
            other.Tags.ToList().ForEach(t => merged.Tags.Add(t));
            return merged;
        }
    }
}