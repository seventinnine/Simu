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
using Radzen;
using Radzen.Blazor;
using Simu.Common;

namespace Simu.Pages
{
    public partial class Sheets
    {

        public DamageCalculator Calculator { get; set; } = default!;

        public Stats CurrentlySelectedSheetStats { get; set; } = default!;

        protected override void OnInitialized()
        {
            //TODO: impl loading from static class? which handles reading from local storage?
            CurrentlySelectedSheetStats = new();
            CurrentlySelectedSheetStats.AddBaseStats();
            Calculator = new(CurrentlySelectedSheetStats);
        }

        public void RecalculateStats()
        {
            Calculator.RecalculateAllStats();
            Calculator.RefreshDamageBreakdown();
            //TODO: recalculate current breakdown and stuff
            //Console.WriteLine(Calculator.CalculateDamagePerSecond());
        }
    }
}