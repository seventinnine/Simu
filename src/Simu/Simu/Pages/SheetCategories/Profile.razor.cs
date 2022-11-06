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
using Simu.Common.Systems.Profile;
using System.Diagnostics;
using Simu.Models;
using Simu.Logic;

namespace Simu.Pages.SheetCategories
{
    public partial class Profile
    {

        [Parameter]
        public EventCallback OnStatsChanged { get; set; }

        [Parameter]
        public ProfileStats Model { get; set; } = default!;
        
        protected override void OnInitialized()
        {
            Model!.OnChange += OnValidSubmit;
        }

        public void OnValidSubmit()
        {
            OnStatsChanged.InvokeAsync();
        }

    }
}