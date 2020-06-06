using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bearings
{    
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }        

        private Bitmap filter(Bitmap bmp)
        {
            Bitmap result = new Bitmap(bmp.Width, bmp.Height);
            for (int i=0; i<bmp.Width; i++)
                for (int j=0; j<bmp.Height; j++)
                {
                    Color p = bmp.GetPixel(i, j);
                    if (Math.Abs(p.R - 255) <= 60 && Math.Abs(p.G - 255) <= 60 && Math.Abs(p.B - 255) <= 60)
                        result.SetPixel(i, j, Color.Black);
                    else
                        result.SetPixel(i, j, p);
                }
            return result;
        }

        private void btnLoadVNS_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp, *.png)|*.jpg; *.jpeg; *.gif; *.bmp; *.png";
            if (open.ShowDialog() == DialogResult.OK)
            {
                Bitmap bmp = new Bitmap(open.FileName);                
                pbVNSClustering.Image = (bmp);
                pbCMOClustering.Image = filter(bmp);
                btnCMO.Enabled = true;
                lblFiltered.Text = "Filtered image";
                lblClassNumber.Visible = false;
                label1.Visible = false;
                btnLearn.Enabled = false;
                cbFailureType.Enabled = false;
                btnTest.Enabled = false;
                txtDecision.Enabled = false;
            }
        }

        public class Planet
        {
            public CMOPoint Start = null;
            public List<CMOPoint> Satellites = null;
            public bool isDisposed = false;
            public Point Center = new Point(-1, -1);
            public int Mass = 0;
            public Color Color = Color.White;
            public int arrayIndex = -1;
            public int sumX = 0;
            public int sumY = 0;
            public double UMass = 0;

            public void dispose()
            {
                Satellites = null;
                Start = null;
                isDisposed = true;
            }



            public void addSatellite(CMOPoint S)
            {
                if (Satellites == null)
                    Satellites = new List<CMOPoint>();
                Satellites.Add(S);
                S.planetIndex = arrayIndex;                       

                double MassRatio = 100 / Mass;
                double DistA = S.color.A - Color.A;
                double DistG = S.color.G - Color.G;

                double NewA = Color.A + (DistA * MassRatio / 100);
                double NewG = Color.G + (DistG * MassRatio / 100);

                Color = Color.FromArgb((int)NewA, (int)NewG, (int)NewG, (int)NewG);
                
                sumX += S.x;
                sumY += S.y;
                Center = new Point((sumX) / (Mass + 1), (sumY) / (Mass + 1));
                Mass++;
            }

            public void removeSatellite(CMOPoint S)
            {
                double MassRatio = 100 / Mass;
                double DistA = S.color.A - Color.A;
                double DistG = S.color.G - Color.G;

                double NewA = Color.A - (DistA * MassRatio / 100);
                double NewG = Color.G - (DistG * MassRatio / 100);

                Color = Color.FromArgb((int)NewA, (int)NewG, (int)NewG, (int)NewG);

                sumX -= S.x;
                sumY -= S.y;
                Center = new Point((sumX) / (Mass - 1), (sumY) / (Mass - 1));
                Mass--;
                S.planetIndex = -1;
                Satellites.Remove(S);
            }

            public Planet(CMOPoint start, int index)
            {
                Start = start;
                Satellites = new List<CMOPoint>();
                Center = new Point(start.x, start.y);
                Mass = 1;
                Color = start.color;
                sumX = start.x;
                sumY = start.y;
                arrayIndex = index;           
            }
        }

        //CMO Algotithm
        public class CMO
        {
            //Attributes
            private Bitmap image = null;
            public Size imgSize = new Size(0, 0);                      
            public Color bgReference = Color.White;
            private int noBGPixelNumber = 0;
            private List<CMOPoint> activePixels = null;
            private List<Planet> Planets = null;
            private List<CMOPoint> Satellites = null;            
            
            public List<Planet> getPlanets()
            {
                return Planets;
            }            

            //Methods
            private Bitmap ConvertToGrayScale(Bitmap img)
            {
                Bitmap gs = new Bitmap(img.Width, img.Height);

                for (int i = 0; i < img.Width; i++)
                {
                    for (int x = 0; x < img.Height; x++)
                    {
                        Color oc = img.GetPixel(i, x);
                        int grayScale = (int)((oc.R * 0.3) + (oc.G * 0.59) + (oc.B * 0.11));
                        Color gscolor = Color.FromArgb(oc.A, grayScale, grayScale, grayScale);
                        gs.SetPixel(i, x, gscolor);
                    }
                }

                return gs;
            }
            private bool isBackground(Bitmap img, Point point)
            {
                if (point.X >= 0 && point.X <= image.Width && point.Y >= 0 && point.Y <= image.Height)
                {
                    Color color = img.GetPixel(point.X, point.Y);
                    if (Math.Abs(color.R - bgReference.R) <= 60 && Math.Abs(color.G - bgReference.G) <= 60 && Math.Abs(color.B - bgReference.B) <= 60)
                        return true;
                    return false;
                }
                return false;
            }

            private void detectBackground()
            {
                noBGPixelNumber = 0;
                List<Color> background = new List<Color>();
                List<int> bgcounter = new List<int>();

                int counter = 1;
                for (int x = 0; x < image.Width; x++)
                {
                    for (int y = 0; y < image.Height; y++)
                    {
                        Color pixel = image.GetPixel(x, y);
                        int grayScale = (int)((pixel.R * 0.3) + (pixel.G * 0.59) + (pixel.B * 0.11));
                        Color pixelcolor = Color.FromArgb(pixel.A, grayScale, grayScale, grayScale);

                        if (background.Count == 0)
                        {
                            background.Add(pixelcolor);
                            bgcounter = new List<int>() { 1 };
                        }
                        else
                        {
                            bool found = false;
                            for (int k = 0; k <= background.Count - 1; k++)
                                if (Math.Sqrt(Math.Pow(background[k].A - pixelcolor.A, 2) - Math.Pow(background[k].R - pixelcolor.R, 2) - Math.Pow(background[k].G - pixelcolor.G, 2) - Math.Pow(background[k].B - pixelcolor.B, 2)) == 0)
                                {
                                    bgcounter[k]++;
                                    found = true;
                                    break;
                                }
                            if (!found)
                            {
                                background.Add(pixelcolor);
                                bgcounter.Add(1);
                            }
                        }
                    }
                }

                int bgIdx = 0;
                int bgNbr = 0;
                for (int i = 0; i < background.Count; i++)
                    if (bgcounter[i] > bgNbr)
                    {
                        bgNbr = bgcounter[i];
                        bgIdx = i;
                    }
                bgReference = background[bgIdx];

                for (int x = 0; x < image.Width; x+=2)
                {
                    for (int y = 0; y < image.Height; y+=2)
                    {
                        Color pixel = image.GetPixel(x, y);
                        int grayScale = (int)((pixel.R * 0.3) + (pixel.G * 0.59) + (pixel.B * 0.11));
                        Color pixelcolor = Color.FromArgb(pixel.A, grayScale, grayScale, grayScale);
                        if (!isBackground(image, new Point(x, y)))
                        {
                            if (activePixels == null)
                                activePixels = new List<CMOPoint>() { new CMOPoint(x, y, Color.FromArgb(pixel.A, pixel.R, pixel.G, pixel.B)) };
                            else
                                activePixels.Add(new CMOPoint(x, y, Color.FromArgb(pixel.A, pixel.R, pixel.G, pixel.B)));
                            noBGPixelNumber++;
                            if (counter == 22)
                            {
                                if (Planets == null)
                                    Planets = new List<Planet>() { new Planet(new CMOPoint(x, y, Color.FromArgb(pixel.A, pixel.R, pixel.G, pixel.B)), 0) };
                                else
                                    Planets.Add(new Planet(new CMOPoint(x, y, Color.FromArgb(pixel.A, pixel.R, pixel.G, pixel.B)), Planets.Count));
                                counter = 1;
                            }
                            else
                            {
                                if (Satellites == null)
                                    Satellites = new List<CMOPoint>() { new CMOPoint(x, y, Color.FromArgb(pixel.A, pixel.R, pixel.G, pixel.B)) };
                                else
                                    Satellites.Add(new CMOPoint(x, y, Color.FromArgb(pixel.A, pixel.R, pixel.G, pixel.B)));
                                counter++;
                            }
                        }
                    }
                }                
            }            

            private double Distance(Point p1, Point p2)
            {
                return Math.Sqrt(Math.Abs(Math.Pow(p1.X - p2.X, 3)) + Math.Abs(p1.Y - p2.Y));
            }

            private double colorDifference(Color c1, Color c2)
            {
                return Math.Sqrt(Math.Pow(c1.R - c2.R, 2) + Math.Pow(c1.G - c2.G, 2) + Math.Pow(c1.B - c2.B, 2) + Math.Pow(c1.A - c2.A, 2));
            }

            private double F(Planet P, CMOPoint S, Planet P2 = null)
            {
                double PGDSS = 5906380;
                double PGDImg = getMaxDistance(activePixels);

                double DistanceRatio = PGDImg * DRA / PGDSS;
                if (P2 == null)
                    return P.Mass / (Distance(new Point(P.Center.X, P.Center.Y), new Point(S.x, S.y)) * Math.Exp(Math.Sqrt(colorDifference(P.Color, S.color)) / 5000));
                else
                    return P.UMass * P2.UMass / (Distance(new Point(P.Center.X, P.Center.Y), new Point(P2.Center.X, P2.Center.Y)) * DistanceRatio * Math.Exp(Math.Sqrt(colorDifference(P.Color, P2.Color)) / 5000));
            }
            
            public void EarthMoonSystem()
            {
                //Calculate m
                int m = Planets.Count();
                //Calculate n
                int n = Satellites.Count();
                //EM System

                List<List<int>> groups = new List<List<int>>();

                bool Equilibre = false;
                while (!Equilibre)
                {
                    Equilibre = true;
                    for (int i = 0; i < n; i++)
                    {                        
                        CMOPoint S = Satellites[i];
                        double greatestForce = 0;
                        int planetIndex = -1;
                        for (int j = 0; j < m; j++)
                        {
                            Planet P = Planets[j];
                            if (greatestForce == 0)
                            {
                                greatestForce = F(P, S);
                                planetIndex = j;
                            }
                            else
                            {
                                double F2 = F(P, S);
                                if (F2 > greatestForce)
                                {
                                    greatestForce = F2;
                                    planetIndex = j;
                                }
                            }
                        }

                        if (S.visitedPlanets.Count(p => p == planetIndex) > 2)
                            S.visitedPlanets[0] = S.visitedPlanets[0];

                        if (planetIndex != S.planetIndex && S.visitedPlanets.Count(p => p == planetIndex)<=2 && groups.Count(g => g[0] == i && g[1] == planetIndex) == 0)
                        {
                            groups.Add(new List<int>() { i, planetIndex });
                            Equilibre = false;
                        }
                    }
                    for (int i = 0; i < groups.Count; i++)
                    {                                                
                        Planets[groups[i][1]].addSatellite(Satellites[groups[i][0]]);                        
                    }                    
                }

                //IsolatedClassAnalysis();
            }

            private void IsolatedClassAnalysis()
            {
                List<List<int>> groups = null;
                int biggestClass = Planets[0].Satellites.Count();
                for (int i = 0; i < Planets.Count; i++)
                    if (Planets[i].Satellites.Count > biggestClass)
                        biggestClass = Planets[i].Satellites.Count;
                int minSize = Convert.ToInt32(biggestClass * 0.1);
                groups = new List<List<int>>();

                for (int i = 0; i < Planets.Count; i++)
                    if (Planets[i].Satellites.Count <= minSize)
                    {
                        double greatestForce = 0;
                        int planetIndex = -1;
                        for (int j = 0; j < Planets.Count(); j++)
                            if (Planets[j].Satellites.Count() > minSize)
                            {
                                Planet P = Planets[j];
                                if (greatestForce == 0)
                                {
                                    greatestForce = F(P, null, Planets[i]);
                                    planetIndex = j;
                                }
                                else
                                {
                                    double F2 = F(P, null, Planets[j]);
                                    if (F2 > greatestForce)
                                    {
                                        greatestForce = F2;
                                        planetIndex = j;
                                    }
                                }
                            }
                        if (groups.Count(g => (g[0] == i && g[1] == planetIndex) || (g[0] == planetIndex && g[1] == i)) == 0)
                            groups.Add(new List<int>() { i, planetIndex });
                    }
                for (int i = 0; i < groups.Count; i++)
                {
                    if (Planets[groups[i][0]].Center.X != -1)
                        foreach (CMOPoint S in Planets[groups[i][0]].Satellites)
                        {
                            Planets[groups[i][1]].addSatellite(S);
                            Planets[groups[i][0]].Center.X = -1;
                        }
                }

                Planets.RemoveAll(p => p.Center.X == -1);
                Planets.RemoveAll(p => p.Satellites.Count == 0);
            }

            double getMaxDistance(List<CMOPoint> points)
            {
                int minX = 0;
                int minY = 0;
                int maxX = 0;
                int maxY = 0;
                int minC = 0;
                int maxC = -1;

                for (int i = 0; i < points.Count; i++)
                {
                    if (points[i].x < minX || minX == 0)
                        minX = points[i].x;
                    if (points[i].x > maxX || maxX == 0)
                        maxX = points[i].x;
                    if (points[i].y < minY || minY == 0)
                        minY = points[i].y;
                    if (points[i].y > maxY || maxY == 0)
                        maxY = points[i].y;
                    if (points[i].color.G < minC || minC == -1)
                        minC = points[i].color.G;
                    if (points[i].color.G > maxC || maxC == -1)
                        maxC = points[i].color.G;
                }

                double result = Math.Sqrt(Math.Pow(minX - maxX, 2) + Math.Pow(minY - maxY, 2)) * (Math.Sqrt(Math.Abs(minC - maxC)));
                return result;
                    
            }

            public void EarthAppleSystem()
            {
                if (Planets != null)
                {
                    double SSMass = 2987831;
                    double UMass = SSMass / activePixels.Count;

                    double PGDSS = 5906380;
                    double PGDImg = getMaxDistance(activePixels);

                    double DistanceRatio = PGDImg * DRA / PGDSS;

                    List<List<int>> groups = new List<List<int>>();

                    bool Equilibre = false;
                    while (!Equilibre)
                    {
                        Equilibre = true;
                        for (int i = 0; i < Planets.Count; i++)
                            for (int j = i + 1; j < Planets.Count; j++)                                
                                {
                                    if (Planets[i].UMass == 0)
                                        Planets[i].UMass = UMass * Planets[i].Mass;
                                    if (Planets[j].UMass == 0)
                                        Planets[j].UMass = UMass * Planets[j].Mass;

                                    double distance = Distance(Planets[i].Center, Planets[j].Center) * Math.Exp(Math.Sqrt(colorDifference(Planets[i].Color, Planets[j].Color)));

                                    if ((DistanceRatio * Planets[i].UMass / 213 > distance || DistanceRatio * Planets[j].UMass / 213 > distance) && groups.Count(g => (g[0] == i && g[1] == j) || (g[0] == j && g[1] == i)) == 0)
                                    {
                                        groups.Add(new List<int>() { i, j });
                                        Equilibre = false;
                                    }

                                }                        
                    }
                    for (int i = 0; i < groups.Count; i++)
                    {
                        if (Planets[groups[i][0]].Center.X != -1)
                            foreach (CMOPoint S in Planets[groups[i][0]].Satellites)
                            {
                                Planets[groups[i][1]].addSatellite(S);
                                Planets[groups[i][0]].Center.X = -1;
                            }                                                    
                    }

                    Planets.RemoveAll(p => p.Center.X == -1);
                    Planets.RemoveAll(p => p.Satellites.Count == 0);

                    int biggestClass = Planets[0].Satellites.Count();
                    for (int i = 0; i < Planets.Count; i++)
                        if (Planets[i].Satellites.Count > biggestClass)
                            biggestClass = Planets[i].Satellites.Count;
                    int minSize = Convert.ToInt32(biggestClass * 0.2);
                    groups = new List<List<int>>();

                    for (int i = 0; i < Planets.Count; i++)
                        if (Planets[i].Satellites.Count <= minSize)
                        {
                            double greatestForce = 0;
                            int planetIndex = -1;
                            for (int j = 0; j < Planets.Count(); j++)
                                if (Planets[j].Satellites.Count() > minSize)
                                {
                                    Planet P = Planets[j];
                                    if (greatestForce == 0)
                                    {
                                        greatestForce = F(P, null, Planets[i]);
                                        planetIndex = j;
                                    }
                                    else
                                    {
                                        double F2 = F(P, null, Planets[j]);
                                        if (F2 > greatestForce)
                                        {
                                            greatestForce = F2;
                                            planetIndex = j;
                                        }
                                    }
                                }
                            if (groups.Count(g => (g[0] == i && g[1] == planetIndex) || (g[0] == planetIndex && g[1] == i)) == 0)
                                groups.Add(new List<int>() { i, planetIndex });
                        }
                    for (int i = 0; i < groups.Count; i++)
                    {
                        if (Planets[groups[i][0]].Center.X != -1)
                            foreach (CMOPoint S in Planets[groups[i][0]].Satellites)
                            {
                                Planets[groups[i][1]].addSatellite(S);
                                Planets[groups[i][0]].Center.X = -1;
                            }
                    }

                    Planets.RemoveAll(p => p.Center.X == -1);
                    Planets.RemoveAll(p => p.Satellites.Count == 0);
                }                
            }

            //Class cunstructor
            public CMO(Bitmap img)
            {
                image = (Bitmap)img.Clone();
                imgSize = new Size(image.Width, image.Height);
                detectBackground();
                EarthMoonSystem();
                EarthAppleSystem();
            }            
        }

        public static List<string> LearningResults = null;
        public static List<string> TestResults = null;

        public class Learning
        {
            private double Distance(Point p1, Point p2)
            {
                return Math.Sqrt(Math.Abs(Math.Pow(p1.X - p2.X, 3)) + Math.Abs(p1.Y - p2.Y));
            }

            private static Rectangle getBounds(List<CMOPoint> points)
            {
                int minX = points[0].x;
                int minY = points[0].y;
                int maxX = points[0].x;
                int maxY = points[0].y;

                for (int i = 0; i < points.Count; i++)
                {
                    if (points[i].x < minX)
                        minX = points[i].x;
                    if (points[i].x > maxX)
                        maxX = points[i].x;
                    if (points[i].y < minY)
                        minY = points[i].y;
                    if (points[i].y > maxY)
                        maxY = points[i].y;
                }


                return new Rectangle(minX, minY, maxX - minX + 100, maxY - minY + 100);
            }

            public static List<Point> getAdj(int x, int y, int w, int h, int minx, int miny, int level = 1)
            {
                List<Point> Adj = new List<Point>();
                for (int i = x - level; i <= x + level; i++)
                    for (int j = y - level; j <= y + level; j++)
                        if (i >= minx && i <= w && j >= miny && j <= h)
                            Adj.Add(new Point(i, j));
                return Adj;
            }

            public static List<Point> ExtractEdges(List<CMOPoint> points)
            {
                List<Point> result = new List<Point>();

                Rectangle r = getBounds(points);
                int[,] mat = new int[r.Width, r.Height];

                foreach (CMOPoint p in points)
                {
                    List<Point> adj = getAdj(p.x, p.y, r.Width + r.X, r.Height + r.Y, r.X, r.Y, 10);
                    foreach (Point p2 in adj)
                        mat[p2.X - r.X, p2.Y - r.Y] = 1;
                }

                for (int x = 0; x < r.Width; x += 1)
                {
                    for (int y = 0; y < r.Height; y += 1)
                        if (mat[x, y] == 1)
                        {
                            result.Add(new Point(x, y));
                            break;
                        }
                }

                for (int y = 0; y < r.Height; y += 1)
                {
                    for (int x = r.Width - 1; x >= 0; x--)
                        if (mat[x, y] == 1)
                        {
                            result.Add(new Point(x, y));
                            break;
                        }
                }


                for (int x = r.Width - 1; x >= 0; x--)
                {
                    for (int y = r.Height - 1; y >= 0; y--)
                        if (mat[x, y] == 1)
                        {
                            result.Add(new Point(x, y));
                            break;
                        }
                }

                for (int y = r.Height - 1; y >= 0; y--)
                {
                    for (int x = 0; x < r.Width; x++)
                        if (mat[x, y] == 1)
                        {
                            result.Add(new Point(x, y));
                            break;
                        }
                }

                return result;
            }            
                        
            public Learning(List<Planet> CMOClustering, string FailureName, bool Test = false)
            {
                if (CMOClustering != null)
                {
                    bool firstlearning = false;
                    string existTest = Globale.Query("select count(name) as c from failure_types where name='" + FailureName + "'").Rows[0]["c"].ToString();
                    if (Convert.ToInt32(existTest) == 0)
                        firstlearning = true;
                    int ClassOrder = 0;

                    //Coefficients
                    double EdgesCoeff = 15;
                    double PatternShapesCoeff = 15.0;
                    double ColorChangesCoeff = 20.0;
                    double ColorPatternsCoeff = 20.0;
                    double PatternLocationsCoeff = 10.0;                    
                    double PatternSizesCoeff = 10.0;

                    LearningResults = new List<string>();

                    if (firstlearning && !Test)
                        LearningResults.Add("First learning session for " + FailureName + " :");
                    else if (Test)
                        TestResults.Add("Test step initiated...");

                    List<List<double>> links = new List<List<double>>();

                    List<List<List<List<double>>>> Features = new List<List<List<List<double>>>>();

                    foreach (Planet P in CMOClustering)
                    {
                        List<List<List<double>>> classFeatures = new List<List<List<double>>>();

                        LearningResults.Add("Class " + Convert.ToString(ClassOrder) + " :");
                        List<CMOPoint> Data = P.Satellites;
                        //Extract edges
                        List<Point> Edges = ExtractEdges(Data);                        
                        Rectangle r = getBounds(Data);
                        double[,] mat = new double[r.Width, r.Height];

                        foreach (CMOPoint p in Data)
                        {
                            List<Point> adj = getAdj(p.x, p.y, r.Width + r.X, r.Height + r.Y, r.X, r.Y);
                            foreach (Point p2 in adj)
                                mat[p2.X - r.X, p2.Y - r.Y] = p.color.G;
                        }

                        //Classe's dominant color
                        double dcolor = 0;
                        double dci = 0;
                        
                        for (int mi=0; mi<r.Width; mi++)
                            for (int mj = 0; mj < r.Height; mj++)
                                if(mat[mi, mj] != 0)
                                {
                                    dcolor += mat[mi, mj];
                                    dci++;
                                }
                        dcolor /= dci;                       

                        //Extract Color Patternss & Color Changes
                        List<double> Colors = new List<double>();                                                
                        
                        int j = 0;
                        int segment = 1;
                        int i = 0;

                        List<List<int>> ppos = new List<List<int>>();

                        while (i < r.Width)
                        {
                            bool posfound = false;
                                                        
                            double color = 0;
                            int ci = 0;

                            while (j < r.Height && i < r.Width)
                            {
                                if (mat[i, j] != 0)
                                {
                                    if (!posfound)
                                    {
                                        ppos.Add(new List<int>() { i, j });
                                        posfound = true;
                                    }
                                    color += mat[i, j];
                                    ci++;
                                }                                
                                i += 1;
                                if (i >= (20 * segment))
                                {
                                    i = (segment * 20) - 20;
                                    j++;
                                }
                            }
                            if (Colors.Count >= 1)
                            {
                                if (ci > 0 && Math.Abs(Colors[Colors.Count - 1] - (color / ci)) >= 10)
                                    Colors.Add(color / ci);
                            }                                
                            else
                                Colors.Add(color / ci);

                            i = (segment * 20) + 1;
                            j = 0;
                            segment++;
                        }

                        //Color Changes
                        List<double> ColorChanges = new List<double>();
                        List<List<double>> ColorPatterns = new List<List<double>>();
                        if (Colors.Count() > 0)
                        {
                            double c = Colors[0];
                            int cpt = 1;
                            int ppi = 0;
                            for (int k = 1; k < Colors.Count; k++)
                                if (Math.Abs(Colors[k] - c) >= 10)
                                {
                                    ColorChanges.Add(Math.Abs(Colors[k] - c));
                                    ColorPatterns.Add(new List<double>() { c, cpt, ppos[ppi][0], ppos[ppi][1], c/dcolor });
                                    ppi++;
                                    cpt = 1;
                                    c = Colors[k];
                                }
                                else
                                    cpt++;
                            ColorPatterns.Add(new List<double>() { c, cpt, ppos[ppi][0], ppos[ppi][1], c / dcolor });

                        }

                        List<List<double>> Patterns = new List<List<double>>();
                        for (int k=0; k<ColorPatterns.Count; k++)
                        {
                            double pcolor = ColorPatterns[k][0];
                            List<CMOPoint> apattern = new List<CMOPoint>();
                            for (int l=0; l<r.Width; l++)
                                for (int m = 0; m < r.Height; m++)
                                    if(Math.Abs(mat[l, m] - pcolor) <= 10 && mat[l, m] != 0)                                    
                                        apattern.Add(new CMOPoint(l, m, Color.FromArgb(255, (int)mat[l, m], (int)mat[l, m], (int)mat[l, m])));
                            //Extract pattern edges
                            List<Point> pEdges = ExtractEdges(apattern);
                            //Check if shape exists (%)

                            DataTable shapes = Globale.Query("select shapeid from shapes group by shapeid");
                            double sprc = 0;
                            string shapename = "";

                            if (shapes != null)
                                foreach (DataRow s in shapes.Rows)
                                {
                                    int cpt = 0;
                                    DataTable spoints = Globale.Query("select x, y from shapes where shapeid='" + s["shapeid"].ToString() + "'");
                                    if (spoints != null)
                                        foreach (DataRow dr in spoints.Rows)
                                            foreach (Point e in pEdges)
                                                if (Distance(new Point(Convert.ToInt32(dr["x"]), Convert.ToInt32(dr["y"])), new Point(e.X, e.Y)) <= 5)
                                                    cpt++;
                                    double prc = cpt * 100 / pEdges.Count;
                                    if (prc > sprc)
                                    {
                                        shapename = s["shapeid"].ToString();
                                        sprc = prc;
                                    }
                                }

                            //Insert pShape if no match was found then Build pattern inside Patterns
                            if (sprc < 70)
                            {                                
                                DataTable oldshapes = Globale.Query("select isnull(max(shapeid), 0) as max from shapes");
                                string newshape = Convert.ToString(Convert.ToInt32(oldshapes.Rows[0]["max"].ToString()) + 1);

                                foreach (Point pe in pEdges)
                                    Globale.Query("insert into shapes(shapeid, x, y) values('" + newshape + "', '" + Convert.ToString(pe.X) + "', '" + Convert.ToString(pe.Y) + "')");
                                shapename = newshape;
                            }

                            double pacolor = 0;
                            int pix = apattern[0].x;
                            int piy = apattern[0].y;
                            for (int pi = 0; pi < apattern.Count; pi++)
                            {                                
                                pacolor += apattern[pi].color.G;
                                if (apattern[pi].x < pix)
                                    pix = apattern[pi].x;
                                if (apattern[pi].y < piy)
                                    piy = apattern[pi].y;
                            }
                            pcolor /= apattern.Count;
                            DataTable oldpatterns = Globale.Query("select isnull(max(patternid), 0) as max from patterns where failuretypename='" + FailureName + "'");
                            string patternid = Convert.ToString(Convert.ToInt32(oldpatterns.Rows[0]["max"].ToString()) + 1);
                            Patterns.Add(new List<double>() { Convert.ToDouble(patternid), Convert.ToDouble(shapename), apattern.Count, pacolor, pix, piy });

                        }                                                                                                

                        
                        if (! Test)
                        {
                            for (int k = 0; k < ColorChanges.Count; k++)
                            {
                                int csorder = Convert.ToInt32(Globale.Query("select isnull(max(csorder), -1) + 1 from color_changes where [FailureTypeName] = '" + FailureName + "' and classorder = '" + ClassOrder + "' ").Rows[0][0].ToString());
                                Globale.Query("insert into color_changes(failuretypename, colorchange, csorder, classorder) values('" + FailureName + "', '" + ColorChanges[k] + "', '" + csorder + "', '" + ClassOrder + "')");
                            }
                            LearningResults.Add("Color changes : " + Convert.ToString(ColorChanges.Count) + " inserted.");

                            for (int k = 0; k < ColorPatterns.Count; k++)
                                Globale.Query("insert into color_patterns(failuretypename, color, size, x, y, condensation, classorder) values('" + FailureName + "', '" + Convert.ToDouble(ColorPatterns[k][0]) + "', '" + ColorPatterns[k][1] + "', '" + ColorPatterns[k][2] + "', '" + ColorPatterns[k][3] + "', '" + Convert.ToDouble(ColorPatterns[k][4]) + "', '" + ClassOrder + "')");
                            LearningResults.Add("Color patterns : " + Convert.ToString(ColorPatterns.Count) + " inserted.");

                            for (int k = 0; k < Edges.Count; k++)
                                Globale.Query("insert into edges(failuretypename, x, y, classorder) values('" + FailureName + "', '" + Convert.ToString(Edges[k].X) + "', '" + Convert.ToString(Edges[k].Y) + "', '" + ClassOrder + "')");
                            LearningResults.Add("Class edges : " + Convert.ToString(Edges.Count) + " inserted.");

                            for (int k = 0; k < Patterns.Count; k++)
                            {
                                int patternid = Convert.ToInt32(Globale.Query("select isnull(max(patternid), -1) + 1 from patterns where [FailureTypeName] = '" + FailureName + "' and classorder = '" + ClassOrder + "'").Rows[0][0].ToString());
                                Globale.Query("insert into patterns([FailureTypeName],[PatternID],[ShapeID],[Size], Color,[X],[Y], classorder) values('" + FailureName + "', '" + patternid + "', '" + Patterns[k][1] + "', '" + Patterns[k][2] + "', '" + Patterns[k][3] + "', '" + Patterns[k][4] + "', '" + Patterns[k][5] + "', '" + ClassOrder + "')");
                            }
                            LearningResults.Add("Class patterns : " + Convert.ToString(Patterns.Count) + " inserted.");

                            //TODO: Existing failure type, we update it
                        }
                        
                        classFeatures.Add(new List<List<double>>() { ColorChanges });
                        classFeatures.Add(ColorPatterns);
                        classFeatures.Add(Patterns);
                        classFeatures.Add(new List<List<double>>());
                        for (int ei = 0; ei < Edges.Count; ei++)
                            classFeatures[classFeatures.Count - 1].Add(new List<double>() { Edges[ei].X, Edges[ei].Y });

                        Features.Add(classFeatures);

                        ClassOrder++;
                    }

                    //New failure type, we insert it
                    if (firstlearning && !Test)
                        Globale.Query("insert into failure_types([Name] ,[ColorPatternsCoeff] ,[ColorChangesCoeff] ,[PatternShapesCoeff] ,[PatternSizesCoeff] ,[PatternLocationsCoeff] ,[EdgesCoeff]) values('" + FailureName + "' ," + ColorPatternsCoeff + " ," + ColorChangesCoeff + " ," + PatternShapesCoeff + " ," + PatternSizesCoeff + " ," + PatternLocationsCoeff + " ," + EdgesCoeff + ")");

                    if (Test)
                    {
                        List<string> FailureTypes = new List<string>();
                        DataTable FT = Globale.Query("select name from failure_types");
                        foreach (DataRow f in FT.Rows)
                            FailureTypes.Add(f[0].ToString());

                        if (FailureTypes.Count == 0)
                        {
                            TestResults.Add("Knowledge base is empty.");
                            TestResults.Add("No matching can be performed.");
                        }
                        else
                        {
                            List<double> Probailities = new List<double>();
                            List<string> Decisions = new List<string>();

                            foreach (string FailureTypeName in FailureTypes)
                            {
                                DataTable DBTColorChanges = Globale.Query("select * from color_changes where failuretypename='" + FailureTypeName + "' order by classorder, csorder");
                                DataTable DBTColorPatterns = Globale.Query("select * from color_patterns where failuretypename='" + FailureTypeName + "' order by classorder, x");
                                DataTable DBTEdges = Globale.Query("select * from edges where failuretypename='" + FailureTypeName + "' order by classorder");
                                DataTable DBTPatterns = Globale.Query("select * from patterns where failuretypename='" + FailureTypeName + "' order by classorder, patternid");

                                //FTName, ColorChange, CSOrder, ClassOrder
                                List<List<double>> DBColorChanges = new List<List<double>>();
                                //FTName, Color, Size, X, Y, Condensation, classorder
                                List<List<double>> DBColorPatterns = new List<List<double>>();
                                //FTName, X, Y, ClassOrder
                                List<List<double>> DBEdges = new List<List<double>>();
                                //FTName, PatternID, ShapeID, Size, Color, X, Y, ClassOrder
                                List<List<double>> DBPatterns = new List<List<double>>();

                                foreach (DataRow dr in DBTColorChanges.Rows)
                                    DBColorChanges.Add(new List<double>() { Convert.ToDouble(dr[1].ToString()), Convert.ToDouble(dr[2].ToString()), Convert.ToDouble(dr[3].ToString()) });

                                foreach (DataRow dr in DBTColorPatterns.Rows)
                                    DBColorPatterns.Add(new List<double>() { Convert.ToDouble(dr[1].ToString()), Convert.ToDouble(dr[2].ToString()), Convert.ToDouble(dr[3].ToString()), Convert.ToDouble(dr[4].ToString()), Convert.ToDouble(dr[5].ToString()) });                                

                                foreach (DataRow dr in DBTPatterns.Rows)
                                    DBPatterns.Add(new List<double>() { Convert.ToDouble(dr[1].ToString()), Convert.ToDouble(dr[2].ToString()), Convert.ToDouble(dr[3].ToString()), Convert.ToDouble(dr[4].ToString()), Convert.ToDouble(dr[5].ToString()), Convert.ToDouble(dr[6].ToString()), Convert.ToDouble(dr[7].ToString()) });

                                foreach (DataRow dr in DBTEdges.Rows)
                                    DBEdges.Add(new List<double>() { Convert.ToDouble(dr[1].ToString()), Convert.ToDouble(dr[2].ToString()), Convert.ToDouble(dr[3].ToString()) });

                                int NBClassDB = Convert.ToInt32(Convert.ToString(Globale.Query("select max(classorder) from color_changes where failuretypename='" + FailureTypeName + "'").Rows[0][0]));
                                int NBClassExtract = CMOClustering.Count;

                                int iDBClass = 0;
                                for (int i=0; i<Features.Count; i++)
                                {
                                    List<List<double>> EColorChanges = Features[i][0];
                                    List<List<double>> EColorPatterns = Features[i][1];
                                    List<List<double>> EPatterns = Features[i][2];
                                    List<List<double>> EEdges = Features[i][3];

                                    if (iDBClass <= NBClassDB)
                                    {
                                        List<List<double>> classDBColorChanges = DBColorChanges.FindAll(l => Convert.ToInt32(l[2]) == iDBClass);
                                        List<List<double>> classDBColorPatterns = DBColorPatterns.FindAll(l => Convert.ToInt32(l[5]) == iDBClass);
                                        List<List<double>> classDBEdges = DBEdges.FindAll(l => Convert.ToInt32(l[2]) == iDBClass);
                                        List<List<double>> classDBDBPatterns = DBPatterns.FindAll(l => Convert.ToInt32(l[6]) == iDBClass);


                                    }
                                    iDBClass++;
                                }

                            }
                        }                        
                    }
                }
            }
        }
        
        public static double DRA = 1;        
        private void btnCMO_Click(object sender, EventArgs e)
        {
            float output = 0;
            if (float.TryParse(txtRatio.Text, out output))
                DRA = Convert.ToDouble(txtRatio.Text) * 10000;
            else
                DRA = 10000;
            CMO CMO = new CMO((Bitmap)pbVNSClustering.Image.Clone());
            TraceSolarSystem(CMO);
            lblFiltered.Visible = false;
            lblClassNumber.Visible = true;
            label1.Visible = true;
            btnLearn.Enabled = true;
            cbFailureType.Enabled = true;
            btnTest.Enabled = true;
            txtDecision.Enabled = true;
            cbFailureType.SelectedIndex = 0;
        }

        public static List<CMOPoint> GetEdgess(List<CMOPoint> points, int imageWidth, int imageHeight)
        {            
            Point start = new Point(0, 0);
            Point finish = new Point(0, 0);
            Point center = new Point(0, 0);
            foreach (CMOPoint p in points)
            {
                center.X += p.x;
                center.Y += p.y;

                if (start.X == 0 || p.x < start.X)
                    start.X = p.x;
                if (start.Y == 0 || p.y < start.Y)
                    start.Y = p.y;

                if (finish.X == 0 || p.x > finish.X)
                    finish.X = p.x;
                if (finish.Y == 0 || p.y > finish.Y)
                    finish.Y = p.y;
            }
            center.X /= points.Count;
            center.Y /= points.Count;
            
            Rectangle r = new Rectangle(new Point(center.X - 1, center.Y - 1), new Size(2, 2));

            while (points.Count(p => p.x >= r.X && p.y >= r.Y && p.x <= r.X + r.Width && p.y <= r.Y + r.Height) < points.Count() && r.X >= start.X && r.Y >= start.Y)
                r = new Rectangle(new Point(r.X - 1, r.Y - 1), new Size(r.Width + 2, r.Height + 2));

            return new List<CMOPoint>() { new CMOPoint(r.X, r.Y, Color.Black), new CMOPoint(r.Width, r.Height, Color.Black) }; 
        }        

        public void TraceSolarSystem(CMO cmo)
        {                        
            Bitmap CMOGraphicalRepresentation = new Bitmap(cmo.imgSize.Width, cmo.imgSize.Height);
            using (Graphics g = Graphics.FromImage(CMOGraphicalRepresentation))
                g.FillRectangle(Brushes.White, new Rectangle(0, 0, cmo.imgSize.Width, cmo.imgSize.Height));
            List<Planet> System = cmo.getPlanets();

            CMOResults = System;
            test = System;

            int c = 0;
            List<Color> Colors = new List<Color> { Color.LightGreen, Color.Magenta, Color.Yellow, Color.Red, Color.Pink, Color.Purple, Color.Orange };

            if (System != null)
            {
                lblClassNumber.Text = Convert.ToString(System.Count()) + " Classes";
                for (int i = 0; i < System.Count; i++)
                    using (Graphics g = Graphics.FromImage(CMOGraphicalRepresentation))
                    {
                        List<CMOPoint> points = System[i].Satellites;
                        if (points != null)
                            for (int j = 0; j < points.Count; j++)
                                using (SolidBrush b = new SolidBrush(Colors[c]))
                                    g.FillEllipse(b, points[j].x - 3, points[j].y - 3, 6, 6);
                        //List<CMOPoint> Edges = GetEdgess(points, cmo.imgSize.Width, cmo.imgSize.Height);
                        Pen p = new Pen(Colors[c]);
                        p.Width = 8.0F;
                        //g.DrawRectangle(p, getBounds(points));                        
                        //g.DrawLine(Pens.Black, System[i].Center.X - 2, System[i].Center.Y - 2, System[i].Center.X + 2, System[i].Center.Y + 2);
                        //g.DrawLine(Pens.Black, System[i].Center.X - 2, System[i].Center.Y + 2, System[i].Center.X + 2, System[i].Center.Y - 2);
                        c++;
                        if (c >= Colors.Count())
                            c = 0;
                    }
                //c = 0;
                //for (int i=0; i<System.Count; i++)
                //{
                //    using (Graphics g = Graphics.FromImage(CMOGraphicalRepresentation))
                //        g.DrawString(Convert.ToString(i) + "." + Colors[c].Name, new Font(new FontFamily("Arial"), 8, FontStyle.Bold), Brushes.Black, new Point(System[i].Center.X, System[i].Center.Y));
                //    c++;
                //    if (c >= Colors.Count)
                //        c = 0;
                //}                    
            }
            pbCMOClustering.Image = CMOGraphicalRepresentation;
        }        

        private void Form1_Load(object sender, EventArgs e)
        {
            Globale.SeConnecter();
            txtRatio.Text = "1";
        }

        List<Planet> CMOResults = null;

        //Learning Algorithm
        private void btnLearn_Click(object sender, EventArgs e)
        {
            if (cbFailureType.SelectedIndex == 0)
                MessageBox.Show("Please select failure type for learning.");
            else
            {
                Learning learning = new Learning(CMOResults, cbFailureType.Text);
                dgl.Rows.Clear();
                if (LearningResults != null)
                    for (int i = 0; i < LearningResults.Count; i++)
                        dgl.Rows.Add(new string[] { LearningResults[i] });
                tabControl1.SelectTab(0);
            }
        }
        List<Planet> test = null;     
        private void button1_Click_1(object sender, EventArgs e)
        {
           
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Confirmation : \n Do you really want to clear the knowledge base? \n This action is irreversible.", "Clear the knowledge base", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                Globale.Query("delete from color_changes");
                Globale.Query("delete from color_patterns");
                Globale.Query("delete from edges");
                Globale.Query("delete from failure_types");
                Globale.Query("delete from patterns");
                Globale.Query("delete from shapes");

                MessageBox.Show("Knowledge base cleared.");
            }
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            Learning learning = new Learning(CMOResults, cbFailureType.Text, true);
            if (TestResults != null)
                for (int i = 0; i < TestResults.Count; i++)
                    dgt.Rows.Add(new string[] { TestResults[i] });
            tabControl1.SelectTab(1);
        }

        //Test Algorithm

    }
    //Point with: x, y and color (needed for CMO class)
    public class CMOPoint
    {
        public int x = 0;
        public int y = 0;
        public Color color = Color.White;
        public int planetIndex = -1;
        public List<int> visitedPlanets = null;

        public CMOPoint(int _x, int _y, Color _color)
        {
            x = _x;
            y = _y;
            color = _color;
            visitedPlanets = new List<int>();
        }
    }    
}
