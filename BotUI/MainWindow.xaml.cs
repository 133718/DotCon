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
using System.Windows.Navigation;
using System.Windows.Shapes;
using DotBot;
using Serilog;
using Serilog.Sinks.RichTextBox;
using Serilog.Sinks.File;
using System.ComponentModel;

namespace BotUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        readonly SgoBot bot = new();

        public MainWindow()
        {
            InitializeComponent();
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.RichTextBox(OutputBox)
                .WriteTo.File("log.txt")
                .CreateLogger();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            _ = bot.RunAsync();
            TaskBar.Background = Resources["TaskBarStartedBrush"] as Brush;
            TaskBarLabel.Content = "Started";
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            _ = bot.StopAsync();
            TaskBar.Background = Resources["TaskBarStopedBrush"] as Brush;
            TaskBarLabel.Content = "Stoped";
        }

        private void ClosingEvent(object sender, CancelEventArgs e)
        {
        }

        private void ClosedEvent(object sender, EventArgs e)
        {
            if (bot.Connected)
            {
                bot.StopAsync().Wait();
            }
            Log.CloseAndFlush();
        }

        private void OutputBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            OutputBox.ScrollToEnd();
        }
    }
}
