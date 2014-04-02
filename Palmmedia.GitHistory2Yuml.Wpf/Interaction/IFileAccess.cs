namespace Palmmedia.GitHistory2Yuml.Wpf.Interaction
{
    public interface IFileAccess
    {
        string SelectDirectory(string initialDirectory);

        void SaveFile(string extension, byte[] fileContent);
    }
}
