using FATWA_DOMAIN.Models.CaseManagment;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using static FATWA_DOMAIN.Enums.WorkflowParameterEnums;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;

namespace FATWA_WEB.Pages.Shared
{
    //<History Author = 'Hassan Abbas' Date='2022-10-30' Version="1.0" Branch="master">Render Parameter Component</History>
    public partial class FileDraftParameter : ComponentBase
    {
        [Parameter]
        public CaseTemplateParamsEnum ParameterType { get; set; }
        [Parameter]
        public bool Disabled { get; set; }

        [Parameter]
        public string? Class { get; set; }

        [Parameter]
        public string ParamValue { get; set; }

        [Parameter]
        public EventCallback<string> ParamValueChanged { get; set; }

        protected async Task ValueChanged()
        {
            await ParamValueChanged.InvokeAsync(ParamValue);
        }
    }
}
