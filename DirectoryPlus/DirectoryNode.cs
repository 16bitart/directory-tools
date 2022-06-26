using System.Text;

namespace DirectoryPlus;

public record DirectoryTree(string RootPath)
{
    public string RootPath { get; private set; } = RootPath;
    public DirectoryNode RootNode { get; private set; } = new(RootPath);
}

public record DirectoryNode : INodePrefix
{
    public string RootPath { get; private set; }
    public DirectoryInfo RootInfo { get; private set; }
    public string DirPath => RootInfo.FullName;
    public List<DirectoryNode> Children { get; private set; } = new();
    public List<FileNode> Files { get; private set; } = new();
    public string DisplayPrefix => "[D] ";

    public DirectoryNode(string rootPath)
    {
        RootPath = rootPath;
        RootInfo = new DirectoryInfo(rootPath);
        if (!RootInfo.Exists)
        {
            throw new FileNotFoundException($"No directory at this path. {rootPath}");
        }

        foreach (var subDir in RootInfo.EnumerateDirectories())
        {
            if (subDir.Exists)
            {
                Children.Add(new DirectoryNode(subDir.FullName));
            }
        }

        foreach (var file in RootInfo.EnumerateFiles())
        {
            var newFileNode = new FileNode(this, file);
            Files.Add(newFileNode);
            newFileNode.OnFileRemoved += OnFileDeleted;
        }
    }

    private void OnFileDeleted(FileNode node)
    {
        if (Files.Contains(node))
        {
            Files.Remove(node);
        }
    }

    public IEnumerable<DirectoryNode> GetChildren()
    {
        foreach (var directoryNode in Children)
        {
            yield return directoryNode;
        }
    }

    public IEnumerable<FileInfo> GetFiles()
    {
        return RootInfo.EnumerateFiles();
    }

    public async Task CreateFile(string fileName, byte[] fileBytes)
    {
        var fullFileName = Path.Combine(RootInfo.FullName, fileName);
        await using var file = File.Create(fullFileName);
        await file.WriteAsync(fileBytes);
        Files.Add(new FileNode(this, new FileInfo(fullFileName)));
    }

    public async Task<string> ToStringRecursive(int index = 0, int level = 0)
    {
        var initialIndex = index;
        var sb = new StringBuilder();
        foreach (var directoryNode in Children)
        {
            var next = await directoryNode.ToStringRecursive(index + 1, level + 1);
            var str = $"{next}".PadLeft(index, '>');
            sb.AppendLine(str.Insert(0, $"Index {index}:"));
        }
        return $"{Environment.NewLine}I:{index}L:{level}{ToString()}{sb}".PadLeft(initialIndex, '-');
    }

    public override string ToString()
    {
        return $"Node Root Path: {RootPath}{Environment.NewLine}" +
               $"Children Directory Nodes: {Children.Count}{Environment.NewLine}" +
               $"File Nodes: {Files.Count}";
    }
}