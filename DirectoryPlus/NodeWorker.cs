namespace DirectoryPlus;

public class NodeWorker
{
    public event Action Executed;
    public FileNode FileNode { get; private set; }
    public Action OnWorkerExecute;

    public NodeWorker(FileNode fileNode, Action onWorkerExecute)
    {
        FileNode = fileNode;
        OnWorkerExecute = onWorkerExecute;
    }
    public async Task ExecuteAsync()
    {
        await Task.FromResult(() => OnWorkerExecute?.Invoke());
        Executed?.Invoke();
    }

    public void Execute()
    {
        OnWorkerExecute?.Invoke();
        Executed?.Invoke();
    }
}

public class StringTransformFunction
{
    public Func<string, string> Operation { get; private set; }
    public StringTransformFunction(Func<string, string> operation)
    {
        Operation = operation;
    }
    public async Task<string> Run(string input)
    {
        return await Task.FromResult(Operation.Invoke(input));
    }
}