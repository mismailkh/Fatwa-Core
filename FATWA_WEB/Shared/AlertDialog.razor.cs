using DocumentFormat.OpenXml.Drawing;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FATWA_WEB.Shared
{
    public partial class AlertDialog : ComponentBase
    {

        [Parameter]
        public string? Title { get; set; }

        [Parameter]
        public string? Content { get; set; }

        [Parameter]
        public string? OkButtonText { get; set; }

        private async Task OnOkClick()
        {
            dialogService.Close(null);
        }
    }
}
