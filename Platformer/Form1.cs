using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Platformer
{
    public partial class Form1 : Form
    { 
        Image playerImg;
        Image grassImg;
        Image dirtImg;

        Player player;
        private bool isPressedAnyKey = false;

        const int width=10;
        const int height=15;

        int sideOfMapObject;

        Point delta;

        int[,] map;

        public Form1()
        {
            InitializeComponent();
            playerImg = new Bitmap("C:\\Users\\sodrk\\Desktop\\sprites.png");
            grassImg = new Bitmap("C:\\Users\\sodrk\\Desktop\\grass.png");
            dirtImg = new Bitmap("C:\\Users\\sodrk\\Desktop\\dirt.jpg");
            player = new Player(new Size(32,75),80,0,playerImg);
            timer2.Interval = 10;
            timer2.Tick += new EventHandler(updMovement);
            timer1.Interval = 10;
            timer1.Tick += new EventHandler(update);
            timer1.Start();
            timer2.Start();
            sideOfMapObject = 80;
            delta = new Point(0, 0);

            map = new int[width,height] { {9,9,9,9,9,9,9,9,9,9,1,1,1,1,1 },
                {9,1,1,1,1,1,1,1,1,1,1,1,1,1,1 },
               {9,1,1,1,1,1,1,1,1,1 ,1,1,1,1,1},
                {9,1,1,1,1,1,1,1,1,1 ,1,1,1,1,1},
                {9,1,1,1,1,1,1,1,1,1,1,1,1,1,1 },
                {9,1,1,1,1,1,1,1,1,1,1,1,1,1,1 },
                {9,1,1,1,1,1,1,1,1,1,1,1,1,1,1 },
                {9,1,1,1,1,1,1,1,1,1,1,1,1,1,1 },
                {9,1,1,1,1,1,1,1,1,1,1,1,1,1,1 },
                {9,9,9,9,9,9,9,9,9,9,1,1,1,1,1 },
                         };

            this.KeyDown += new KeyEventHandler(keyboard);
            this.KeyUp += new KeyEventHandler(freeKeyb);

            

            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        private void updMovement(object sender, EventArgs e)
        {
            switch (player.currAnimation)
            {
                case 0:
                    //currAnimation = 0;
                    player.Right();
                    if (player._x >this.Width/2 && player._x<sideOfMapObject*height- this.Width/2)
                        delta.X -= player.speed;
                    break;
                case 1:
                    //currAnimation = 1;
                    player.Left();
                    if (player._x > this.Width / 2 && player._x < sideOfMapObject * height - this.Width / 2)
                        delta.X += player.speed;
                    break;
                case 3:
                    //currAnimation = 3;
                    player.Up();
                    if (player._y > this.Height / 2 && player._y < sideOfMapObject * width - this.Height / 2)
                        delta.Y+= player.speed;
                    break;
                case 2:
                    //currAnimation = 2;
                    player.Down();
                    if (player._y > this.Height / 2 && player._y < sideOfMapObject * width - this.Height / 2)
                        delta.Y-= player.speed;
                    break;
            }
            this.Invalidate();
        }

        private void freeKeyb(object sender, KeyEventArgs e)
        {
            isPressedAnyKey = false;
            switch (e.KeyCode.ToString())
            {
                case "D":
                    player.currAnimation = 5;
                    break;
                case "A":
                    player.currAnimation = 6;
                    break;
                case "W":
                    player.currAnimation = 8;
                    break;
                case "S":
                    player.currAnimation = 7;
                    break;
            }
            player.currFrame = 0;
            label1.Text = "Последняя нажата клавиша: " + e.KeyCode;
        }

        private void keyboard(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode.ToString())
            {
                case "D":
                    player.currAnimation = 0;
                    //pictureBox1.Location = new Point(pictureBox1.Location.X + 2, pictureBox1.Location.Y);
                    break;
                case "A":
                    player.currAnimation = 1;
                    //pictureBox1.Location = new Point(pictureBox1.Location.X - 2, pictureBox1.Location.Y);
                    break;
                case "W":
                    player.currAnimation = 3;
                    //pictureBox1.Location = new Point(pictureBox1.Location.X, pictureBox1.Location.Y-2);
                    break;
                case "S":
                    player.currAnimation = 2;
                    //pictureBox1.Location = new Point(pictureBox1.Location.X, pictureBox1.Location.Y + 2);
                    break;
            }
            isPressedAnyKey = true;

        }

        private void CreateMap(Graphics gr)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (map[i, j] == 1)
                    {
                        gr.DrawImage(grassImg, j * 80+delta.X, i * 80+delta.Y, new Rectangle(new Point(0, 0), new Size(80, 80)), GraphicsUnit.Pixel);
                    }
                    else if (map[i, j] == 9)
                    {
                        gr.DrawImage(dirtImg, j * 80+delta.X, i * 80+delta.Y, new Rectangle(new Point(0, 0), new Size(80, 80)), GraphicsUnit.Pixel);
                    }
                }
            }
        }

        private void PlayAnimation(Graphics gr)
        {
            if (isPressedAnyKey)
            {
                if (player.currAnimation != -1 && player.currAnimation <= 4)
                {
                    gr.DrawImage(player._spritesAnimation, player._x+delta.X, player._y+delta.Y, new Rectangle(new Point(75 * player.currFrame, 170 * player.currAnimation), new Size(75, 170)), GraphicsUnit.Pixel);
                }
            }
            else
            {
                if (player.currAnimation >= 5)
                {
                    gr.DrawImage(player._spritesAnimation, player._x+delta.X, player._y+delta.Y, new Rectangle(new Point(75 * player.currFrame, 170 * (player.currAnimation - 5)), new Size(75, 170)), GraphicsUnit.Pixel);
                }
            }

        }
        

        private void OnPaint(object sender, PaintEventArgs e)
        {
            Graphics gr = e.Graphics;

            CreateMap(gr);
            PlayAnimation(gr);
            
        }
        

        private void update(object sender, EventArgs e)
        {   
            //label1.Text = this.
            if (isPressedAnyKey)
            {
                timer1.Interval = 50;
                //playAnimationMovement();
                if (player.currFrame == 11)
                    player.currFrame = 2;
            }
            else
            {
                timer1.Interval = 125;
                //playAnimationIdle();
                if (player.currFrame == 2)
                    player.currFrame = 0;
            }
            player.currFrame++;
            Invalidate();
        }
        
    }
}
