
using Microsoft.Maui.Controls.Shapes;
using OpenAI_API;
using System.Diagnostics;
using System.Security.AccessControl;
using Path = System.IO.Path;
using Inspector_Gadget_Maui.Controls;

namespace Inspector_Gadget_Maui
{
    public partial class MainPage : ContentPage
    {
        #region Member variables

        int count = 0;

        /// <summary>
        /// Transcription language
        /// </summary>
        private static string whisperLanguage = "English";

        /// <summary>
        /// Transcription model
        /// </summary>
        private static string whisperModel = whisperModels.tiny.ToString();

        /// <summary>
        /// cmd name
        /// </summary>
        private static long irCmdFileNameCounter = 0;

        /// <summary>
        /// Whisper process (speech-to-text)
        /// </summary>
        private Process whisper = null;

        #endregion /Member variables

        #region c_tor

        public MainPage()
        {
            try
            {
                Application.Current.UserAppTheme = AppTheme.Light;
                InitializeComponent();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion /c_tor

        #region Methods

        #region Button's event handlers

        /// <summary>
        /// Select file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BtnSelectFile_Clicked(object sender, EventArgs e)
        {
            try
            {
                var customFileType = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                // { DevicePlatform.iOS, new[] { "public.my.comic.extension" } }, // UTType values
                // { DevicePlatform.Android, new[] { "application/comics" } }, // MIME type
                { DevicePlatform.WinUI, new[] { ".mp3", ".mp4" } }, // file extension
                { DevicePlatform.Tizen, new[] { "*/*" } },
                { DevicePlatform.MacCatalyst, new[] { "mp3", "mp4" } }, // UTType values
                });

                PickOptions options = new()
                {
                    PickerTitle = "Please select a media file",
                    FileTypes = customFileType,
                };

                var result = await FilePicker.Default.PickAsync(options);
                if (result != null)
                {
                    entFilePath.Text = result.FullPath;

                }

                if (!string.IsNullOrWhiteSpace(result.FullPath))
                {
                    video.Source = new FileVideoSource
                    {
                        File = result.FullPath
                    };
                }

                ContentPage_SizeChanged(null, null);
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// Transcribe
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BtnTranscribe_Clicked(object sender, EventArgs e)
        {
            try
            {
                video.Stop();
                video.Source = null;
                await Navigation.PushAsync(new TranscriptionPage(entFilePath.Text));
            }
            catch (Exception ex) { throw ex; }
        }

        #endregion /Button's event handlers

        /// <summary>
        /// player unload
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnContentPageUnloaded(object sender, EventArgs e)
        {
            video.Handler?.DisconnectHandler();
        }

        /// <summary>
        /// page size changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContentPage_SizeChanged(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex) { throw ex; }

        }

        #endregion /Methods
    }
}
