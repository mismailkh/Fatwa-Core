using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using FATWA_DOMAIN.Models;
using FATWA_ADMIN.Pages;
using Microsoft.JSInterop;

namespace FATWA_ADMIN.Layouts
{
	//<History Author = 'Hassan Abbas' Date='2022-03-14' Version="1.0" Branch="master">created main layout component.</History>
	public partial class MainLayoutComponent : LayoutComponentBase
	{
		[Inject]
		protected NavigationManager UriHelper { get; set; }

		[Inject]
		protected DialogService dialogService { get; set; }

		[Inject]
		protected TooltipService TooltipService { get; set; }

		[Inject]
		protected ContextMenuService ContextMenuService { get; set; }

		[Inject]
		protected NotificationService NotificationService { get; set; }

		protected RadzenBody body0;
		protected RadzenSidebar sidebar0;
		protected RadzenHeader Head0;
		protected IJSRuntime JsInterop;


		/*protected async Task SidebarToggle0Click(dynamic args)
		{

			await InvokeAsync(() => { body0.Toggle(); });

			await JsInterop.InvokeVoidAsync("ToggleSideBar");
		}*/
	}
}
