using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Microsoft.JSInterop;
using Simu;
using Simu.Shared;
using Simu.Common;

namespace Simu.Pages
{
    public partial class Sheet
    {
        public StatsCalculator Calculator { get; set; }
        public Stats Stats { get; set; }
        public AttackMode Mode { get; set; }

        public Sheet()
        {
            Mode = AttackMode.Melee;
            Stats = new();
            Calculator = new(Stats, Mode);
        }
        public Sheet Clone()
        {
            Sheet cpy = new Sheet() { Stats = Stats.Clone(), Mode = Mode};
            cpy.Calculator = new StatsCalculator(cpy.Stats, Mode);
            return cpy;
        }

        public void RecalculateDamage()
        {
            Calculator.RecalculateAllStats();
            Console.WriteLine(Calculator.CalculateDamagePerSecond());
        }

    }
}