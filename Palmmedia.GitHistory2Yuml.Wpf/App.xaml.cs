using System;
using System.IO;
using System.Windows;
using Palmmedia.GitHistory2Yuml.Core;
using Palmmedia.GitHistory2Yuml.Wpf.Interaction;
using Palmmedia.GitHistory2Yuml.Wpf.ViewModels;

namespace Palmmedia.GitHistory2Yuml.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            string[] args = Environment.GetCommandLineArgs();
            if (args != null && args.Length > 1)
            {
                this.CreateGraphs(args[1]);
            }
            else
            {
                base.OnStartup(e);

                var mainViewModel = new MainViewModel(
                    new YumlGraphBuilder(),
                    new YumlGraphRenderer(),
                    new FormFileAccess());

                var mainWindow = new MainWindow();
                mainWindow.DataContext = mainViewModel;

                this.MainWindow = mainWindow;
                this.MainWindow.Show();
            }
        }

        private async void CreateGraphs(string directory)
        {
            try
            {
                string directoryName = new DirectoryInfo(directory).Name;

                IYumlGraphBuilder builder = new YumlGraphBuilder();
                string yumlDiagramm = builder.CreateYumlGraph(directory);
                string mergedYumlDiagramm = builder.CreateMergedYumlGraph(directory);

                IYumlGraphRenderer renderer = new YumlGraphRenderer();
                byte[] yumlGraphImage = await renderer.RenderYumlGraphAync(yumlDiagramm);
                File.WriteAllBytes(directoryName + "_full.png", yumlGraphImage);

                byte[] mergedYumlGraphImage = await renderer.RenderYumlGraphAync(mergedYumlDiagramm);
                File.WriteAllBytes(directoryName + "_merged.png", mergedYumlGraphImage);

                this.Shutdown();
            }
            catch (Exception)
            {
                this.Shutdown(1);
            }
        }
    }
}
