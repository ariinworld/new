using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace L5A
{
    public partial class Form1 : Form
    {
        private Button[] buttons;
        private Point[] initialPositions;
        private Timer timer;
        private int maxDragTimeInSeconds = 5; // Максимальное время перетаскивания в секундах
        private DateTime startTime;

        private bool isDragging;
        private Button draggedButton;

        public delegate void UnlockedEventHandler(object sender, EventArgs e);
        public event UnlockedEventHandler Unlocked;
       // private void Podpisca(object sender, EventArgs e);

        public Form1()
        {
            
            InitializeComponent();
            InitializeButtons();
            InitializeTimer();
            SubscribeToButtonEvents();
            //Podpisca();
            Unlocked += new EventHandler(Podpisca);
        }



        private void InitializeButtons()
        {
            buttons = new Button[3];
            initialPositions = new Point[buttons.Length];

            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i] = new Button();
                buttons[i].Text = (i + 1).ToString();
                buttons[i].Size = new Size(50, 30);
                buttons[i].Location = new Point(50 + (2 - i) * 100, 50);  // Измененные координаты
                initialPositions[i] = buttons[i].Location;
                Controls.Add(buttons[i]);
            }
        }
        private void ToggleButton_Click(object sender, EventArgs e)
        {
            SwitchedOn = !SwitchedOn;
        }//пшшшшшшшшшшшшш
        private void InitializeTimer()
        {
            timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += Timer_Tick;
        }

        private void SubscribeToButtonEvents()
        {
            foreach (Button button in buttons)
            {
                button.MouseDown += Button_MouseDown;
                button.MouseMove += Button_MouseMove;
                button.MouseUp += Button_MouseUp;
            }
        }

        private void Button_MouseDown(object sender, MouseEventArgs e)
        {
            isDragging = true;
            draggedButton = sender as Button;
            draggedButton.BringToFront();
            startTime = DateTime.Now;
            timer.Start();
        }

        private void Button_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                if (draggedButton != null)
                {
                    draggedButton.Left = e.X + draggedButton.Left - draggedButton.Width / 2;
                    draggedButton.Top = e.Y + draggedButton.Top - draggedButton.Height / 2;
                }
            }
        }

        private void Button_MouseUp(object sender, MouseEventArgs e)
        {
            //timer.Stop();
            isDragging = false;

            Button button = sender as Button;
            int index = Array.IndexOf(buttons, button);

            if (draggedButton != null && index != -1)
            {
                //initialPositions[index] = button.Location;
                CheckCombination();
                draggedButton = null;
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            TimeSpan elapsedTime = DateTime.Now - startTime;
            if (elapsedTime.TotalSeconds > maxDragTimeInSeconds)
            {

                buttons[0].Location = initialPositions[0];
                buttons[1].Location = initialPositions[1];
                buttons[2].Location = initialPositions[2];


                timer.Stop(); // Останавливаем таймер
            }
        }

        private void CheckCombination()
        {
            bool isCorrectCombination = buttons[0].Left < buttons[1].Left && buttons[1].Left < buttons[2].Left;

            if (isCorrectCombination)
            {
                Unlocked?.Invoke(this, EventArgs.Empty);
                //MessageBox.Show("Замок открыт");
            }

        }

       
        private void Podpisca(object sender, EventArgs e)
        {
            MessageBox.Show("Замок открыт");
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }



        /* protected virtual void UnlockedEventHandler()
         {

             MessageBox.Show("Замок открыт");
         }
        */
        /*private void OnUnlocked()
        {
            MessageBox.Show("Замок открыт");
        }*/
    }
}

