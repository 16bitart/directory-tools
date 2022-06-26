namespace DirectoryPlus
{
    public class FileNode : INodePrefix
    {
        public event Action<FileNode> OnFileRemoved;
        public DirectoryNode ParentDirectoryNode { get; private set; }
        public FileInfo FileInfo { get; private set; }
        public string FileName => FileInfo.Name;
        public string FileExtension => FileInfo.Extension;
        public long FileSize => FileInfo.Length;
        public string DisplayPrefix => "[F] ";
        public FileNode(DirectoryNode parentDirectoryNode, FileInfo fileInfo)
        {
            ParentDirectoryNode = parentDirectoryNode;
            FileInfo = fileInfo;
        }

        public async Task CopyTo(DirectoryNode target, Func<string, string> renameFunc)
        {
            var name = renameFunc(FileName);
            await CopyTo(target, name, await GetBytesAsync());
        }

        public async Task CopyTo(DirectoryNode target, bool deleteAfterCopy = false)
        {
            await CopyTo(target, FileName, await GetBytesAsync());
        }
        protected async Task CopyTo(DirectoryNode target, string fileName, byte[] bytes, bool deleteAfterCopy = false)
        {
            await target.CreateFile(fileName, bytes);
            if (deleteAfterCopy)
            {
                FileInfo.Delete();
                OnFileRemoved?.Invoke(this);
            }
        }

        public byte[] GetBytes()
        {
            return File.ReadAllBytes(FileInfo.FullName);
        }

        public async Task<byte[]> GetBytesAsync()
        {
            return await File.ReadAllBytesAsync(FileInfo.FullName);
        }

        public string ReadText()
        {
            return File.ReadAllText(FileInfo.FullName);
        }

        public async Task<string> ReadTextAsync()
        {
            return await File.ReadAllTextAsync(FileInfo.FullName);
        }
    }
}