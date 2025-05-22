using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XmlFileEngine;
using System.IO;
using System.Threading;

namespace textEditor
{
    public partial class Editor : Form
    {
        private static Commands xmlCommands;
        private static string path = "";

        public Editor()
        {
            InitializeComponent();
        }

        private void Editor_Load(object sender, EventArgs e)
        {
            string[] args = Environment.GetCommandLineArgs();

            if (args.Length > 1)
            {
                xmlCommands = new Commands(args[1].Split('\n')[0]);
                path = args[1].Split('\n')[1];

                WaitTillFileIsReady(xmlCommands.BasePath);

                richTextBox.Text = xmlCommands.GetFileAttribute(path, "text");

                Text = "Editor | " + xmlCommands.BasePath + " | " + path;
            }
        }

        private void save() {
            WaitTillFileIsReady(xmlCommands.BasePath);

            xmlCommands.ChangeFileAttribute(path, "text", richTextBox.Text);
        }

        public static void WaitTillFileIsReady(string filename)
        {
            while (!IsFileReady(filename))
            {
                Thread.Sleep(30);
            }
        }
        public static bool IsFileReady(string filename)
        {
            try
            {
                using (Stream stream = new FileStream(filename, FileMode.Open))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        private void richTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S && e.Modifiers == Keys.Control)
            {
                save();
            }
        }
    }
}
