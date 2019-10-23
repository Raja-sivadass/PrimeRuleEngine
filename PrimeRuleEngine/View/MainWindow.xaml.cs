using PrimeRuleEngine.ViewModel;
using System;
using System.Windows;

namespace PrimeRuleEngine
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new PrimeEngineVM();
        }

        private void BrowseFile(object sender, RoutedEventArgs e)
        {
            try {
                var vmObj = (this.DataContext) as PrimeEngineVM;

                var fileDialog = new System.Windows.Forms.OpenFileDialog();
                var result = fileDialog.ShowDialog();
                switch (result)
                {
                    case System.Windows.Forms.DialogResult.OK:
                        var file = fileDialog.FileName;
                        txt_filePath.Text = file;
                        vmObj.InputFilePath = file;
                        txt_filePath.ToolTip = file;
                        break;
                    case System.Windows.Forms.DialogResult.Cancel:
                    default:
                        txt_filePath.Text = null;
                        vmObj.InputFilePath = string.Empty;
                        txt_filePath.ToolTip = null;
                        break;
                }
            }
            catch(Exception ex) { }
        }
    }
}
