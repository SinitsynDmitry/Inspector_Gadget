using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Inspector_Gadget
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
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
                    cmbLanguages.Items.Add(vrLanguage);
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

        private void btnSelectFolder_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
