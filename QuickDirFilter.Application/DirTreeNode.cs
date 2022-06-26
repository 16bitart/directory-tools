using DirectoryPlus;

namespace QuickDirFilter.Application;

public class DirTreeNode<T> : TreeNode where T : INodePrefix
{
    /// <summary>
    /// Invokes an action with the old value, and new value as parameters.
    /// </summary>
    public Action<string, string> OnTextChange;
    public string Identifier { get; private set; }
    public T OrgNode { get; private set; }
    public new string Text
    {
        get => base.Text;
        set => UpdateName(value);
    }

    public DirTreeNode(string text, T node) : base(text)
    {
        Identifier = text;
        OrgNode = node;
    }

    public void UpdateName(string newName)
    {
        OnTextChange?.Invoke(Text, newName);
        base.Text = $@"{OrgNode.DisplayPrefix} {newName}";
    }
}