using DirectoryPlus;

namespace QuickDirFilter.Application
{
    public partial class Form1 : Form
    {
        private DirectoryTree _directoryTree;
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            var path = textBox1.Text;
            if (!Directory.Exists(path)) return;
            if(_directoryTree.RootPath != path) _directoryTree = new DirectoryTree(path);
        }

        private void textBox1_MouseClick(object sender, MouseEventArgs e)
        {
            var clickedPath = folderBrowserDialog1.ShowDialog();
            if (clickedPath == DialogResult.OK)
            {
                textBox1.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void ShowItems()
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            listView1.Items.Add(textBox2.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}