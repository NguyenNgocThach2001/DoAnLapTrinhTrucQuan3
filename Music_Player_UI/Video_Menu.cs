using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Media.Animation;
using AxWMPLib;

namespace Music_Player_UI
{
    public partial class Video_Menu : UserControl
    {
        
        public Video_Menu()
        {
            InitializeComponent();
        }

        private void Menu_Load(object sender, EventArgs e)
        {
            Menu_Refresh();
        }

        private string splitFileName(string line)
        {
            int pos1 = -1;
            int pos2 = -1;
            bool done1 = false, done2 = false;
            for (int i = line.Length - 1; i >= 0; i--)
            {
                if (line[i] == '.' && !done1)
                {
                    pos1 = i;
                    done1 = true;
                }
                if (line[i] == '\\' && !done2)
                {
                    pos2 = i;
                    done2 = true;
                }
                if (done1 && done2) break;
            }
            if(done1 && done2)
                line = line.Substring(pos2 + 1, pos1 - pos2 - 1);
            return line;
        }
        internal void Menu_Refresh()
        {
            string curFile = @"C:\Users\ACER\Documents\Save Video\Meow.txt";
            if (File.Exists(curFile))
            {
                List<string> myList = new List<string>();
                string[] lines = System.IO.File.ReadAllLines(@"C:\Users\ACER\Documents\Save Video\Meow.txt");
                foreach (string line in lines)
                    myList.Add(line);
                myList = myList.Distinct().ToList();
                TextWriter tsw = new StreamWriter(@"C:\Users\ACER\Documents\Save Video\Meow.txt");
                listView1.Clear();
                foreach (string line in myList)
                {
                    listView1.Items.Add((line));
                    //listView1.Items.Add((splitFileName(line)));
                    tsw.WriteLine(line);
                }
                tsw.Close();
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
        
        }

        public event EventHandler ItemDoubleClick;

        protected virtual void ITEM_DoubleClick(EventArgs e)
        {
            EventHandler handler = ItemDoubleClick;
            handler?.Invoke(this, e);
            //MessageBox.Show(Form1.Path);
        }
        private void Item_DoubleClick(Object sender, EventArgs e)
        {
            Form1.selectPath = listView1.SelectedItems[0].Text;
            Form1.nowVideoSelectIndex = listView1.FocusedItem.Index;
            ITEM_DoubleClick(e);
        }   
    }
}
