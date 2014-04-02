using System.IO;
using System.Windows;
using System.Windows.Input;
using Palmmedia.GitHistory2Yuml.Wpf.Common;
using Palmmedia.GitHistory2Yuml.Wpf.Interaction;

namespace Palmmedia.GitHistory2Yuml.Wpf.ViewModels
{
    public class GraphViewModel : ViewModelBase
    {
        private readonly string name;

        private readonly IFileAccess fileAccess;

        private string yumlGraph;

        private byte[] yumlImage;

        public GraphViewModel(string name, IFileAccess fileAccess)
        {
            this.name = name;
            this.fileAccess = fileAccess;

            this.SaveImageCommand = new RelayCommand(o =>
            {
                this.fileAccess.SaveFile("png", this.YumlImage);
            });

            this.CopyYumlCommand = new RelayCommand(o =>
            {
                Clipboard.SetText(this.YumlGraph);
            });
        }

        public string YumlGraph
        {
            get { return this.yumlGraph; }
            set { this.SetProperty(ref this.yumlGraph, value); }
        }

        public byte[] YumlImage
        {
            get { return this.yumlImage; }
            set { this.SetProperty(ref this.yumlImage, value); }
        }

        public ICommand SaveImageCommand { get; private set; }

        public ICommand CopyYumlCommand { get; private set; }

        public override string ToString()
        {
            return this.name;
        }
    }
}
