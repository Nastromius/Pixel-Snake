using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pixel_Snake
{
    public partial class Form1 : Form
    {
        private int rI, rJ;
        private PictureBox fruit;
        private PictureBox[] snake = new PictureBox[300];
        private int dirX, dirY;
        private int _wigth = 600;       //Размеры игрового поля
        private int _height = 600;      
        private int _sizeOfSidesWall = 600;     //Размер бокового ограничителя поля
        private int _sizeOfSidesCube = 30;      //Размер куба змейки
        private int score = 0;
        public Form1()
        {
            InitializeComponent();
            Text = $"Snake Score: {score}";
            dirX = 1;
            dirY = 0;
            snake[0] = new PictureBox();
            snake[0].Location = new Point(301, 301);
            snake[0].Size = new Size(_sizeOfSidesCube-1, _sizeOfSidesCube-1);
            snake[0].BackColor = Color.Gray;
            Controls.Add(snake[0]);
            fruit = new PictureBox();
            fruit.BackColor = Color.Yellow;
            fruit.Size = new Size(_sizeOfSidesCube-1, _sizeOfSidesCube-1);
            _generateWall();
            _generateFruit();
            timer.Tick += new EventHandler(_update);
            timer.Interval = 400;
            timer.Start();
            KeyDown += new KeyEventHandler(OKP);
        }

        private void _checkWall()//Переход через стены
        {
            if (snake[0].Location.X < 0)
                snake[0].Location = new Point(snake[0].Location.X - dirX * _sizeOfSidesWall, snake[0].Location.Y + dirY * _sizeOfSidesCube);

            if (snake[0].Location.X > _sizeOfSidesWall)
                snake[0].Location = new Point(snake[0].Location.X - dirX * _sizeOfSidesWall, snake[0].Location.Y + dirY * _sizeOfSidesCube);
            
            if (snake[0].Location.Y < 0)
                snake[0].Location = new Point(snake[0].Location.X + dirX * _sizeOfSidesCube, snake[0].Location.Y - dirY * _sizeOfSidesWall);

            if (snake[0].Location.Y > _sizeOfSidesWall)
                snake[0].Location = new Point(snake[0].Location.X + dirX * _sizeOfSidesCube, snake[0].Location.Y - dirY * _sizeOfSidesWall) ;
        }

        private void _eatFruit()//Увеличение змейки(массива)
        {
            if (snake[0].Location.X == rI && snake[0].Location.Y == rJ)
            {
                Text = $"Snake Score: {++score}";
                snake[score] = new PictureBox();
                snake[score].Location = new Point(snake[score - 1].Location.X + _sizeOfSidesCube * dirX, snake[score - 1].Location.Y + _sizeOfSidesCube * dirY);
                snake[score].Size = new Size(_sizeOfSidesCube-1, _sizeOfSidesCube-1);
                snake[score].BackColor = Color.Black;
                Controls.Add(snake[score]);
                _generateFruit();
            }
        }


        private void _generateFruit()   //случайное появление еды
        {
            Random random = new Random();

            if (score == 0)
            {
                rI = random.Next(_wigth - _sizeOfSidesCube);
                int tempI = rI % _sizeOfSidesCube;
                rI -= tempI;

                rJ = random.Next(_height - _sizeOfSidesCube);
                int tempJ = rJ % _sizeOfSidesCube;
                rJ -= tempJ;

                rI++;
                rJ++;

                fruit.Location = new Point(rI, rJ);
                Controls.Add(fruit);
            }

            for (int _i = 0; _i <= score; _i++)
            {
                if (fruit.Location != snake[_i].Location)
                {
                    rI = random.Next(_wigth - _sizeOfSidesCube);
                    int tempI = rI % _sizeOfSidesCube;
                    rI -= tempI;

                    rJ = random.Next(_height - _sizeOfSidesCube);
                    int tempJ = rJ % _sizeOfSidesCube;
                    rJ -= tempJ;

                    rI++;
                    rJ++;

                    fruit.Location = new Point(rI, rJ);
                    Controls.Add(fruit);
                }
                else
                {
                    _generateFruit();
                }
            }
        }

        private void _eatItSelf()//Поедание самой себя
        {
            for (int _i = 1; _i < score; _i++)
            {
                if (snake[0].Location == snake[_i].Location)
                {
                    for (int _j = _i; _j <= score; _j++)
                        Controls.Remove(snake[_j]);

                    Text = $"Snake Score: {score = score - (score - _i + 1)}" ;
                }
            }
        }

        private void _generateWall() //Создание границ и
        {
            for (int i = 0; i <= _wigth / _sizeOfSidesWall; i++)
            {
                PictureBox pic = new PictureBox();
                pic.BackColor = Color.Black;
                pic.Location = new Point(0, _sizeOfSidesWall * i);
                pic.Size = new Size(_wigth, 1);
                this.Controls.Add(pic);
            }

            for (int i = 0; i <= _height / _sizeOfSidesWall; i++)
            {
                PictureBox pic = new PictureBox();
                pic.BackColor = Color.Black;
                pic.Location = new Point( _sizeOfSidesWall * i, 0);
                pic.Size = new Size(1, _wigth);
                this.Controls.Add(pic);
            }
        }

        private void _moveSnake()//Перемещение зейки(массива)
        {
            for (int i = score; i >= 1; i--)
            {
                snake[i].Location = snake[i - 1].Location;
            }
            snake[0].Location = new Point(snake[0].Location.X + dirX * _sizeOfSidesCube, snake[0].Location.Y + dirY * _sizeOfSidesCube);
            _eatItSelf();
        }
        

        private void _update(Object myObject, EventArgs eventArgs)
        {
            _checkWall();
            _eatFruit();
            _moveSnake();
        }

        private void OKP(object sender, KeyEventArgs e)     //Управление змейкой
        {
            switch(e.KeyCode.ToString())
            {
                case "Right":
                    if (dirX !=-1)
                        dirX = 1;
                    dirY = 0;
                    break;
                case "Left":
                    if (dirX != 1)
                        dirX = -1;
                    dirY = 0;
                    break;
                case "Up":
                    if (dirY != 1)
                        dirY = -1;
                    dirX = 0;
                    break;
                case "Down":
                    if (dirY != -1)
                        dirY = 1;
                    dirX = 0;
                    break;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
