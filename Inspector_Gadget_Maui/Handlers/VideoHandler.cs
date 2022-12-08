#if IOS || MACCATALYST
using PlatformView = Inspector_Gadget_Maui.Platforms.MaciOS.MauiVideoPlayer;
#elif ANDROID
using PlatformView = Inspector_Gadget_Maui.Platforms.Android.MauiVideoPlayer;
#elif WINDOWS
using PlatformView = Inspector_Gadget_Maui.Platforms.Windows.MauiVideoPlayer;
#elif (NETSTANDARD || !PLATFORM) || (NET6_0_OR_GREATER && !IOS && !ANDROID)
using PlatformView = System.Object;
#endif
using Inspector_Gadget_Maui.Controls;
using Microsoft.Maui.Handlers;


namespace Inspector_Gadget_Maui.Handlers
{
    public partial class VideoHandler
    {
        public static IPropertyMapper<Video, VideoHandler> PropertyMapper = new PropertyMapper<Video, VideoHandler>(ViewHandler.ViewMapper)
        {
            [nameof(Video.AreTransportControlsEnabled)] = MapAreTransportControlsEnabled,
            [nameof(Video.Source)] = MapSource,
            [nameof(Video.IsLooping)] = MapIsLooping,
            [nameof(Video.Position)] = MapPosition
        };

        public static CommandMapper<Video, VideoHandler> CommandMapper = new(ViewCommandMapper)
        {
            [nameof(Video.UpdateStatus)] = MapUpdateStatus,
            [nameof(Video.PlayRequested)] = MapPlayRequested,
            [nameof(Video.PauseRequested)] = MapPauseRequested,
            [nameof(Video.StopRequested)] = MapStopRequested
        };

        public VideoHandler() : base(PropertyMapper, CommandMapper)
        {
        }
    }
}
