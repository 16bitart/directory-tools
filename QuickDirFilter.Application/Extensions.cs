using DirectoryPlus;

namespace QuickDirFilter.Application;

public static class Extensions
{
    public static void Add(this ListBox lb, string item)
    {
        lb.Items.Add(item);
    }
    public static TreeNode ToTreeNode(this DirectoryNode n)
    {
        var thisNode = new DirTreeNode<DirectoryNode>($"[D]{n.RootInfo.Name}", n);
        foreach (var item in n.Children)
        {
            thisNode.Nodes.Add(item.ToTreeNode());
        }

        foreach (var f in n.Files)
        {
            thisNode.Nodes.Add(f.ToTreeNode());
        }

        return thisNode;
    }

    public static TreeNode ToTreeNode(this FileNode n)
    {
        return new DirTreeNode<FileNode>($"[F]{n.FileName}", n);
    }
}