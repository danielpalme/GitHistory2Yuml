using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Palmmedia.GitHistory2Yuml.Core;
using Palmmedia.GitHistory2Yuml.Wpf.Common;
using Palmmedia.GitHistory2Yuml.Wpf.Interaction;

namespace Palmmedia.GitHistory2Yuml.Wpf.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IYumlGraphBuilder yumlGraphBuilder;

        private readonly IYumlGraphRenderer yumlGraphRenderer;

        private readonly IFileAccess fileAccess;

        private string directory, error;

        private GraphViewModel selectedGraph;

        public MainViewModel(
            IYumlGraphBuilder yumlGraphBuilder,
            IYumlGraphRenderer yumlGraphRenderer,
            IFileAccess fileAccess)
        {
            this.yumlGraphBuilder = yumlGraphBuilder;
            this.yumlGraphRenderer = yumlGraphRenderer;
            this.fileAccess = fileAccess;

            this.Graphs = new ObservableCollection<GraphViewModel>();

            this.SelectDirectoryCommand = new RelayCommand(o =>
            {
                this.Directory = this.fileAccess.SelectDirectory(this.Directory);
                this.UpdateGraph();
            });
        }

        public string Directory
        {
            get { return this.directory; }
            set { this.SetProperty(ref this.directory, value); }
        }

        public string Error
        {
            get { return this.error; }
            set { this.SetProperty(ref this.error, value); }
        }

        public ObservableCollection<GraphViewModel> Graphs { get; private set; }

        public GraphViewModel SelectedGraph
        {
            get { return this.selectedGraph; }
            set { this.SetProperty(ref this.selectedGraph, value); }
        }

        public ICommand SelectDirectoryCommand { get; private set; }

        private void UpdateGraph()
        {
            this.Graphs.Clear();
            this.Error = null;

            if (this.Directory == null
                || !System.IO.Directory.Exists(this.Directory)
                || !System.IO.Directory.Exists(System.IO.Path.Combine(this.Directory, ".git")))
            {
                return;
            }

            Task<string>.Factory.StartNew(() =>
            {
                return this.yumlGraphBuilder.CreateYumlGraph(this.Directory);
            })
            .ContinueWith(
                async t =>
                {
                    GraphViewModel graph = new GraphViewModel("Full Graph", this.fileAccess);
                    graph.YumlGraph = t.Result;
                    try
                    {
                        graph.YumlImage = await this.yumlGraphRenderer.RenderYumlGraphAync(t.Result);
                    }
                    catch (System.Exception ex)
                    {
                        this.Error = "Failed to create graph: " + ex.Message;
                    }

                    this.Graphs.Add(graph);
                },
                System.Threading.CancellationToken.None,
                TaskContinuationOptions.NotOnFaulted,
                TaskScheduler.FromCurrentSynchronizationContext());

            var mergedYumlGraphTask = Task<string>.Factory.StartNew(() =>
            {
                return this.yumlGraphBuilder.CreateMergedYumlGraph(this.Directory);
            })
            .ContinueWith(
                async t =>
                {
                    GraphViewModel graph = new GraphViewModel("Merged Graph", this.fileAccess);
                    graph.YumlGraph = t.Result;
                    graph.YumlImage = await this.yumlGraphRenderer.RenderYumlGraphAync(t.Result);

                    this.Graphs.Add(graph);
                },
                System.Threading.CancellationToken.None,
                TaskContinuationOptions.NotOnFaulted,
                TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}
