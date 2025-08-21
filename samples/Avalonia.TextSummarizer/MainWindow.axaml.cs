using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using Mscc.GenerativeAI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Avalonia.TextSummarizer
{
    public partial class MainWindow : Window
    {
        private string apiKey = Environment.GetEnvironmentVariable("GOOGLE_API_KEY") ?? string.Empty;
        private string modelName = Model.GeminiPro; // Use Gemini model
        private GenerativeModel model;

        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            var googleAI = new GoogleAI(apiKey: apiKey);
            model = googleAI.GenerativeModel(model: modelName);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private async void LoadFileButton_Click(object sender, RoutedEventArgs e)
        {
            var storageProvider = ((Window)this).StorageProvider; // Get the StorageProvider

            if (storageProvider != null)
            {
                // var fileTypes = new[] { "txt" }; // Filter for text files
                // var result = await storageProvider.OpenFilePickerAsync(
                //     new FilePickerOpenOptions
                //     {
                //         Title = "Open Text File",
                //         AllowMultiple = false,
                //         // FileTypeFilter = fileTypes
                //     });
                //
                // if (result != null)
                // {
                //     InputText.Text = await File.ReadAllTextAsync(result[0]);
                // }
                var files = await storageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
                {
                    Title = "Open Text File",
                    AllowMultiple = false
                });
                
                if (files.Count >= 1)
                {
                    // Open reading stream from the first file.
                    await using var stream = await files[0].OpenReadAsync();
                    using var streamReader = new StreamReader(stream);
                    // Reads all the content of file as a text.
                    InputText.Text = await streamReader.ReadToEndAsync();
                }
            }
        }

        private async void SummarizeButton_Click(object sender, RoutedEventArgs e)
        {
            string textToSummarize = string.IsNullOrEmpty(TextInput?.Text) ? InputText?.Text ?? string.Empty : TextInput?.Text ?? string.Empty;

            if (!string.IsNullOrEmpty(textToSummarize))
            {
                try
                {
                    var response = await model.GenerateContent(textToSummarize);
                    SummaryOutput.Text = response.Text;
                }
                catch (Exception ex)
                {
                    SummaryOutput.Text = "Error: " + ex.Message;
                }
            }
        }
    }
}