using DirectoryPlus;

namespace QuickDirFilter.Application
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            System.Windows.Forms.Application.Run(new Form1());
        }
    }
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
}