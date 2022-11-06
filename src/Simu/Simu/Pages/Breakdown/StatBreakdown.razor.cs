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

namespace Simu.Pages.Breakdown
{
    public partial class StatBreakdown
    {

        [Inject]
        protected TooltipService tooltipService { get; set; } = default!;
        

        [Parameter]
        public Stats AllStats { get; set; } = default!;
        [Parameter]
        public DamageCalculator Calculator { get; set; } = default!;

        private TooltipOptions _options = new() { Position = TooltipPosition.Right, Duration = null, Style = "background-color: rgba(24, 24, 24, 0.9);" };

    }
}