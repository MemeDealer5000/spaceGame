using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace gayshit
{
    public partial class StartMenu : Form
    {
        private SoundPlayer player;

        public StartMenu()
        {
            InitializeComponent();
        }
        
        private void button1_MouseMove(object sender, MouseEventArgs e)
        {

            button1.BackgroundImage = Image.FromFile(@"C:\Users\Семен\Desktop\GayNiggasOuttaSpace-master\Images\StartButtonPushed.png");
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            button1.BackgroundImage = Image.FromFile(@"C:\Users\Семен\Desktop\GayNiggasOuttaSpace-master\Images\StartButton1.png");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            var form = new GayWindow();
            form.ShowDialog();
            this.Close();
        }

        private void button3_MouseMove(object sender, MouseEventArgs e)
        {

            button2.BackgroundImage = Image.FromFile(@"C:\Users\Семен\Desktop\GayNiggasOuttaSpace-master\Images\ExitButtonPushed.png");
        }

        private void button3_MouseLeave(object sender, EventArgs e)
        {
            button2.BackgroundImage = Image.FromFile(@"C:\Users\Семен\Desktop\GayNiggasOuttaSpace-master\Images\ExitButton1.png");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void InitializeSound()
        {
            player = new SoundPlayer()
            {
                SoundLocation = "Space.wav"
            };
            player.Load();
        }
    }
}
