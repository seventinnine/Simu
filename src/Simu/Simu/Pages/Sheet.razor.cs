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
using Simu.Models;
using Simu.Logic;

namespace Simu.Pages
{
    public partial class Sheet
    {
        #region Models
        public ProfileModel ProfileData { get; set; }

        #endregion
        
        public Stats Stats { get; set; }
        public StatsCalculator Calculator { get; set; }
        public AttackMode Mode { get; set; }

        public Sheet()
        {
            Mode = AttackMode.Melee;
            Stats = new();
            Stats.AddBaseStats();
            Calculator = new(Stats, Mode);

            #region Create Models

            ProfileData = new(Stats);
            #endregion
        }

        protected override void OnInitialized()
        {
            #region MyRegion

            #endregion
        }

        public Sheet Clone()
        {
            Sheet cpy = new Sheet() { Stats = Stats.Clone(), Mode = Mode};
            cpy.Calculator = new StatsCalculator(cpy.Stats, Mode);
            cpy.ProfileData = ProfileData.Clone(cpy.Stats);
            return cpy;
        }

        public void RecalculateDamage()
        {
            Calculator.RecalculateAllStats();
            Console.WriteLine(Calculator.CalculateDamagePerSecond());
        }

    }
}