
namespace Inspector_Gadget_Maui;

public partial class Settings : ContentPage
{

    /// <summary>
    /// The sr selected language.
    /// </summary>
    private static string srSelectedLanguage = "English";
    /// <summary>
    /// The sr selected model.
    /// </summary>
    private static string srSelectedModel = whisperModels.tiny.ToString();

    /// <summary>
    /// Initializes a new instance of the <see cref="Settings"/> class.
    /// </summary>
    public Settings()
    {
        InitializeComponent();

        FillCombos();
    }

    /// <summary>
    /// Fill combos.
    /// </summary>
    private void FillCombos()
    {
        try
        {
            foreach (var vrLanguage in Enum.GetValues(typeof(enLanguages)).Cast<enLanguages>())
            {
                cmbLanguages.Items.Add(vrLanguage.ToString());
            }
            foreach (var vrModel in Enum.GetValues(typeof(whisperModels)).Cast<whisperModels>())
            {
                cmbBoxModels.Items.Add(vrModel.processModelEnums());
            }

            cmbLanguages.SelectedItem = enLanguages.English;
            cmbBoxModels.SelectedItem = whisperModels.tiny.ToString();
        }
        catch
        {
            throw;
        }
    }

    private async void btnSelectFolder_Click(object sender, EventArgs e)
    {
        var customFileType = new FilePickerFileType(
               new Dictionary<DevicePlatform, IEnumerable<string>>
               {
                   // { DevicePlatform.iOS, new[] { "public.my.comic.extension" } }, // UTType values
                   // { DevicePlatform.Android, new[] { "application/comics" } }, // MIME type
                    { DevicePlatform.WinUI, new[] { ".exe" } }, // file extension
                    { DevicePlatform.Tizen, new[] { "*/*" } },
                    { DevicePlatform.macOS, new[] { "exe" } }, // UTType values
               });

        PickOptions options = new()
        {
            PickerTitle = "Please select a Whisper file",
            FileTypes = customFileType,
        };

        var result = await FilePicker.Default.PickAsync(options);
        if (result != null)
        {
            tbWhisperPathSelected.Text = result.FullPath;
        }
    }
}