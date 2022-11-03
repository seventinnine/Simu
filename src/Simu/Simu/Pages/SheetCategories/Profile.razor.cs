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
        public Stats? AllStats { get; set; }
        [Parameter]
        public EventCallback OnStatsChanged { get; set; }
        [Inject]
        protected ProfileLogic _logic { get; set; } = default!;

        public ProfileModel ProfileData { get; set; } = default!;
        private EditContext? context;

        private bool _isInitialized = false;

        protected override void OnInitialized()
        {
            ProfileData = new(_logic);
            context = new EditContext(ProfileData);
            ProfileData.Initialize(AllStats!, context);
            ProfileData.OnChange += OnValidSubmit;
            _isInitialized = true;
        }

        /// <summary>
        /// Completed <see cref="MelodySong"/>s by name.
        /// </summary>
        public List<string> MelodyCompletionNames { get; set; } = new();

        /// <summary>
        /// SlayerType => level
        /// </summary>
        public Dictionary<string, int> SlayerLevel { get; set; } = new();

        public void OnValidSubmit()
        {
            if (_isInitialized)
            {
                OnStatsChanged.InvokeAsync();
            }
        }

    }
}