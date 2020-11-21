using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using Microsoft.DirectX.AudioVideoPlayback;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Windows.Threading;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Music_Player_UI
{

    public partial class Form1 : Form
    {
        internal String[] paths, files;
        public static bool Loop = false;
        public static string playPath = "";
        public static string selectPath = "";
        public static int nowVideoSelectIndex = -1;
        public static int nowVideoPlayIndex = -1;
        public static bool Now__Video_Mode = true;
        public static bool Now_Music_Mode = false;
        public static bool Menu_Now_On_Top = true;
        public static bool NowPlaying_Now_On_Top = false;
        public static bool RecentViewed_Now_On_Top = false;
        public static bool RecentPlayed_Now_On_Top = false;
        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        DispatcherTimer timer = new DispatcherTimer();
        public Form1()
        {
            InitializeComponent();
            this.AllowDrop = true;
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            if (Now__Video_Mode)
            {
                if (player1.Status == "Play")
                {
                    int currentTime = Convert.ToInt32(player1.vid.CurrentPosition);
                    int videoLength = Convert.ToInt32(player1.vid.Duration);
                    bunifuCustomLabel4.Text = TimeSpan.FromSeconds(currentTime).ToString();
                    bunifuCustomLabel5.Text = TimeSpan.FromSeconds(videoLength - currentTime).ToString();
                    bunifuSlider3.MaximumValue = videoLength;
                    bunifuSlider3.Value = currentTime;
                    if (videoLength - currentTime == 0)
                    {
                        if (!Loop)
                        {
                            if (Ahead()) c_Ahead();
                            else c_Back();
                        }
                        player1.vid.CurrentPosition = 0;
                    }
                }
            }
            else
            {
                if (axWindowsMediaPlayer1.currentMedia != null && axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsPlaying)
                {
                    bunifuSlider3.Value = (int)axWindowsMediaPlayer1.Ctlcontrols.currentPosition;
                    bunifuSlider3.MaximumValue = (int)axWindowsMediaPlayer1.currentMedia.duration;
                    bunifuCustomLabel4.Text = axWindowsMediaPlayer1.Ctlcontrols.currentPositionString;
                    bunifuCustomLabel5.Text = axWindowsMediaPlayer1.currentMedia.durationString;
                }
            }

        }

        private void init()
        {
            Loop = false;
            trackBar2.Value = trackBar2.Maximum;
            playPath = "";
            selectPath = "";
            playPath = "";
            selectPath = "";
            nowVideoSelectIndex = -1;
            nowVideoPlayIndex = -1;
            Now__Video_Mode = true;
            Now_Music_Mode = false;
            Menu_Now_On_Top = true;
            NowPlaying_Now_On_Top = false;
            RecentViewed_Now_On_Top = false;
            RecentPlayed_Now_On_Top = false;
            player1.Status = "";
            player1.FSCREEN = false;
            bunifuSlider3.Value = 0;
            bunifuCustomLabel4.Text = "00:00:00";
            bunifuCustomLabel5.Text = "00:00:00";
            bunifuCustomLabel2.Text = "Name of the song";
        }
        // click on slider
        private void bunifuSlider3_ValueChanged(object sender, EventArgs e)
        {
            if (Now_Music_Mode)
            {
                // bunifuSlider3.Value = (int)axWindowsMediaPlayer1.Ctlcontrols.currentPosition;
                //bunifuSlider3.MaximumValue = (int)axWindowsMediaPlayer1.currentMedia.duration;
                axWindowsMediaPlayer1.Ctlcontrols.currentPosition = bunifuSlider3.Value;
            }
            else
            {
                int currentTime = Convert.ToInt32(player1.vid.CurrentPosition);
                int videoLength = Convert.ToInt32(player1.vid.Duration);
                player1.vid.CurrentPosition = bunifuSlider3.Value;
            }
        }


        private void bunifuSlider3_Scroll(object sender, EventArgs e)
        {
            player1.vid.CurrentPosition = bunifuSlider3.Value;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        // recent played 
        private void bunifuFlatButton2_Click(object sender, EventArgs e)
        {
            recent1.BringToFront();
            Menu_Now_On_Top = false;
            NowPlaying_Now_On_Top = false;
            RecentViewed_Now_On_Top = false;
            RecentPlayed_Now_On_Top = true;
            if (Now__Video_Mode)
            {

            }
            else
            {

            }

        }

        private void bunifuFlatButton4_Click(object sender, EventArgs e)
        {
            if (Now__Video_Mode)
            {

            }
            else
            {

            }
        }

        private void bunifuCustomLabel1_Click(object sender, EventArgs e)
        {

        }

        // now playing
        private void setNowPlayingTop()
        {
            Menu_Now_On_Top = false;
            NowPlaying_Now_On_Top = true;
            RecentViewed_Now_On_Top = false;
            RecentPlayed_Now_On_Top = false;
        }
        private void bunifuFlatButton1_Click(object sender, EventArgs e)
        {
            player1.BringToFront();
            setNowPlayingTop();
            if (Now__Video_Mode)
            {

            }
            else
            {

            }

        }

        private void bunifuImageButton4_Click(object sender, EventArgs e)
        {

        }

        private void deleteVideo()
        {
            player1.vid.Stop();
            player1.vid.Dispose();
        }

        private void creatVideo()
        {
            player1.vid = new Video(selectPath, false);
            playPath = selectPath;
            nowVideoPlayIndex = nowVideoSelectIndex;
            player1.vid.Owner = player1.panel1;
            player1.vid.Size = player1.panelSize;
            player1.vid.Play();
        }

        private void showVideo()
        {
            player1.Status = "Play";
            player1.BringToFront();
        }
        // Video Double click item
        internal void UserControl_DoubleClick(object sender, EventArgs e)
        {
            if (player1.Status == "Pause")
                player1.vid.Play();
            else if (selectPath != "")
            {
                if (player1.Status == "Play")
                {
                    deleteVideo();
                }
                creatVideo();
            }
            showVideo();
        }

        // Music Double click item
        internal void UscerControl_DoubleClick1(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.URL = paths[music_Menu1.listBox1.SelectedIndex];
            axWindowsMediaPlayer1.Ctlcontrols.play();
        }
        // Play button
        internal void bunifuImageButton7_Click(object sender, EventArgs e)
        {
            if (Now__Video_Mode)
            {
                if (player1.Status != "Play")
                {
                    if (player1.Status == "Pause")
                    {
                        player1.vid.Play();
                    }
                    else if (selectPath != "")
                    {
                        creatVideo();
                    }
                    if(selectPath != "")
                        showVideo();
                }
            }
            else
            {
                axWindowsMediaPlayer1.Ctlcontrols.play();
                axWindowsMediaPlayer1.URL = paths[music_Menu1.listBox1.SelectedIndex];
            }
        }


        // Pause button
        private void bunifuImageButton6_Click(object sender, EventArgs e)
        {
            if (Now__Video_Mode)
            {
                if (player1.Status == "Play")
                {

                    player1.vid.Pause();
                    player1.Status = "Pause";
                }
            }
            else
            {
                axWindowsMediaPlayer1.Ctlcontrols.pause();
            }
        }

        internal void c_VideoDoubleClick(object sender, EventArgs e)
        {
            if (!player1.FSCREEN)
            {
                player1.vid.Fullscreen = true;
                player1.FSCREEN = true;
            }
            else
            {
                player1.vid.Fullscreen = false;
                player1.FSCREEN = false;
            }
        }

        private void bunifuImageButton8_Click(object sender, EventArgs e)
        {

        }

        private void recent1_Load(object sender, EventArgs e)
        {

        }

        private void player1_Load(object sender, EventArgs e)
        {

        }

        // Recent Viewed
        private void bunifuFlatButton3_Click(object sender, EventArgs e)
        {
            recently_viewed1.BringToFront();
            Menu_Now_On_Top = false;
            NowPlaying_Now_On_Top = false;
            RecentViewed_Now_On_Top = true;
            RecentPlayed_Now_On_Top = false;
            if (Now__Video_Mode)
            {

            }
            else
            {

            }
        }

        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            if (Now_Music_Mode)
            {
                init();
                trackBar2.Maximum = 8000;
                trackBar2.Minimum = 0;
                trackBar2.SmallChange = 300;
                trackBar2.TickFrequency = 300;
                if (axWindowsMediaPlayer1.currentMedia != null)
                {
                    axWindowsMediaPlayer1.Ctlcontrols.stop();
                }
                bunifuFlatButton1.Text = "      NowPlaying";
                if (Menu_Now_On_Top) menu1.BringToFront();
                return;
            }
        }

        private void bunifuImageButton2_Click(object sender, EventArgs e)
        {
            if (Now__Video_Mode)
            {
                init();
                trackBar2.Maximum = 50;
                trackBar2.Minimum = 0;
                trackBar2.SmallChange = 3;
                if (player1.vid != null)
                {
                    deleteVideo();
                }
                Now__Video_Mode = false;
                Now_Music_Mode = true;
                bunifuFlatButton1.Text = "      Liked Songs";
                if (Menu_Now_On_Top) music_Menu1.BringToFront();
                return;
            }
        }

        private void bunifuFlatButton5_Click(object sender, EventArgs e)
        {
            if (Now__Video_Mode)
            {

            }
            else
            {

            }
        }

        private void bunifuImageButton3_Click(object sender, EventArgs e)
        {
            if (Now__Video_Mode)
            {
                
            }
            else
            {
                //Chọn nhạc
                OpenFileDialog List = new OpenFileDialog();
                List.Multiselect = true;
                if (List.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    files = List.SafeFileNames;// Lưu tên bài nhạc vào trong mảng files
                    paths = List.FileNames; //Lưu đường dẫn nhạc vào mảng paths
                                            //Hiển thị tên nhạc trong PlayList
                    for (int i = 0; i < files.Length; i++)
                    {
                        music_Menu1.listBox1.Items.Add(files[i]);
                    }
                }
            }
        }

        // menu
        private void setMenuTop()
        {
            Menu_Now_On_Top = true;
            NowPlaying_Now_On_Top = false;
            RecentViewed_Now_On_Top = false;
            RecentPlayed_Now_On_Top = false;
        }
        private void bunifuFlatButton6_Click(object sender, EventArgs e)
        {
            if (Now_Music_Mode)
                music_Menu1.BringToFront();
            else
                menu1.BringToFront();
            setMenuTop();
        }

        private void menu1_Load(object sender, EventArgs e)
        {

        }

        private void bunifuCustomLabel4_Click(object sender, EventArgs e)
        {

        }

        private void bunifuCustomLabel5_Click(object sender, EventArgs e)
        {

        }
        // Lap video
        private void bunifuImageButton11_Click(object sender, EventArgs e)
        {
            Loop = Loop == true ? false : true;
        }

        // Video phia truoc
        private void nextVideo()
        {
            deleteVideo();
            playPath = menu1.listView1.Items[nowVideoPlayIndex].SubItems[1].Text;
            selectPath = playPath;
            nowVideoSelectIndex = nowVideoPlayIndex;
            creatVideo();
            showVideo();
        }

        private bool Back()
        {
            if (nowVideoPlayIndex > 0)
                return true;
            return false;
        }

        private bool Ahead()
        {
            if (nowVideoPlayIndex <= menu1.listView1.Items.Count - 1 && nowVideoPlayIndex > -1)
                return true;
            return false;
        }

        private void c_Back()
        {
            if (Back())
            {
                nowVideoPlayIndex--;
                nextVideo();
            }
        }

        private void c_Ahead()
        {
            if (Ahead())
            {
                if (nowVideoPlayIndex == menu1.listView1.Items.Count - 1)
                    nowVideoPlayIndex = 0;
                else
                    nowVideoPlayIndex++;
                nextVideo();
            }
        }
        private void bunifuImageButton8_Click_1(object sender, EventArgs e)
        {
            if (Now__Video_Mode) c_Back();
            else
            {
                if (music_Menu1.listBox1.SelectedIndex > 0)
                {
                    music_Menu1.listBox1.SelectedIndex = music_Menu1.listBox1.SelectedIndex - 1; //Lùi lại 1 bài hát
                    axWindowsMediaPlayer1.URL = paths[music_Menu1.listBox1.SelectedIndex];
                    axWindowsMediaPlayer1.Ctlcontrols.play();
                }
            }
        }

        // Video phia sau
        private void bunifuImageButton9_Click(object sender, EventArgs e)
        {
            if (Now__Video_Mode) c_Ahead();
            else
            {
                if (music_Menu1.listBox1.SelectedIndex < music_Menu1.listBox1.Items.Count - 1)
                {
                    music_Menu1.listBox1.SelectedIndex = music_Menu1.listBox1.SelectedIndex + 1; //Chuyển sang bài hát tiếp theo
                    axWindowsMediaPlayer1.URL = paths[music_Menu1.listBox1.SelectedIndex];
                    axWindowsMediaPlayer1.Ctlcontrols.play();
                }
            }
        }


        private void trackBar1_Scroll(object sender, EventArgs e)
        {

        }

        private void bunifuCustomLabel2_Click(object sender, EventArgs e)
        {

        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            if (Now_Music_Mode)
            {
                axWindowsMediaPlayer1.settings.volume = trackBar2.Value;
            }
            else
                player1.vid.Audio.Volume = trackBar2.Value - 8000;
            if (trackBar2.Value > 0)
            {
                bunifuImageButton15.BringToFront();

            }
            if (trackBar2.Value == 0)
            {
                bunifuImageButton12.BringToFront();

            }
        }
        
        // Mo file
        private void bunifuImageButton13_Click(object sender, EventArgs e)
        {
            if (Now__Video_Mode)
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.Filter = "Media Files|*.mpg;*.avi;*.wma;*.mov;*.wav;*mkv;*mp4;|All Files|*.*";

                openFileDialog1.Multiselect = true;
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    var path1 = "";
                    path = Path.Combine(path, "Save Video");
                    path1 = path;
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);
                    List<string> myList = new List<string>();
                    foreach (string file in openFileDialog1.FileNames)
                        myList.Add(System.IO.Path.GetFullPath(file));
                    path = Path.Combine(path, "Meow.txt");
                    TextWriter tsw = new StreamWriter(path, true);
                    foreach (string vidPath in myList)
                    {
                        var imgPath = System.IO.Path.Combine(path1, menu1.getFileName(vidPath)) + ".bmp";
                        if (!File.Exists(imgPath))
                        {
                            var ffMpeg = new NReco.VideoConverter.FFMpegConverter();
                            Bitmap bmp = new Bitmap(0, 0);
                            bmp.Save(imgPath);
                            ffMpeg.GetVideoThumbnail(vidPath, imgPath, 1);
                        }
                        tsw.WriteLine(vidPath);
                    }
                    tsw.Close();
                    menu1.Menu_Refresh();
                }
            }
            else
            {
                //Chọn nhạc
                OpenFileDialog List = new OpenFileDialog();
                List.Multiselect = true;
                if (List.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    files = List.SafeFileNames;// Lưu tên bài nhạc vào trong mảng files
                    paths = List.FileNames; //Lưu đường dẫn nhạc vào mảng paths
                                            //Hiển thị tên nhạc trong PlayList
                    for (int i = 0; i < files.Length; i++)
                    {
                        music_Menu1.listBox1.Items.Add(files[i]);
                    }
                }
            }
        }
        public Point mouseLocation;
        private void panel10_MouseDown(object sender, MouseEventArgs e)
        {
            mouseLocation = new Point(-e.X, -e.Y);
        }

        private void panel10_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point mousePose = Control.MousePosition;
                mousePose.Offset(mouseLocation.X, mouseLocation.Y);
                Location = mousePose;
            }
        }

        private void panel8_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel8_MouseDown(object sender, MouseEventArgs e)
        {
            mouseLocation = new Point(-e.X, -e.Y);

        }

        private void panel8_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void panel8_MouseUp(object sender, MouseEventArgs e)
        {

        }

        private void panel10_MouseUp(object sender, MouseEventArgs e)
        {

        }
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

       
        private void panel9_MouseDown(object sender, MouseEventArgs e)
        {
            mouseLocation = new Point(-e.X, -e.Y);
        }

        private void panel9_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void panel10_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel7_Paint(object sender, PaintEventArgs e)
        {

        }

        private void bunifuImageButton3_MouseHover(object sender, EventArgs e)
        {

        }

        private void bunifuImageButton3_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void bunifuImageButton3_MouseHover_1(object sender, EventArgs e)
        {
            bunifuImageButton3.BackColor = Color.Red;
        }

        private void bunifuImageButton3_MouseLeave(object sender, EventArgs e)
        {
            bunifuImageButton3.BackColor = Color.Gray;
        }

        private void bunifuImageButton14_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void bunifuImageButton14_MouseHover(object sender, EventArgs e)
        {
            Color clr = Color.FromArgb(50, 50, 50);
            bunifuImageButton14.BackColor = clr;
        }

        private void bunifuImageButton14_MouseLeave(object sender, EventArgs e)
        {
            Color clr = Color.FromArgb(30, 30, 30);
            bunifuImageButton14.BackColor = clr;
        }

        private void bunifuImageButton12_Click(object sender, EventArgs e)
        {
            if (trackBar2.Value == 0)
            {
                bunifuImageButton12.BringToFront();

            }

        }

        private void bunifuImageButton15_Click(object sender, EventArgs e)
        {
            if (trackBar2.Value > 0)
            {
                bunifuImageButton15.BringToFront();
                
            }

        }

        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void panel2_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            menu1.Menu_Refresh(textBox1.Text);
        }

        private void panel9_Paint_1(object sender, PaintEventArgs e)
        {

        }

       
    }
}

        