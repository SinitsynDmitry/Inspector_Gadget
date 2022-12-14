using Inspector_Gadget_Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using OpenAI_API;
using System.Diagnostics;
using Path = System.IO.Path;

namespace Inspector_Gadget_Maui;

public partial class TranscriptionPage : ContentPage
{
    int count = 0;
    private static string whisperLanguage = "English";
    private static string whisperModel = whisperModels.tiny.ToString();
    private static long irCmdFileNameCounter = 0;

    private Process whisper = null;

    private string filePath = "";

    /// <summary>
    /// detect first load (player fix)
    /// </summary>
    private bool isFirstLoad = true;

    public TranscriptionPage(string filePath)
    {
        try
        {
            InitializeComponent();

            if (!string.IsNullOrWhiteSpace(filePath))
            {
                this.filePath = filePath.Trim();

              //  rfTranscription.IsRefreshing= true;

                StartWhisper(filePath);
            }
            else
            {
                btnProcessText.IsEnabled = true;
            }

        }
        catch (Exception ex) { throw ex; }
    }

    private async void StartWhisper(string filePath)
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
                string srCommand = @$"cmd /C C:\Phyton399\Scripts\whisper ""{filePath}"" --language {whisperLanguage} --model {whisperModel} --device cpu --task transcribe";
                string srCommandName = Directory.GetCurrentDirectory() + $"/commands/cmd_{Interlocked.Read(ref irCmdFileNameCounter)}.cmd";

                Interlocked.Add(ref irCmdFileNameCounter, 1);

                File.WriteAllText(srCommandName, srCommand); //"commands/cmd_0.cmd"

                phraseObjs.Clear();

                await Task.Run(async () =>
                {
                    StartWhisperProcess(srCommandName);
                });
            }
        }
        catch (Exception ex) { throw ex; }
    }

    public List<PhraseObj> phraseObjs { get; private set; } = new List<PhraseObj>();

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
            whisper.Exited += Whisper_Exited;

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
                       // phraseObjs.Add(new PhraseObj(line));
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

    private void Whisper_Exited(object sender, EventArgs e)
    {
        try
        {
            Application.Current.Dispatcher.Dispatch(() =>
            {
                btnProcessText.IsEnabled = true;

              //  rfTranscription.IsRefreshing = false;

                if (!string.IsNullOrWhiteSpace(filePath))
                {
                    video.Source = new FileVideoSource
                    {
                        File = filePath
                    };

                    //if (isFirstLoad)
                    //{
                    //    isFirstLoad = false;
                    //    grdVideo.HeightRequest = video.Height * 3;
                    //    grdVideo.MaximumHeightRequest = grdVideo.HeightRequest;
                    //}
                }

            });

        }
        catch (Exception ex)
        {
            throw;
        }
    }


    private readonly object balanceLock = new object();
    private async void BtnProcessText_Clicked(object sender, EventArgs e)
    {
        try
        {
            //Your secret API key from https://beta.openai.com/account/api-keys

            var api = new OpenAIAPI("secret_API_key", new Engine("text-davinci-003"));
            //tldr

            if (!string.IsNullOrEmpty(tbx_input.Text))
            {
                string[] lines = tbx_input.Text.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

                var max_tokens = lines.Length / 10;

                if(max_tokens < 60)
                {
                    max_tokens = 60;
                }
                if (max_tokens > 4000)
                {
                    max_tokens = 3999;
                }

                for (int i = 0; i < lines.Length; i++)
                {
                    var line = lines[i];
                    phraseObjs.Add(new PhraseObj(line,i));
                }

                tbx_output.Text = "";

               
                await foreach (var token in api.Completions.StreamCompletionEnumerableAsync(new CompletionRequest(tbx_input.Text + " tl;dr:", temperature: 0.7, max_tokens: max_tokens, top_p: 1.0, frequencyPenalty: 0.0, presencePenalty: 1)))
                {
                    tbx_output.Text += token.ToString();
                }

                //keywords

                tbx_keywords.Text = "";


                await foreach (var token in api.Completions.StreamCompletionEnumerableAsync(new CompletionRequest("Extract keywords from this text:\n\n" + tbx_input.Text+ "\n\n", temperature: 0.5, max_tokens: max_tokens, top_p: 1.0, frequencyPenalty: 0.8, presencePenalty: 0)))
                {
                    tbx_keywords.Text += token.ToString();
                }

                //esbr

                tbx_esrb.Text = "";

             
                var stop = new string[1] { "\n" };

                for (int i = 0; i < 10; i++)
                {
                    string line = "";
                    var multip = lines.Length / 10;

                    if (multip == 0)
                    {
                        multip = 1;
                    }

                    var position = i * multip;
                    var positionEnd = (i + 1) * multip;
                    if (positionEnd > lines.Length)
                    {
                        positionEnd = lines.Length;
                    }

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
        catch (Exception ex) { throw ex; }
    }

    private void btnSearch_Clicked(object sender, EventArgs e)
    {

        try
        {


        }
        catch (Exception ex)
        {
            throw;
        }
    }

    private void tbx_input_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {

           
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}