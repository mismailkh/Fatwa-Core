using DocumentFormat.OpenXml.Drawing.ChartDrawing;
using FATWA_DOMAIN.Models.ViewModel;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace FATWA_WEB.Pages.Shared
{
    public partial class TileComponent : ComponentBase
    {

        #region Parameter
        [Parameter]
        public string Title { get; set; }
        [Parameter]
        public int Value { get; set; }
        [Parameter]
        public string CSSClass { get; set; }
        #endregion
    }
}
