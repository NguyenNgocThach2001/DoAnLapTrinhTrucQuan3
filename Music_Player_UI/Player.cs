using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.DirectX.AudioVideoPlayback;
using System.IO;

namespace Music_Player_UI
{
    public partial class Player : UserControl
    {
        internal bool FSCREEN = false;
        internal Video vid;
        internal string Status = "";
        //internal Size formSize;
        internal Size panelSize;
        public Player()
        {
            InitializeComponent();
        }

        // video double click
        public event EventHandler VideoDoubleClick;

        protected virtual void OnVideoDoubleClick(EventArgs e)
        {
            EventHandler handler = VideoDoubleClick;
            handler?.Invoke(this, e);
        }

        private void Video_DoubleClick(Object sender, EventArgs e)
        {
            OnVideoDoubleClick(e);
        }

        private void Player_Load(object sender, EventArgs e)
        {
            //formSize = new Size(this.Width, this.Height);
            panelSize = new Size(panel1.Width, panel1.Height);
        }
    }
}
