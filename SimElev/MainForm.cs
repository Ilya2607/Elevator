using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimElev
{
    public partial class MainForm : Form
    {
        private Graphics graphics;
        private Elevator[] elevator;
        private Random random = new Random();
        private Stopwatch stopwatch = new Stopwatch();
        private Settings settings;
        private int monkeyCount;
        private int[] elevMonkeyCount;
        private bool pauseChecked = false;
        private bool SOSChecked = false;
        private bool[] flag;
        public MainForm()
        {
            InitializeComponent();
        }
        private void StartSimulation()
        {
            if (timer.Enabled) { return; }
            if (pauseChecked)
            {
                pauseChecked = false;
            }
            else
            {
                textBox.Clear();
                settings = new Settings(10, 5);
                pictureBox.Image = new Bitmap(pictureBox.Width, pictureBox.Height);
                graphics = Graphics.FromImage(pictureBox.Image);
                elevator = new Elevator[settings.GetSettingNumOfElev()];
                monkeyCount = 0;
                elevMonkeyCount = new int[settings.GetSettingNumOfElev()];
                flag = new bool[settings.GetSettingNumOfElev()];
                for (int i = 0; i < settings.GetSettingNumOfElev(); i++)
                {
                    elevator[i] = new Elevator(random.Next(1, 3), random.Next(4, 8), settings.GetSettingLevel());
                    elevMonkeyCount[i] = 0;
                    flag[i] = false;
                }
                textBox.AppendText("Start! All elevators went to their floor" + Environment.NewLine);
            }            
            stopwatch.Start();
            timer.Start();            
        }
        private void StopSimulation()
        {
            if (pauseChecked)
            {
                pauseChecked = false;
            }
            if (!timer.Enabled) { return; }
            stopwatch.Stop();
            stopwatch.Reset();
            timer.Stop();
            toolStripStatusLabelMonkeyCount.Text = "0";
        }
        private void PauseSimulation()
        {
            if (!timer.Enabled) { return; }
            stopwatch.Stop();
            timer.Stop();
            pauseChecked = true;
        }
        private void SosSimulation()
        {            
            if (SOSChecked)
            {
                SOSChecked = false;
                textBox.AppendText("SOS is canceled! All elevators went to their floor" + Environment.NewLine);
            }
            else
            { 
                SOSChecked = true;
                textBox.AppendText("SOS! All elevators went to the first floor " + Environment.NewLine);
            }
        }
        private void DrawHouse()
        {
            Pen shaftPen = new Pen(Color.Black, 2);            
            Pen windowPen = new Pen(Color.Black, 1);
            Pen buildingPen = new Pen(Color.Black, 4);
            
            int numOfElev = settings.GetSettingNumOfElev();
            int level = settings.GetSettingLevel();

            for (int i = 0; i < numOfElev; i++)
            {
                for (int j = 0; j < level; j++)
                {
                    graphics.DrawRectangle(buildingPen, 155 - (numOfElev - 1) * 25, 620 - j * 30, 800 + numOfElev * 20 + (numOfElev - 1) * 30, 30);
                    graphics.DrawRectangle(shaftPen, 555 + i * 50 - (numOfElev - 1) * 25, 620 - j * 30, 20, 30);
                    for (int k = 0; k <= 7; k++)
                    {
                        graphics.DrawRectangle(windowPen, 180 + k * 40 - (numOfElev - 1) * 25, 625 - j * 30, 20, 20);
                        graphics.DrawRectangle(windowPen, 630 + k * 40 + numOfElev * 20 + (numOfElev - 1) * 30 - (numOfElev - 1) * 25, 625 - j * 30, 20, 20);
                    }
                }
            }
        }

        private void ElevMoving()
        {            
            Pen liftPen = new Pen(Color.Red, 4);
            graphics.Clear(Color.White);
            DrawHouse();
            
            toolStripStatusLabelTimeSim.Text = String.Format("{0:00}:{1:00}:{2:00}:{3:00}", stopwatch.Elapsed.Hours, stopwatch.Elapsed.Minutes, stopwatch.Elapsed.Seconds, stopwatch.Elapsed.Milliseconds / 10);
            toolStripStatusLabelMonkeyCount.Text = monkeyCount.ToString();

            Task[] task = new Task[settings.GetSettingNumOfElev()];
            for (int i = 0; i < settings.GetSettingNumOfElev(); i++)
            {
                task[i] = Task.Factory.StartNew(() =>
                {
                    graphics.DrawRectangle(liftPen, 555 + i * 50 - (settings.GetSettingNumOfElev() - 1) * 25, elevator[i].GetCoordinate(), 20, 30);
                    if (SOSChecked)
                    {
                        elevator[i].SetNextLevel(1);
                        if (elevator[i].GetLevel() > elevator[i].GetNextLevel())
                        {
                            elevator[i].SetCoordinate(elevator[i].GetCoordinate() + (elevator[i].GetSpeed() * elevator[i].GetAcceleration()));
                        }
                        else
                        {                            
                            elevator[i].SetCoordinate(elevator[i].GetCoordinate());
                        }                        
                    }
                    else
                    {
                        if (elevator[i].GetLevel() > elevator[i].GetNextLevel())
                        {
                            elevator[i].SetCoordinate(elevator[i].GetCoordinate() + (elevator[i].GetSpeed() * elevator[i].GetAcceleration()));
                        }
                        else if (elevator[i].GetLevel() < elevator[i].GetNextLevel())
                        {
                            elevator[i].SetCoordinate(elevator[i].GetCoordinate() - (elevator[i].GetSpeed() * elevator[i].GetAcceleration()));
                        }
                        else
                        {
                            if (flag[i])
                            {
                                textBox.AppendText($"The Monkey {elevMonkeyCount[i]} on elevator {i + 1} arrived at floor " + elevator[i].GetNextLevel().ToString() + Environment.NewLine);
                            }
                            else if (monkeyCount != 0)
                            {
                                textBox.AppendText($"The elevator {i + 1} arrived at floor " + elevator[i].GetNextLevel().ToString() + Environment.NewLine);
                            }
                            int rand = random.Next(0, 2);
                            if (rand == 1)
                            {
                                flag[i] = true;
                                while (elevator[i].GetLevel() == elevator[i].GetNextLevel())
                                {
                                    elevator[i].SetNextLevel(random.Next(1, settings.GetSettingLevel()));
                                }
                                monkeyCount++;
                                elevMonkeyCount[i]++;
                                textBox.AppendText($"The Monkey {elevMonkeyCount[i]} on elevator {i + 1} went to floor " + elevator[i].GetNextLevel().ToString() + Environment.NewLine);
                            }
                            else
                            {
                                flag[i] = false;
                                while (elevator[i].GetLevel() == elevator[i].GetNextLevel())
                                {
                                    elevator[i].SetNextLevel(random.Next(1, settings.GetSettingLevel()));
                                }
                                textBox.AppendText($"The elevator {i + 1} went to floor " + elevator[i].GetNextLevel().ToString() + Environment.NewLine);
                            }
                        }
                    }
                });
                task[i].Wait();
            }
            pictureBox.Refresh();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            ElevMoving();
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            StartSimulation();            
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            StopSimulation();
        }

        private void buttonPause_Click(object sender, EventArgs e)
        {
            PauseSimulation();
        }
        private void buttonSOS_Click(object sender, EventArgs e)
        {
            SosSimulation();
        }

        private void trackBar_Scroll(object sender, EventArgs e)
        {
           
        }
    }
}
