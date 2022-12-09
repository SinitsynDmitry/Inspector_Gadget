
using Microsoft.Maui.Controls.Shapes;
using OpenAI_API;
using System.Diagnostics;
using System.Security.AccessControl;
using Path = System.IO.Path;

namespace Inspector_Gadget_Maui
{
    public partial class MainPage : ContentPage
    {
        int count = 0;
        private static string whisperLanguage = "English";
        private static string whisperModel = whisperModels.tiny.ToString();
        private static long irCmdFileNameCounter = 0;

        private Process whisper = null;

        public MainPage()
        {
            InitializeComponent();

            //var test = new PhraseObj("[01:13:13.460 --> 01:13:15.460]  I don't think anybody will look at that video.");
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

        private async void btnSettings_Click(object sender, EventArgs e)
        {
            try
            {
                //Settings settings = new Settings();
                //settings.Owner = Window.GetWindow(this);


                //if (settings.ShowDialog() == true)
                //{

                //}
                await Navigation.PushAsync(new Settings());

            }
            catch (Exception ex) { throw ex; }
        }

        private async void btnSelectFilePage_Click(object sender, EventArgs e)
        {
            try
            {

                await Navigation.PushAsync(new SelectFilePage());
            }
            catch (Exception ex) { throw ex; }
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

        private readonly object balanceLock = new object();

        private async void btn_esrb_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(tbx_input.Text))
                {
                    tbx_esrb.Text = "";

                    var api = new OpenAIAPI("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", new Engine("text-davinci-003"));

                    string[] lines = tbx_input.Text.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

                    var stop = new string[1] { "\n" };

                    for (int i = 0; i < 10; i++)
                    {
                        string line = "";
                        var multip = lines.Length / 10;
                        var position = i * multip;
                        var positionEnd = (i+1) * multip;
                        var prefix = $"{position}-{positionEnd}: ";

                        for (int j = position; j < positionEnd && j < lines.Length; j++)
                        {
                            line += lines[j];
                        }
                        lock (balanceLock)
                        {
                            tbx_esrb.Text += prefix;
                        }

                        await foreach (var token in api.Completions.StreamCompletionEnumerableAsync(new CompletionRequest("Provide an ESRB rating for the following text:\n\n" + line + "\n\nESRB rating:\"", temperature: 0.3, max_tokens: 60, top_p: 1.0, frequencyPenalty: 0.0, presencePenalty: 0, stopSequences: stop)))
                        {
                            lock (balanceLock)
                            {
                                tbx_esrb.Text += $"{token.ToString()}";
                            }
                        }

                        lock (balanceLock)
                        {
                            tbx_esrb.Text += Environment.NewLine;
                        }
                    }

                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

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
                    { DevicePlatform.MacCatalyst, new[] { "public.video", "public.audio" } }, // UTType values
                });

                PickOptions options = new()
                {
                    PickerTitle = "Please select a media file",
                    FileTypes = customFileType,
                };

                var result = await FilePicker.Default.PickAsync(options);
                if (result != null)
                {
                    tbInfo.Text = result.FullPath;
                }
            }
            catch (Exception ex) { throw ex; }
        }

        private async void btnStart_Clicked(object sender, EventArgs e)
        {
            try
            {

                if (whisper != null)
                {
                    whisper.Kill();
                }
                else
                {

                    Directory.SetCurrentDirectory(Path.GetDirectoryName(Environment.ProcessPath));

                    if (!Directory.Exists(Path.Combine(Path.GetDirectoryName(Environment.ProcessPath), "commands")))
                    {
                        Directory.CreateDirectory(Path.Combine(Path.GetDirectoryName(Environment.ProcessPath), "commands"));
                    }
                    string srCommand = @$"cmd /C C:\Phyton399\Scripts\whisper ""{tbInfo.Text}"" --language {whisperLanguage} --model {whisperModel} --device cpu --task transcribe";
                    string srCommandName = Directory.GetCurrentDirectory() + $"/commands/cmd_{Interlocked.Read(ref irCmdFileNameCounter)}.cmd";

                    Interlocked.Add(ref irCmdFileNameCounter, 1);

                    File.WriteAllText(srCommandName, srCommand); //"commands/cmd_0.cmd"

                    await Task.Run(async () =>
                    {
                        StartWhisperProcess(srCommandName);
                    });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void StartWhisperProcess(string srCommandName)
        {
            try
            {
                ProcessStartInfo psInfo = new ProcessStartInfo(srCommandName);

                whisper = new Process();
                whisper.StartInfo = psInfo;
                whisper.EnableRaisingEvents = true;
                whisper.StartInfo.UseShellExecute = false;
                whisper.StartInfo.CreateNoWindow = true;
                whisper.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                whisper.StartInfo.RedirectStandardOutput = true;

                whisper.Start();

                while (!whisper.StandardOutput.EndOfStream)
                {
                    string line = whisper.StandardOutput.ReadLine();
                    Debug.WriteLine(line);
                    Application.Current.Dispatcher.Dispatch(() =>
                    {
                        if (!string.IsNullOrWhiteSpace(line) && line.StartsWith("["))
                        {

                            tbx_input.Text += line + Environment.NewLine;
                        }
                    });
                }

                // Debug.WriteLine(p.StandardOutput.ReadToEnd());
                whisper.WaitForExit();

            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (whisper != null)
                {
                    whisper.Kill();
                }
                whisper = null;
            }
        }


    }
}
