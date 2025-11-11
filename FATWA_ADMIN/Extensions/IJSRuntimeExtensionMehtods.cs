using Microsoft.JSInterop;

namespace FATWA_ADMIN.Extensions
{
    public static class IJSRuntimeExtensionMehtods
    {
        public static async ValueTask InitilizeInActivityTimer<T>(this IJSRuntime js, DotNetObjectReference<T> dotnetObjectReference) where T : class
        {
            await js.InvokeVoidAsync("initilizeInActivityTimer", dotnetObjectReference);
        }
    }
}
