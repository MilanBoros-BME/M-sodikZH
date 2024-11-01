using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MasodikZH
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private float PenWidth_1 = 3;
        private float PenWidth_2 = 1;
        float ax, ay, bx, by, cx, cy;
        int click_counter = 0;
        Point newPoint;
        Random random = new Random();
        Point[] points = new Point[3];

        private void Form1_Load(object sender, EventArgs e)
        {
            button1.Text = "Tömegközéppont";
            button2.Text = "Véletlenszerű kezdőpont";
            button3.Text = "Sierpinski";
            numericUpDown1.Value = 10;
            button3.Enabled = false;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            this.Text = click_counter.ToString() + ". click ";
            if (click_counter == 1)
            {
                e.Graphics.DrawLine(new Pen(Color.Red, PenWidth_2), ax, ay, bx, by);
            }
            if (click_counter == 2)
            {
                e.Graphics.DrawLine(new Pen(Color.Red, PenWidth_1), ax, ay, bx, by);
                e.Graphics.DrawLine(new Pen(Color.Red, PenWidth_2), ax, ay, cx, cy);
                e.Graphics.DrawLine(new Pen(Color.Red, PenWidth_2), bx, by, cx, cy);
            }
            if (click_counter == 3)
            {
                e.Graphics.DrawLine(new Pen(Color.Red, PenWidth_1), ax, ay, bx, by);
                e.Graphics.DrawLine(new Pen(Color.Red, PenWidth_1), ax, ay, cx, cy);
                e.Graphics.DrawLine(new Pen(Color.Red, PenWidth_1), bx, by, cx, cy);
                this.Text += " A(" + ax.ToString() + ", " + ay.ToString() + ") B(" + bx.ToString() + ", " + by.ToString() + ") C(" + cx.ToString() + ", " + cy.ToString() + ")";
            }
            if (click_counter > 3)
            {
                e.Graphics.Clear(Color.White);
                click_counter = 0;
            }
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            ++click_counter;
            switch (click_counter)
            {
                case 1:
                    {
                        ax = e.X;
                        ay = e.Y;
                        cx = ax; cy = ay; bx = ax; by = ay; 
                        break;
                    }
                case 2:
                    {
                        bx = e.X;
                        by = e.Y;
                        break;
                    }

                case 3:
                    {
                        cx = e.X;
                        cy = e.Y;
                        break;
                    }
                    // A háromszög csúcsai: ax, ay; bx, by; cx, cy
            }
            Refresh();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            newPoint.X = newPoint.X - 2;
            newPoint.Y = newPoint.Y - 2;

            int limit = (int)numericUpDown1.Value;
            int asdX;
            int asdY;
            for (int i = 1; i <= limit; i++)
            {
                int randomNumber = random.Next(1, 4);
                if (randomNumber == 1)
                {
                    asdX = ((int)ax + newPoint.X) / 2;
                    asdY = ((int)ay + newPoint.Y) / 2;
                }
                else if (randomNumber == 2)
                {
                    asdX = ((int)bx + newPoint.X) / 2;
                    asdY = ((int)by + newPoint.Y) / 2;
                }
                else
                {
                    asdX = ((int)cx + newPoint.X) / 2;
                    asdY = ((int)cy + newPoint.Y) / 2;
                }
                Graphics p = this.CreateGraphics();
                p.FillEllipse(Brushes.Black, asdX, asdY, 2, 2);
                newPoint.X = asdX;
                newPoint.Y = asdY;
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (click_counter == 1)
            {
                bx = e.X;
                by = e.Y;
                Refresh();
            }
            if (click_counter == 2)
            {
                cx = e.X;
                cy = e.Y;
                Refresh();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Tömegközéppont
            float x = (ax + bx + cx) / 3;
            float y = (ay + by + cy) / 3;
            using (Graphics g = this.CreateGraphics())
            {
                g.FillEllipse(Brushes.Green, x - 3, y - 3, 6, 6);
            }
            label1.Text = "Baricenter: (" + Math.Round(x).ToString() + ", " + Math.Round(y).ToString() + ")";
        }

        private void button2_Click(object sender, EventArgs e)  //Véletlenszerű kezdőpont
        {
            points[0] = new Point((int)ax, (int)ay);
            points[1] = new Point((int)bx, (int)by);
            points[2] = new Point((int)cx, (int)cy);

            PointF randomPoint = StartingPoint(points);
            newPoint = new Point((int)randomPoint.X, (int)randomPoint.Y);
            using (Graphics g = this.CreateGraphics())
            {
                g.FillEllipse(Brushes.Black, newPoint.X - 2, newPoint.Y - 2, 4, 4);
            }
            button3.Enabled = true;
        }
        private PointF StartingPoint(Point[] points)
        {
            Random rnd = new Random();
            float r1 = (float)rnd.NextDouble();  // random number between 0 and 1
            float r2 = (float)rnd.NextDouble();

            // Ensure the point is inside the triangle
            if (r1 + r2 > 1)
            {
                r1 = 1 - r1;
                r2 = 1 - r2;
            }

            float x = (1 - r1 - r2) * ax + r1 * bx + r2 * cx;
            float y = (1 - r1 - r2) * ay + r1 * by + r2 * cy;
            return new PointF(x, y);
        }
        //Létrehozok egy kezdőpontot, amely a PointF segítségével a háromszög belsejében van, a parancs egy 2D-s pontot határoz meg. Erre azét volt szükség mivel, ha csak egy random pontot generáltam.
        //Gyakran befagyott a program.

        private bool IsOutsideTriangle(Point point)
        {
            points[0] = new Point((int)ax, (int)ay);
            points[1] = new Point((int)bx, (int)by);
            points[2] = new Point((int)cx, (int)cy);

            for (int i = 0; i < points.Length; i++)
            {
                int j = (i + 1) % points.Length;
                if (IsOnRight(points[i], points[j], point))
                {
                    return true;
                }
            }
            return false;
        }
        private bool IsOnRight(Point p1, Point p2, Point p)
        {
            return ((p2.X - p1.X) * (p.Y - p1.Y) - (p2.Y - p1.Y) * (p.X - p1.X)) < 0;
        }

    }
    }




