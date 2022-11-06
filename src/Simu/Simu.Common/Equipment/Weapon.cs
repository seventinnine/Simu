using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simu.Common.Equipment
{
    public class Weapon
    {
        public string Name { get; set; } = string.Empty;
        public AttackMode Type { get; set; } = AttackMode.Melee;
        public bool IsShortbow { get; set; } = false;
        public decimal Damage { get; set; } = 5.0m;
    }
}
