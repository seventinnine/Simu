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
using Radzen;

namespace Simu.Pages
{
    public partial class Sheet
    {
        #region Models
        public ProfileStats ProfileData { get; set; } = default!;

        #endregion


        [Parameter]
        public EventCallback OnStatsChanged { get; set; }

        [Parameter]
        public Stats Stats { get; set; } = default!;



        //TODO: how to load savestate?
        protected override void OnInitialized()
        {
            #region Create Models

            ProfileData = new(Stats);
            
            #endregion
        }

        public Sheet Clone()
        {
            Sheet cpy = new Sheet() { Stats = Stats.Clone()};
            cpy.ProfileData = new ProfileStats(cpy.Stats, ProfileData);
            return cpy;
        }

        public void NotifyStatsChanged()
        {
            OnStatsChanged.InvokeAsync();
        }


    }
}