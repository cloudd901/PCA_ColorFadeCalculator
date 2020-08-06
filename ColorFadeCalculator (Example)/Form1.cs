namespace ColorFadeCalculator__Example_
{
    using PCAFFINITY;
    using System;
    using System.Diagnostics;
    using System.Drawing;
    using System.Threading;
    using System.Windows.Forms;

    public partial class Form1 : Form
    {
        private bool FadeSwitch;

        private Thread T;

        private bool ThreadRunning;

        public Form1()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "Stop")
            {
                ThreadRunning = false;
            }
            else
            {
                //I know this is not very elegant.
                //Just threw this together for the example.
                ThreadRunning = true;
                T = new Thread(() =>
                {
                    while (!this.IsDisposed)
                    {
                        if (!ThreadRunning) { break; }

                        this.Invoke((MethodInvoker)delegate { Fading(); });

                        if (!ThreadRunning) { break; }
                        Thread.Sleep(1000);
                        if (!ThreadRunning) { break; }
                        Thread.Sleep(1000);
                        if (!ThreadRunning) { break; }
                        Thread.Sleep(1000);
                        if (!ThreadRunning) { break; }
                    }

                    button1.Invoke((MethodInvoker)delegate { button1.Text = "Start"; button1.BackColor = Color.PaleGreen; });
                })
                { IsBackground = true };
                T.Start();
                button1.Text = "Stop";
                button1.BackColor = Color.Orange;
            }
        }

        private void Fading()
        {
            const int steps = 100;
            const int delay = 5;
            const int timeout = 2000;
            Color labelFore = label1.ForeColor;
            Color labelBack = label1.BackColor;
            Color formBack = this.BackColor;

            //Create a ColorFadeCalculator which holds all of your Color Steps.
            ColorFadeCalculator labelFade = new ColorFadeCalculator(labelFore, labelBack, steps);

            //For PictureBox Images, you will need to set the Transparencey option to True.
            //The fromColor can be set to anything Transparent.
            //If fromColor is set to Color.Transparent, you may notice a dark shade when fading.
            //Try to use a Transparent color of the Parent for better fading affect!
            ColorFadeCalculator photoFade = new ColorFadeCalculator(Color.FromArgb(0, formBack.R, formBack.G, formBack.B), formBack, steps, true);

            //Below is the code I use to Fade my text and images from Out to In.
            //This code is just an example so please excuse the mess.
            //===================================================================================
            Image photoImage = (Image)pictureBox1.Image.Clone();
            Stopwatch sw = new Stopwatch();
            sw.Start();

            //Fade OUT here
            for (int i = 0; i < labelFade.Steps.Count; i++)
            {
                label1.ForeColor = labelFade.Steps[i];

                if (photoImage != null)
                {
                    using Image img = (Bitmap)photoImage.Clone();
                    using Graphics g = Graphics.FromImage(img);
                    using SolidBrush myBrush = new SolidBrush(photoFade.Steps[i]);
                    g.FillRectangle(myBrush, 0, 0, img.Width, img.Height);
                    g.Save();
                    UpdatePhoto(img);
                }

                pictureBox1.Update();
                label1.Update();

                if (sw.ElapsedMilliseconds >= timeout)
                {
                    //Ensure last color is set after Fade Out
                    //Timer is used in case of slow computer.
                    label1.ForeColor = labelFade.Steps[labelFade.Steps.Count - 1];
                    if (photoImage != null)
                    {
                        using Image img = (Bitmap)photoImage.Clone();
                        using Graphics g = Graphics.FromImage(img);
                        using SolidBrush myBrush = new SolidBrush(photoFade.Steps[photoFade.Steps.Count - 1]);
                        g.FillRectangle(myBrush, 0, 0, img.Width, img.Height);
                        g.Save();
                        UpdatePhoto(img);
                    }

                    pictureBox1.Update();
                    label1.Update();
                    break;
                }

                //I use a delay to extend the fading time without extending the Steps
                if (delay > 0)
                {
                    long check = sw.ElapsedMilliseconds;
                    while (sw.ElapsedMilliseconds < check + delay)
                    {
                        Thread.Sleep(0);
                    }
                }
            }

            sw.Stop();
            photoImage?.Dispose();

            //Here we change what we need to after everything is Faded Out.
            label1.Text = FadeSwitch ? "This text fades as all things do." : "See, I tried to tell you.";
            pictureBox1.Image = FadeSwitch ? Properties.Resources.spots : Properties.Resources.ChaosForest2;
            FadeSwitch = !FadeSwitch;

            photoImage = (Image)pictureBox1.Image.Clone();
            sw.Reset();
            sw.Start();

            //And now we Fade In the same way we Faded Out.
            //But counting in reverse.
            for (int i = labelFade.Steps.Count - 1; i >= 0; i--)
            {
                label1.ForeColor = labelFade.Steps[i];

                if (photoImage != null)
                {
                    using Image img = (Bitmap)photoImage.Clone();
                    using Graphics g = Graphics.FromImage(img);
                    using SolidBrush myBrush = new SolidBrush(photoFade.Steps[i]);
                    g.FillRectangle(myBrush, 0, 0, img.Width, img.Height);
                    g.Save();
                    UpdatePhoto(img);
                }

                pictureBox1.Update();
                label1.Update();

                if (sw.ElapsedMilliseconds >= timeout)
                {
                    label1.ForeColor = labelFade.Steps[0];
                    if (photoImage != null)
                    {
                        using Image img = (Bitmap)photoImage.Clone();
                        using Graphics g = Graphics.FromImage(img);
                        using SolidBrush myBrush = new SolidBrush(photoFade.Steps[0]);
                        g.FillRectangle(myBrush, 0, 0, img.Width, img.Height);
                        g.Save();
                        UpdatePhoto(img);
                    }

                    pictureBox1.Update();
                    label1.Update();
                    break;
                }

                if (delay > 0)
                {
                    long check = sw.ElapsedMilliseconds;
                    while (sw.ElapsedMilliseconds < check + delay)
                    {
                        Thread.Sleep(0);
                    }
                }
            }

            sw.Stop();
            photoImage?.Dispose();
        }

        private void UpdatePhoto(Image image)
        {
            using Bitmap newImage = (Bitmap)image;
            pictureBox1.Image?.Dispose();
            pictureBox1.Image = (Image)newImage.Clone();
        }
    }
}