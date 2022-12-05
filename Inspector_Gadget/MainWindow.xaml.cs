using OpenAI_API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Inspector_Gadget
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
           // OpenAI_API.APIAuthentication.Default = new OpenAI_API.APIAuthentication(Environment.GetEnvironmentVariable("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"));
        }

        private async void btn_submit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(tbx_input.Text))
                {
                    tbx_output.Text = "";

                    var api = new OpenAIAPI("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");

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
    }
}
