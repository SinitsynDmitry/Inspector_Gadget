﻿namespace Inspector_Gadget_Maui.Controls
{
    public class VideoInfo
    {
        public string DisplayName { get; set; }
        public VideoSource VideoSource { get; set; }

        public override string ToString()
        {
            return DisplayName;
        }
    }
}
