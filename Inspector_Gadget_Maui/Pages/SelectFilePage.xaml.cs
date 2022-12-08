namespace Inspector_Gadget_Maui;

public partial class SelectFilePage : ContentPage
{

    public SelectFilePage()
	{
        Application.Current.UserAppTheme = AppTheme.Light;
		InitializeComponent();
	}

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
                entFilePath.Text = result.FullPath;
            }
        }
        catch (Exception ex) { throw ex; }
    }

    private async void BtnTranscribe_Clicked(object sender, EventArgs e)
    {
        try
        {
            await Navigation.PushAsync(new TranscriptionPage(entFilePath.Text));
        }
        catch (Exception ex) { throw ex; }
    }
}