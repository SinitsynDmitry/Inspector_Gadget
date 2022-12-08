using Microsoft.Extensions.Logging;
using Inspector_Gadget_Maui.Controls;
using Inspector_Gadget_Maui.Handlers;

namespace Inspector_Gadget_Maui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
            .ConfigureMauiHandlers(handlers =>
             {
                 handlers.AddHandler(typeof(Video), typeof(VideoHandler));
             });

          

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}