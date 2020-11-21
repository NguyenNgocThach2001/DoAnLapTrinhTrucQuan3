using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AxWMPLib;
using System.IO;

namespace Music_Player_UI
{
    public partial class Music_Menu : UserControl
    {
        public Music_Menu()
        {
            InitializeComponent();
        }
        public event EventHandler ItemDoubleClick;

        protected virtual void ITEM_DoubleClick(EventArgs e)
        {
            EventHandler handler = ItemDoubleClick;
            handler?.Invoke(this, e);
        }
        private void Item_DoubleClick(Object sender, EventArgs e)
        {
            ITEM_DoubleClick(e);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           

        }
    }
}
