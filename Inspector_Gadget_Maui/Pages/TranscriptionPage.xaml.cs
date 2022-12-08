using System.Diagnostics;
using System.Linq.Expressions;
using Inspector_Gadget_Maui.Controls;
using OpenAI_API;

namespace Inspector_Gadget_Maui;

public partial class TranscriptionPage : ContentPage
{
    int count = 0;
    private static string whisperLanguage = "English";
    private static string whisperModel = whisperModels.tiny.ToString();
    private static long irCmdFileNameCounter = 0;

    private Process whisper = null;

    private string filePath = "";

    public TranscriptionPage(string filePath)
	{
		try
		{
			InitializeComponent();

           this.filePath= filePath.Trim();

            StartWhisper(filePath);
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

                await Task.Run(async () =>
                {
                    StartWhisperProcess(srCommandName);
                });
            }
        }
        catch (Exception ex) { throw ex; }
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


                if (!string.IsNullOrWhiteSpace(filePath))
                {
                    video.Source = new FileVideoSource
                    {
                        File = filePath
                    };

                }

            });
           
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    private async void BtnProcessText_Clicked(object sender, EventArgs e)
    {
		try
		{
            //tldr

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
                //empty catch
            }

            //keywords
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

               //empty catch
            }

            //esbr
            try
            {
                if (!string.IsNullOrEmpty(tbx_input.Text))
                {
                    tbx_esrb.Text = "";

                    var api = new OpenAIAPI("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", new Engine("text-davinci-003"));

                    var stop = new string[1] { "\n" };

                    await foreach (var token in api.Completions.StreamCompletionEnumerableAsync(new CompletionRequest("Provide an ESRB rating for the following text:\n\n" + tbx_input.Text + "\n\nESRB rating:\"", temperature: 0.3, max_tokens: 60, top_p: 1.0, frequencyPenalty: 0.0, presencePenalty: 0, stopSequences: stop)))
                    {
                        tbx_esrb.Text += token.ToString();
                    }

                }
            }
            catch (Exception ex)
            {

                //empty catch
            }
        }
		catch(Exception ex) { throw ex; }
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
}