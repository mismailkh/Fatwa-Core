using Microsoft.JSInterop;

namespace FATWA_WEB.Extensions
{
    public static class IJSRuntimeExtensionMehtods
    {
        public static async ValueTask InitilizeInActivityTimer<T>(this IJSRuntime js, DotNetObjectReference<T> dotnetObjectReference) where T : class
        {
            await js.InvokeVoidAsync("initilizeInActivityTimer", dotnetObjectReference);
        }
        public static async ValueTask InitilizePrincipleDetailReference<T>(this IJSRuntime js, DotNetObjectReference<T> dotnetObjectReference) where T : class
        {
            await js.InvokeVoidAsync("initilizePrincipleDetailReference", dotnetObjectReference);
        }
        public static async ValueTask InitilizeSignaturePanel<T>(this IJSRuntime js, DotNetObjectReference<T> dotnetObjectReference) where T : class
        {
            await js.InvokeVoidAsync("InitilizeSignaturePanel", dotnetObjectReference);
        }
        public static async ValueTask InitilizeMentionUserComponent<T>(this IJSRuntime js, DotNetObjectReference<T> dotnetObjectReference) where T : class
        {
            await js.InvokeVoidAsync("InitilizeMentionUserComponent", dotnetObjectReference);
        }
    }
}
