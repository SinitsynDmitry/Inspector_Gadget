using System.Diagnostics;
using System.Linq.Expressions;
using Inspector_Gadget_Maui.Controls;

namespace Inspector_Gadget_Maui;

public partial class TranscriptionPage : ContentPage
{
    int count = 0;
    private static string whisperLanguage = "English";
    private static string whisperModel = whisperModels.tiny.ToString();
    private static long irCmdFileNameCounter = 0;

    private Process whisper = null;

    public TranscriptionPage(string filePath)
	{
		try
		{
			InitializeComponent();

            if (!string.IsNullOrWhiteSpace(filePath))
            {
                video.Source = new FileVideoSource
                {
                    File = filePath
                };

                video.Pause();
            }


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

    private void BtnProcessText_Clicked(object sender, EventArgs e)
    {
		try
		{

		}
		catch(Exception ex) { throw ex; }
    }
}