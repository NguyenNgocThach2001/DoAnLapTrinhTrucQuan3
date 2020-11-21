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
            this.AllowDrop = true;
        }

        internal string getFileName(string line)
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
        internal void Menu_Refresh(string filter = "")
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            path = System.IO.Path.Combine(path, "Save Video");
            var imgPath = path;
            path = System.IO.Path.Combine(path, "Meow.txt");
            ImageList imageList = new ImageList();
            imageList.ImageSize = new Size(140, 100); // size cua thumbnail
            if (File.Exists(path))
            {
                List<string> myList = new List<string>();
                string[] lines = System.IO.File.ReadAllLines(path);
                foreach (string line in lines)
                    myList.Add(line);
                myList = myList.Distinct().ToList();
                TextWriter tsw = new StreamWriter(path);
                listView1.Clear();
                // Đưa video đã lưu vào listView
                int cntVideo = -1;
                for (int i = 0; i < myList.Count(); i++)
                {
                    string vidPath = myList[i];
                    string vidName = getFileName(vidPath);
                    if (filter == "" || (filter != "" && vidName.Contains(filter)))
                    {
                        cntVideo++;
                        var item = new ListViewItem(new[] { vidName, vidPath });
                        var tmpPath = System.IO.Path.Combine(imgPath, vidName + ".bmp");
                        //MessageBox.Show(tmpPath);
                        listView1.Items.Add((item));
                        listView1.Items[cntVideo].ImageIndex = cntVideo;
                        Image img = Image.FromFile(tmpPath);
                        imageList.Images.Add(img);
                    }
                    tsw.WriteLine(vidPath);
                }
                tsw.Close();
                listView1.View = View.LargeIcon;
                listView1.LargeImageList = imageList;
            }
        }
        public static Image resizeImage(Image imgToResize, Size size)
        {
            return (Image)(new Bitmap(imgToResize, size));
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
            Form1.selectPath = listView1.SelectedItems[0].SubItems[1].Text;
            Form1.nowVideoSelectIndex = listView1.FocusedItem.Index;
            ITEM_DoubleClick(e);
        }   
    }
}
