
using OpenAI_API;

namespace Inspector_Gadget_Maui
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            //count++;

            //if (count == 1)
            //    CounterBtn.Text = $"Clicked {count} time";
            //else
            //    CounterBtn.Text = $"Clicked {count} times";

            //SemanticScreenReader.Announce(CounterBtn.Text);
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            //    try
            //    {
            //        Settings settings = new Settings();
            //        settings.Owner = Window.GetWindow(this);


            //        if (settings.ShowDialog() == true)
            //        {

            //        }
            //    }
            //    catch (Exception ex) { throw ex; }
        }

        private async void btn_submit_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(tbx_input.Text))
                {
                    tbx_output.Text = "";

                    var api = new OpenAIAPI("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", new Engine("text-davinci-003"));

                    await foreach (var token in api.Completions.StreamCompletionEnumerableAsync(new CompletionRequest(tbx_input.Text + " tl;dr:", temperature: 0.7, max_tokens: 60, top_p: 1.0, frequencyPenalty: 0.0, presencePenalty: 1)))
                    {
                        tbx_output.Text += token.ToString();
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private async void btn_keywords_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(tbx_input.Text))
                {
                    tbx_keywords.Text = "";

                    var api = new OpenAIAPI("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", new Engine("text-davinci-003"));

                    await foreach (var token in api.Completions.StreamCompletionEnumerableAsync(new CompletionRequest("Extract keywords from this text:\n\n" + tbx_input.Text, temperature: 0.5, max_tokens: 60, top_p: 1.0, frequencyPenalty: 0.8, presencePenalty: 0)))
                    {
                        tbx_keywords.Text += token.ToString();
                    }

                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        //private void btnSettings_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        Settings settings = new Settings();
        //        settings.Owner = Window.GetWindow(this);


        //        if (settings.ShowDialog() == true)
        //        {

        //        }
        //    }
        //    catch (Exception ex) { throw ex; }
        //}

        private async void btnBrowse_Click(object sender, EventArgs e)
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
                    { DevicePlatform.macOS, new[] { "mp3", "mp4" } }, // UTType values
                });

                PickOptions options = new()
                {
                    PickerTitle = "Please select a media file",
                    FileTypes = customFileType,
                };

                var result = await FilePicker.Default.PickAsync(options);
                if (result != null)
                {
                    tbInfo.Text = result.FileName;
                }
            }
            catch (Exception ex) { throw ex; }
        }

        private void btnStart_Clicked(object sender, EventArgs e)
        {
            try
            {

            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
