using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApp1
{   
    public partial class Form1 : Form
    {
        enum directions {up,right,down,left }
        SortedDictionary<int, string> Topten = new SortedDictionary<int, string>();
        directions d=directions.right;
        List<PictureBox> Snack = new List<PictureBox>();
        Point position = new Point(45,0);
        PictureBox food;
        int speed = 300;
        int Level = 1;
        int Score = 0;
        bool Pressed = false;
        string Line = string.Empty;
        public Form1()
        {
            InitializeComponent();
            PictureBox P = new PictureBox();
            P.BackColor = Color.DarkBlue;
            P.Size = new Size(15, 15);
            Snack.Add(P);
            for (int i = 0; i < 3; i++)
            {
                PictureBox p = new PictureBox();
                p.BackColor = Color.Blue;
                p.Size = new Size(15, 15);
                Snack.Add(p);
            }
            
            for (int i = 0; i < Snack.Count; i++)
            {
                panel1.Controls.Add(Snack[Snack.Count - 1 - i]);
                panel1.Controls[panel1.Controls.IndexOf(Snack[Snack.Count - 1 - i])].Location = new Point(  15 * i,0);
            }
            lblLevel.Text = Level.ToString();
            lblScore.Text = Score.ToString();
            
            StreamReader sr1 = new StreamReader(string.Format("{0}Resources\\TopTen.txt", Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\"))));
            while ((Line=sr1.ReadLine())!=null)
            {
                Topten.Add(Convert.ToInt32(Line.Substring(1, Line.IndexOf(',') - 1)), Line.Split(' ')[1].Substring(0, Line.Split(' ')[1].Length - 1));
            }
            sr1.Close();
            
            lblTopTen.Text += "\n\n";
            lblTopTen.Text += string.Join(Environment.NewLine, Topten.Reverse()).Replace('[', ' ').Replace(']', ' ').Replace(", ","   ");

            timer.Interval = speed;
            food = newfood(panel1);
            panel1.Controls.Add(food);
            timer.Start();
        }
        public void move(Keys k)
        {
            switch (k)
            {
                case Keys.Up:
                    if (d != directions.down)
                    {
                        d = directions.up;
                    }
                    break;
                case Keys.Down:
                    if (d != directions.up)
                    {
                        d = directions.down;
                    }
                    break;
                case Keys.Left:
                    if (d != directions.right)
                    {
                        d = directions.left;
                    }
                    break;
                case Keys.Right:
                    if (d != directions.left)
                    {
                        d = directions.right;
                    }
                    break;
            }
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if(!Pressed)
            {
                Pressed = true;
                move(e.KeyCode);
            }
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            Pressed = false;
            Point perm = position;
            Point perm2;
            switch (d)
            {
                case directions.up:
                    if (position.Y > 0)
                        position = new Point(position.X, position.Y - 15);
                    else
                        position = new Point(position.X, 420);
                    break;
                case directions.right:
                    if (position.X < 410)
                        position = new Point(position.X + 15, position.Y);
                    else
                        position = new Point(0, position.Y);
                    break;
                case directions.down:
                    if (position.Y < 410)
                        position = new Point(position.X, position.Y + 15);
                    else
                        position = new Point(position.X, 0);
                    break;
                case directions.left:
                    if (position.X > 0)
                        position = new Point(position.X - 15, position.Y);
                    else
                        position = new Point(420, position.Y);
                    break;
            }
            for (int j = 1; j < Snack.Count; j++)
            {
                if (Snack[j].Location == position)
                {
                    timer.Stop();
                    MessageBox.Show($"You lost !!\nYour score is: {Score}");
                    if (Topten.Count < 10)
                        using (Form2 form2 = new Form2())
                        {
                            if (form2.ShowDialog() == DialogResult.OK)
                            {
                                if (!Topten.ContainsKey(Score))
                                    Topten.Add(Score, form2.name);
                                else
                                    Topten[Score] += "," + form2.name;
                                StreamWriter sw = new StreamWriter(string.Format("{0}Resources\\TopTen.txt", Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\"))), false);
                                sw.WriteLine(string.Join(Environment.NewLine, Topten));
                                sw.Close();
                            }
                        }
                    else if (Score > Topten.Keys.Min())
                    {
                        using (Form2 form2 = new Form2())
                        {
                            if (form2.ShowDialog() == DialogResult.OK)
                            {
                                if (!Topten.ContainsKey(Score))
                                {
                                    Topten.Remove(Topten.Keys.Min());
                                    Topten.Add(Score, form2.name);
                                }
                                else
                                    Topten[Score] += "," + form2.name;
                                StreamWriter sw = new StreamWriter(string.Format("{0}Resources\\TopTen.txt", Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\"))), false);
                                 sw.WriteLine(string.Join(Environment.NewLine,Topten));
                                sw.Close();
                            }
                        }
                    }
                    Snack.Clear();
                    panel1.Controls.Clear();
                    speed = 300;
                    Level = 1;
                    Score = 0;
                    position = new Point(45, 0);
                    perm = position;
                    d = directions.right;
                    PictureBox P = new PictureBox();
                    P.BackColor = Color.DarkBlue;
                    P.Size = new Size(15, 15);
                    Snack.Add(P);
                    for (int i = 0; i < 3; i++)
                    {
                        PictureBox p = new PictureBox();
                        p.BackColor = Color.Blue;
                        p.Size = new Size(15, 15);
                        Snack.Add(p);
                    }
                    System.Threading.Thread.Sleep(100);
                    for (int i = 0; i < Snack.Count; i++)
                    {
                        panel1.Controls.Add(Snack[Snack.Count - 1 - i]);
                        panel1.Controls[panel1.Controls.IndexOf(Snack[Snack.Count - 1 - i])].Location = new Point(15 * i, 0);
                    }
                    lblLevel.Text = Level.ToString();
                    lblScore.Text = Score.ToString();
                    Topten.Clear();
                    StreamReader sr1 = new StreamReader( string.Format("{0}Resources\\TopTen.txt", Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\"))));
                    while ((Line = sr1.ReadLine()) != null)
                    {
                        Topten.Add(Convert.ToInt32(Line.Substring(1, Line.IndexOf(',') - 1)), Line.Split(' ')[1].Substring(0, Line.Split(' ')[1].Length - 1));
                    }
                    sr1.Close();

                    lblTopTen.Text = "Top 10: \n\n";
                    lblTopTen.Text += string.Join(Environment.NewLine, Topten.Reverse()).Replace('[', ' ').Replace(']', ' ').Replace(", ", "   ");
                    timer.Interval = speed;
                    food = newfood(panel1);
                    panel1.Controls.Add(food);
                    timer.Start();
                    break;
                }
            }
            
            if (food.Location != position)
            {
                panel1.Controls[panel1.Controls.IndexOf(Snack[0])].Location = position;
                for (int i = 1; i < Snack.Count; i++)
                {
                    perm2 = panel1.Controls[panel1.Controls.IndexOf(Snack[i])].Location;
                    panel1.Controls[panel1.Controls.IndexOf(Snack[i])].Location = perm;
                    perm = perm2;
                }
            }
            else
            {
                panel1.Controls.Remove(food);
                food.BackColor = Color.Blue;
                if (panel1.Controls[panel1.Controls.IndexOf(Snack[Snack.Count - 2])].Location.X == panel1.Controls[panel1.Controls.IndexOf(Snack[Snack.Count - 1])].Location.X)
                    if (panel1.Controls[panel1.Controls.IndexOf(Snack[Snack.Count - 2])].Location.Y > panel1.Controls[panel1.Controls.IndexOf(Snack[Snack.Count - 1])].Location.Y)
                        if (panel1.Controls[panel1.Controls.IndexOf(Snack[Snack.Count - 1])].Location.Y > 0)
                            food.Location = new Point(panel1.Controls[panel1.Controls.IndexOf(Snack[Snack.Count - 1])].Location.X, panel1.Controls[panel1.Controls.IndexOf(Snack[Snack.Count - 1])].Location.Y - 15);
                        else
                            food.Location = new Point(panel1.Controls[panel1.Controls.IndexOf(Snack[Snack.Count - 1])].Location.X, 420);
                    else
                    {
                        if (panel1.Controls[panel1.Controls.IndexOf(Snack[Snack.Count - 1])].Location.Y < 420)
                            food.Location = new Point(panel1.Controls[panel1.Controls.IndexOf(Snack[Snack.Count - 1])].Location.X, panel1.Controls[panel1.Controls.IndexOf(Snack[Snack.Count - 1])].Location.Y + 15);
                        else
                            food.Location = new Point(panel1.Controls[panel1.Controls.IndexOf(Snack[Snack.Count - 1])].Location.X, 0);
                    }
                if (panel1.Controls[panel1.Controls.IndexOf(Snack[Snack.Count - 2])].Location.Y == panel1.Controls[panel1.Controls.IndexOf(Snack[Snack.Count - 1])].Location.X)
                    if (panel1.Controls[panel1.Controls.IndexOf(Snack[Snack.Count - 2])].Location.X > panel1.Controls[panel1.Controls.IndexOf(Snack[Snack.Count - 1])].Location.X)
                        if (panel1.Controls[panel1.Controls.IndexOf(Snack[Snack.Count - 1])].Location.X > 0)
                            food.Location = new Point(panel1.Controls[panel1.Controls.IndexOf(Snack[Snack.Count - 1])].Location.X - 15, panel1.Controls[panel1.Controls.IndexOf(Snack[Snack.Count - 1])].Location.Y);
                        else
                            food.Location = new Point(420, panel1.Controls[panel1.Controls.IndexOf(Snack[Snack.Count - 1])].Location.Y);
                    else
                    {
                        if (panel1.Controls[panel1.Controls.IndexOf(Snack[Snack.Count - 1])].Location.X < 420)
                            food.Location = new Point(panel1.Controls[panel1.Controls.IndexOf(Snack[Snack.Count - 1])].Location.X + 15, panel1.Controls[panel1.Controls.IndexOf(Snack[Snack.Count - 1])].Location.Y);
                        else
                            food.Location = new Point(0, panel1.Controls[panel1.Controls.IndexOf(Snack[Snack.Count - 1])].Location.Y);
                    }

                lblScore.Text = (Score += 10).ToString();
                lblLevel.Text = (Score / 100).ToString();
                timer.Interval = speed-((Score / 100)*20);
                 Snack.Add(food);
                panel1.Controls.Add(Snack[Snack.Count-1]);
                panel1.Controls[panel1.Controls.IndexOf(Snack[0])].Location = position;
                for (int i = 1; i < Snack.Count; i++)
                {
                    perm2 = panel1.Controls[panel1.Controls.IndexOf(Snack[i])].Location;
                    panel1.Controls[panel1.Controls.IndexOf(Snack[i])].Location = perm;
                    perm = perm2;
                }
                food = newfood(panel1);
                panel1.Controls.Add(food);
            }
                
            
                
        }
        public static PictureBox newfood(Panel panel)
        {
            PictureBox food = new PictureBox();
            food.BackColor = Color.Red;
            food.Size = new Size(15, 15);
            Random rd = new Random();
            Random rd1 = new Random();
            int x, y;
            bool exist = false;
            do 
            {
                x = 15 * rd.Next(0, 28);
                y = 15 * rd1.Next(0, 28);
                foreach (var item in panel.Controls)
                {
                    PictureBox perm = item as PictureBox;
                    if(perm.Location==new Point(x,y))
                    {
                        exist = true;
                        break;
                    }
                }
            }
            while (exist);
            food.Location = new Point(x, y);
            return food;
        }
        
    }
}
