using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab6
{
    public partial class Form1 : Form
    {
        Bitmap bmp;
        Graphics g;
        Pen pen_facets = new Pen(Color.Green);
        Pen pen_lines = new Pen(Color.Gray);
        int centerX, centerY;
        bool is_axis = false;
        point3D axis_P1, axis_P2;
        List<facet> facets = new List<facet>();
        List<point3D> points = new List<point3D>();

        public Form1()
        {
            InitializeComponent();
            bmp = new Bitmap(pictureBox.Size.Width, pictureBox.Size.Height);
            pictureBox.Image = bmp; centerX = pictureBox.Width / 2; centerY = pictureBox.Height / 2;
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            g = Graphics.FromImage(bmp);
        }

        private void initialize_points()
        {
            points.Clear();
            foreach (facet sh in facets)
                for (int i = 0; i < sh.points.Count; i++)
                    if (!points.Contains(sh.points[i]))
                        points.Add(sh.points[i]);
        }

        private void draw_point(point3D p)
        {
            g.FillEllipse(new SolidBrush(Color.Gray), (int)Math.Round(p.X + centerX - 6), (int)Math.Round(-p.Y + centerY - 6), 12, 12);
        }

        private void draw_facet(facet f)
        {
            int n = f.points.Count - 1;
            int x1 = (int)Math.Round(f.points[0].X + centerX); int x2 = (int)Math.Round(f.points[n].X + centerX);
            int y1 = (int)Math.Round(-f.points[0].Y + centerY); int y2 = (int)Math.Round(-f.points[n].Y + centerY);
            g.DrawLine(pen_facets, x1, y1, x2, y2);

            for (int i = 0; i < n; i++)
            {
                x1 = (int)Math.Round(f.points[i].X + centerX); x2 = (int)Math.Round(f.points[i + 1].X + centerX);
                y1 = (int)Math.Round(-f.points[i].Y + centerY); y2 = (int)Math.Round(-f.points[i + 1].Y + centerY);
                g.DrawLine(pen_facets, x1, y1, x2, y2);
            }
        }

        private void redraw_image()
        {
            g.Clear(Color.White);
            foreach (facet f in facets)
                draw_facet(f);
            if (axis_P1 != null && axis_P2 != null)
            {
                draw_point(axis_P1);
                draw_point(axis_P2);
                g.DrawLine(pen_lines, (int)Math.Round(axis_P1.X + centerX), (int)Math.Round(-axis_P1.Y + centerY), (int)Math.Round(axis_P2.X + centerX), (int)Math.Round(-axis_P2.Y + centerY));
            }
            pictureBox.Image = bmp;
        }

        private void add_points_to_tetrahedron(int size)
        {
            double h = Math.Sqrt(3) * size;

            point3D p1 = new point3D(-size, -h / 2, 0);
            point3D p2 = new point3D(0, -h / 2, -h);
            point3D p3 = new point3D(size, -h / 2, 0);
            point3D p4 = new point3D(0, h / 2, 0);

            facet f1 = new facet();
            f1.add(p1); f1.add(p2); f1.add(p3);
            facets.Add(f1);
            facet f2 = new facet();
            f2.add(p1); f2.add(p4); f2.add(p2);
            facets.Add(f2);
            facet f3 = new facet();
            f3.add(p4); f3.add(p2); f3.add(p3);
            facets.Add(f3);
            facet f4 = new facet();
            f4.add(p1); f4.add(p4); f4.add(p3);
            facets.Add(f4);
        }

        private void add_points_to_cube(int size)
        {
            point3D p1 = new point3D(-size, -size, -size);
            point3D p2 = new point3D(-size, size, -size);
            point3D p3 = new point3D(size, size, -size);
            point3D p4 = new point3D(size, -size, -size);
            point3D p5 = new point3D(-size, -size, size);
            point3D p6 = new point3D(-size, size, size);
            point3D p7 = new point3D(size, size, size);
            point3D p8 = new point3D(size, -size, size);

            facet f1 = new facet();
            f1.add(p1); f1.add(p2); f1.add(p3); f1.add(p4);
            facets.Add(f1);
            facet f2 = new facet();
            f2.add(p1); f2.add(p2); f2.add(p6); f2.add(p5);
            facets.Add(f2);
            facet f3 = new facet();
            f3.add(p5); f3.add(p6); f3.add(p7); f3.add(p8);
            facets.Add(f3);
            facet f4 = new facet();
            f4.add(p4); f4.add(p3); f4.add(p7); f4.add(p8);
            facets.Add(f4);
            facet f5 = new facet();
            f5.add(p2); f5.add(p6); f5.add(p7); f5.add(p3);
            facets.Add(f5);
            facet f6 = new facet();
            f6.add(p1); f6.add(p5); f6.add(p8); f6.add(p4);
            facets.Add(f6);
        }

        private void add_points_to_octahedron(int size)
        {
            double a = Math.Sqrt(3) * size;
            double p = (a + a + (size / 2)) / 2;
            double h = 2 * Math.Sqrt(p * (p - (size / 2)) * (p - a) * (p - a)) / (size / 2);

            point3D p1 = new point3D(0, -h, 0);
            point3D p2 = new point3D(-size, 0, -size);
            point3D p3 = new point3D(0, h, 0);
            point3D p4 = new point3D(size, 0, -size);
            point3D p5 = new point3D(-size, 0, size);
            point3D p6 = new point3D(size, 0, size);

            facet f1 = new facet();
            f1.add(p2); f1.add(p3); f1.add(p4);
            facets.Add(f1);
            facet f2 = new facet();
            f2.add(p2); f2.add(p1); f2.add(p4);
            facets.Add(f2);
            facet f3 = new facet();
            f3.add(p2); f3.add(p3); f3.add(p5);
            facets.Add(f3);
            facet f4 = new facet();
            f4.add(p2); f4.add(p1); f4.add(p5);
            facets.Add(f4);
            facet f5 = new facet();
            f5.add(p4); f5.add(p3); f5.add(p6);
            facets.Add(f5);
            facet f6 = new facet();
            f6.add(p4); f6.add(p1); f6.add(p6);
            facets.Add(f6);
            facet f7 = new facet();
            f7.add(p5); f7.add(p3); f7.add(p6);
            facets.Add(f7);
            facet f8 = new facet();
            f8.add(p5); f8.add(p1); f8.add(p6);
            facets.Add(f8);
        }

        private void displacement(int kx, int ky, int kz)
        {
            foreach (point3D p in points)
            {
                p.X += kx;
                p.Y += ky;
                p.Z += kz;
            }
        }

        private void rotate(double xAngle, double yAngle, double zAngle)
        {
            foreach (point3D p in points)
            {
                rotate_0X(p, xAngle);
                rotate_0Y(p, yAngle);
                rotate_0Z(p, zAngle);
            }
        }

        private void rotate_0X(point3D p, double angle)
        {
            double y = p.Y;
            double z = p.Z;
            p.Y = y * Math.Cos(angle) + z * Math.Sin(angle);
            p.Z = y * -Math.Sin(angle) + z * Math.Cos(angle);
        }

        private void rotate_0Y(point3D p, double angle)
        {
            double x = p.X;
            double z = p.Z;
            p.X = x * Math.Cos(angle) + z * -Math.Sin(angle);
            p.Z = x * Math.Sin(angle) + z * Math.Cos(angle);
        }


        private void rotate_0Z(point3D p, double angle)
        {
            double x = p.X;
            double y = p.Y;
            p.X = x * Math.Cos(angle) + y * Math.Sin(angle);
            p.Y = x * -Math.Sin(angle) + y * Math.Cos(angle);
        }

        private point3D center_point()
        {
            double sumX = 0, sumY = 0, sumZ = 0;
            int count = 0;
            for (int i = 0; i < facets.Count; i++)
                for (int j = 0; j < facets[i].points.Count; j++)
                {
                    sumX += facets[i].points[j].X;
                    sumY += facets[i].points[j].Y;
                    sumZ += facets[i].points[j].Z;
                    ++count;
                }
            return new point3D(sumX / count, sumY / count, sumZ / count);
        }

        private void scaling(double xScale, double yScale, double zScale)
        {
            point3D center_P = center_point();
            foreach (point3D p in points)
            {
                p.X -= center_P.X;
                p.Y -= center_P.Y;
                p.Z -= center_P.Z;

                p.X *= xScale;
                p.Y *= yScale;
                p.Z *= zScale;

                p.X += center_P.X;
                p.Y += center_P.Y;
                p.Z += center_P.Z;
            }
        }

        private void reflection(int ind)
        {
            if (ind == 1)
                foreach (point3D p in points)
                    p.Z = -p.Z;
            else if (ind == 2)
                foreach (point3D p in points)
                    p.Y = -p.Y;
            else if (ind == 3)
                foreach (point3D p in points)
                    p.X = -p.X;
        }

        private point3D normalize_vector(point3D pt1, point3D pt2)
        {
            double x = pt2.X - pt1.X;
            double y = pt2.Y - pt1.Y;
            double z = pt2.Z - pt1.Z;
            double length = Math.Sqrt(x * x + y * y + z * z);
            return new point3D(x / length, y / length, z / length);
        }

        private void button1_Click(object sender, EventArgs e)  // перенос
        {
            int kx = (int)x_shift.Value, ky = (int)y_shift.Value, kz = (int)z_shift.Value;
            displacement(kx, ky, kz);
            redraw_image();
        }

        private void button2_Click(object sender, EventArgs e)  // масштабирование
        {
            scaling((double)x_scale.Value, (double)y_scale.Value, (double)z_scale.Value);
            redraw_image();
        }

        private void button3_Click(object sender, EventArgs e)  // поворот
        {
            double x_angle = ((double)x_rotate.Value * Math.PI) / 180;
            double y_angle = ((double)y_rotate.Value * Math.PI) / 180;
            double z_angle = ((double)z_rotate.Value * Math.PI) / 180;
            rotate(x_angle, y_angle, z_angle);
            redraw_image();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            reflection(3);
            redraw_image();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            reflection(2);
            redraw_image();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            reflection(1);
            redraw_image();
        }

        private void button7_Click(object sender, EventArgs e)  // выбираем точки для поворота вокруг оси
        {
            if (axis_P1 != null)
            {
                axis_P1 = axis_P2 = null;
                redraw_image();
            }
            is_axis = true;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            axis_P1 = axis_P2 = null;
            redraw_image();
        }

        private void button9_Click(object sender, EventArgs e)  // поворот вокруг оси
        {
            if (axis_P1 == null || axis_P2 == null)
            {
                MessageBox.Show("Выберите ось!", "Ошибка", MessageBoxButtons.OK);
                return;
            }
            double axisAngle = ((double)axis_angle.Value * Math.PI) / 180;
            axis_rotate(axis_P1, axis_P2, axisAngle);
            redraw_image();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            points.Clear();
            facets.Clear();
            g.FillRectangle(new SolidBrush(Color.White), 0, 0, pictureBox.Size.Width, pictureBox.Size.Height);
            pictureBox.Invalidate();
        }

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            facets.Clear();
            if (comboBox1.SelectedItem.ToString() == "Тетраэдр")
                add_points_to_tetrahedron(100);
            else if (comboBox1.SelectedItem.ToString() == "Октаэдр")
                add_points_to_octahedron(100);
            else if (comboBox1.SelectedItem.ToString() == "Гексаэдр")
                add_points_to_cube(100);

            initialize_points();
            redraw_image();
        }

        private void pictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (!is_axis || e.Button != System.Windows.Forms.MouseButtons.Left)
                return;
            if (axis_P1 == null)
            {
                axis_P1 = new point3D((e.X - centerX), (-e.Y + centerY), 0);
                draw_point(axis_P1);
            }
            else
            {
                axis_P2 = new point3D((e.X - centerX), (-e.Y + centerY), 0);
                draw_point(axis_P2);
                g.DrawLine(pen_lines, (int)Math.Round(axis_P1.X + centerX), (int)Math.Round(-axis_P1.Y + centerY), (int)Math.Round(axis_P2.X + centerX), (int)Math.Round(-axis_P2.Y + centerY));
                is_axis = false;
            }
            pictureBox.Image = bmp;
        }

        private void axis_rotate(point3D pt1, point3D pt2, double angle)
        {
            point3D c = normalize_vector(pt1, pt2);
            double x = c.X, y = c.Y, z = c.Z;
            double d = Math.Sqrt(y * y + z * z);
            double alpha = -Math.Asin(y / d);
            double beta = Math.Asin(x);

            foreach (point3D p in points)
            {
                // перенос первой точки вектора в начало координат
                p.X -= pt1.X;
                p.Y -= pt1.Y;
                p.Z -= pt1.Z;

                //повороты - ось вращения совпадает с осью z, для этого вращаем сначала вокруг оси x, затем вокруг y
                rotate_0X(p, alpha);
                rotate_0Y(p, beta);
                rotate_0Z(p, angle);
                rotate_0Y(p, -beta);
                rotate_0X(p, -alpha);

                p.X += pt1.X;
                p.Y += pt1.Y;
                p.Z += pt1.Z;
            }
        }
    }

    public class point3D
    {
        public double X, Y, Z;

        public point3D()
        {
            this.X = this.Y = this.Z = 0;
        }

        public point3D(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

    }

    public class facet
    {
        public List<point3D> points;

        public facet()
        {
            points = new List<point3D>();
        }

        public void add(point3D p)
        {
            points.Add(p);
        }
        public bool is_visible()
        {
            point3D p1 = points[0];
            point3D p2 = points[1];
            point3D p3 = points[2];
            double aX = p2.X - p1.X;
            double aY = p2.Y - p1.Y;
            double aZ = p2.Z - p1.Z;
            double bX = p3.X - p2.X;
            double bY = p3.Y - p2.Y;
            double bZ = p3.Z - p2.Z;

            point3D n = new point3D(aY * bZ - aZ * bY, aZ * bX - aX * bZ, aX * bY - aY * bX);
            return (aX * bY - aY * bX <= 0);
        }
    }
}
