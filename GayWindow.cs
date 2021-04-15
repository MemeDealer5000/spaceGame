using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Media;
using Timer = System.Windows.Forms.Timer;

namespace gayshit
{
    public class GayWindow : Form
    {
        private SoundPlayer player;
        private readonly Dictionary<string, Bitmap> bitmaps = new Dictionary<string, Bitmap>();
        private readonly GameState gameState;
        private readonly HashSet<Keys> pressedKeys = new HashSet<Keys>();
        private int tickCount;
        private readonly Bitmap animatedImage;

        public GayWindow(DirectoryInfo imagesDirectory = null)
        {
            gameState = new GameState();
            ClientSize = new Size(
                GameState.ElementSize * GameMap.MapWidth,
                GameState.ElementSize * GameMap.MapHeight + GameState.ElementSize);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            if (imagesDirectory == null)
                imagesDirectory = new DirectoryInfo(@"C:\Users\Семен\Desktop\GayNiggasOuttaSpace-master\Images");
            animatedImage = new Bitmap(@"C:\Users\Семен\Desktop\GayNiggasOuttaSpace-master\Images\Background.gif");
            foreach (var e in imagesDirectory.GetFiles("*.png"))
                bitmaps[e.Name] = (Bitmap)Image.FromFile(e.FullName);
            var timer = new Timer
            {
                Interval = 15
            };
            timer.Tick += TimerTick;
            timer.Start();
        }

        private void InitializeSound()
        {
            player = new SoundPlayer
            {
                SoundLocation = "Illuminati.wav"
            };
            player.Load();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Text = "GayNiggersOuttaSpace";
            DoubleBuffered = true;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            pressedKeys.Add(e.KeyCode);
            GameMap.KeyPressed = e.KeyCode;
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            pressedKeys.Remove(e.KeyCode);
            GameMap.KeyPressed = pressedKeys.Any() ? pressedKeys.Min() : Keys.None;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            new Action(MakeAnimationInThread).BeginInvoke(null, null);
            lock (animatedImage)
            {
                e.Graphics.TranslateTransform(0, GameState.ElementSize);
                e.Graphics.DrawImage(
                    animatedImage, 0, 0, GameState.ElementSize * GameMap.MapWidth,
                    GameState.ElementSize * GameMap.MapHeight);
            }
            foreach (var a in gameState.Animations)
            {
                if (!(a.Creature is LevelBoss))
                {
                    var imageName = a.Creature.GetImageFileName();
                    var image = bitmaps[imageName];
                    e.Graphics.DrawImage(image, a.Location);
                }
                else
                {
                    var logicalLocation = new Point(a.Location.X - 52, a.Location.Y - GameState.ElementSize * 4);
                    e.Graphics.DrawImage(bitmaps[a.Creature.GetImageFileName()], logicalLocation);
                }
            }
            e.Graphics.ResetTransform();
            e.Graphics.DrawString("Your score:" + GameMap.Scores.ToString(), new Font("Arial", 16), Brushes.Black, 0, 0);
            e.Graphics.DrawString("Lives:" + GameMap.Lives, new Font("Arial", 16), Brushes.Black, 32*10, 0);
        }

        private void MakeAnimationInThread()
        {
            lock (animatedImage)
            {
                ImageAnimator.Animate(animatedImage, AnimationTick);
                ImageAnimator.UpdateFrames();
            }
        }

        private void AnimationTick(object sender, EventArgs args)
        {
            Invalidate();
        }

        private void TimerTick(object sender, EventArgs args)
        {
            if (tickCount == 0) gameState.BeginAction();
            foreach (var e in gameState.Animations)
                e.Location = new Point(e.Location.X + 4 * e.Command.DeltaX, e.Location.Y + 4 * e.Command.DeltaY);
            if (tickCount == 7)
                gameState.EndActions();
            tickCount++;
            if (tickCount == 8) tickCount = 0;
            Invalidate();
            GC.Collect();
        }
    }
}
