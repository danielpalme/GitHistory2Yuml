using System.IO;
using Microsoft.Win32;

namespace Palmmedia.GitHistory2Yuml.Wpf.Interaction
{
    public class FormFileAccess : IFileAccess
    {
        public string SelectDirectory(string initialDirectory)
        {
            using (var folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                folderBrowserDialog.SelectedPath = initialDirectory;
                folderBrowserDialog.ShowDialog();

                return folderBrowserDialog.SelectedPath;
            }
        }

        public void SaveFile(string extension, byte[] fileContent)
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = string.Format("{0} (*.{1})|*.{1}", extension.ToUpperInvariant(), extension);
            if (saveFileDialog.ShowDialog().GetValueOrDefault())
            {
                File.WriteAllBytes(saveFileDialog.FileName, fileContent);
            }
        }
    }
}
