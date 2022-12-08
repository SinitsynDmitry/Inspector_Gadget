namespace Inspector_Gadget_Maui.Controls
{
    public interface IVideoController
    {
        VideoStatus Status { get; set; }
        TimeSpan Duration { get; set; }
    }
}
