
using NAudio.CoreAudioApi;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Ports;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ControlGuiLed
{

    public partial class Form1 : Form
    {

        public const int BAUD_RATE = 115200;
        // Serial port name
        public const string PORT_NAME = "COM3";
        
        // Number of leds constant
        private const int LEDNUM = 178;
        // Control serial recieve codes
        public const int CONTROL_POWER_SEND_CODE = 0;
        public const int CONTROL_VOL_UP_SEND_CODE = 1;
        public const int CONTROL_FUNC_STOP_SEND_CODE = 2;
        public const int CONTROL_BACKWARDS_SEND_CODE = 3;
        public const int CONTROL_PLAY_PAUSE_SEND_CODE = 4;
        public const int CONTROL_FORWARD_SEND_CODE = 5;
        public const int CONTROL_DOWN_SEND_CODE = 6;
        public const int CONTROL_VOL_DOWN_SEND_CODE = 7;
        public const int CONTROL_UP_SEND_CODE = 8;
        public const int CONTROL_0_SEND_CODE = 9;
        public const int CONTROL_EQ_SEND_CODE = 10;
        public const int CONTROL_ST_REPT_SEND_CODE = 11;
        public const int CONTROL_1_SEND_CODE = 12;
        public const int CONTROL_2_SEND_CODE = 13;
        public const int CONTROL_3_SEND_CODE = 14;
        public const int CONTROL_4_SEND_CODE = 15;
        public const int CONTROL_5_SEND_CODE = 16;
        public const int CONTROL_6_SEND_CODE = 17;
        public const int CONTROL_7_SEND_CODE = 18;
        public const int CONTROL_8_SEND_CODE = 19;
        public const int CONTROL_9_SEND_CODE = 20;
        public const byte COLOR_RECV_CODE = 22;
        public const byte DYNAMIC_RECV_CODE = 23;
        public const byte PIXEL_RECV_CODE = 24;
        public const byte BRIGHTNESS_RECV_CODE = 25;
        public const byte WAITING_RECV_CODE = 26;
        public const byte OFF_RECV_CODE = 27;

        private SerialPort _serialPort;

        public BlockingCollection<byte[]> SerialWriteQueue = new BlockingCollection<byte[]>();
        
        private bool controlNumFunc = false;
        private byte ledBrightness = 200;
        private int rainbowLastH = 0;
        private Rectangle[] ambilightRectangles;
        private Color OFF_COLOR = Color.Black;
        private LedMode ledMode = LedMode.Color;
        private Thread readThread;
        private Thread writeThread;
        private ScreenAmbilightRegions division;
        // Audio device
        private MMDevice device;
        private bool spectogramBrightness = false;
        private int partyLastH = 0;
        //private Task ambilightTask = null;

        public Color[] OFF_COLOR_ARR = new Color[LEDNUM];

        public Form1()
        {
            InitializeComponent();
            for (int i = 0; i < OFF_COLOR_ARR.Length; i++)
            {
                OFF_COLOR_ARR[i] = OFF_COLOR;
            }
            panel1.BackColor = OFF_COLOR;
            colorDialog1.Color = OFF_COLOR;
            ContextMenu contextMenu = new ContextMenu();
            MenuItem exit = new MenuItem();
            exit.Text = "Exit";
            exit.Click += new EventHandler(Exit_Click);
            contextMenu.MenuItems.Add(exit);
            notifyIcon1.ContextMenu = contextMenu;
            division = new ScreenAmbilightRegions();
            ambilightRectangles = division.getRectangles();
            MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
            device = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Console);
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnShutdown);
            BrightnessBar.Value = ledBrightness;
            SerialStart();
        }

        // Turn Off led on shutdown
        private void OnShutdown(object sender, EventArgs e)
        {
            WriteLedColorMode(OFF_COLOR, 0);
            Environment.Exit(0);
        }
        // Make form invisible
        protected override void SetVisibleCore(bool value)
        {
            if (!this.IsHandleCreated)
            {
                this.CreateHandle();
                value = false;
            }
            base.SetVisibleCore(value);
        }
        // Start serial write and read Threads
        public void SerialStart()
        {
            readThread = new Thread(Read);
            readThread.Start();
            writeThread = new Thread(Write);
            writeThread.Start();
        }
        // Read Thread method
        public void Read()
        {
            while (true)
            {
                try
                {
                    int message = _serialPort.ReadByte();
                    Console.WriteLine(message);
                    switch (message)
                    {
                        case CONTROL_POWER_SEND_CODE:
                            controlNumFunc = !controlNumFunc;
                            break;
                        case CONTROL_VOL_UP_SEND_CODE:
                            KeyBD.keybd_event((byte)Keys.VolumeUp, 0, 0, 0);
                            break;
                        case CONTROL_FUNC_STOP_SEND_CODE:
                            KeyBD.keybd_event((byte)Keys.Escape, 0, 0, 0);
                            break;
                        case CONTROL_BACKWARDS_SEND_CODE:
                            KeyBD.keybd_event((byte)Keys.MediaPreviousTrack, 0, 0, 0);
                            break;
                        case CONTROL_PLAY_PAUSE_SEND_CODE:
                            KeyBD.keybd_event((byte)Keys.MediaPlayPause, 0, 0, 0);
                            break;
                        case CONTROL_FORWARD_SEND_CODE:
                            KeyBD.keybd_event((byte)Keys.MediaNextTrack, 0, 0, 0);
                            break;
                        case CONTROL_DOWN_SEND_CODE:
                            KeyBD.keybd_event((byte)Keys.Down, 0, 0, 0);
                            break;
                        case CONTROL_VOL_DOWN_SEND_CODE:
                            KeyBD.keybd_event((byte)Keys.VolumeDown, 0, 0, 0);
                            break;
                        case CONTROL_UP_SEND_CODE:
                            KeyBD.keybd_event((byte)Keys.Up, 0, 0, 0);
                            break;
                        case CONTROL_0_SEND_CODE:
                            //Haha rickroll
                            //System.Diagnostics.Process.Start("https://www.youtube.com/watch?v=dQw4w9WgXcQ");
                            if (!controlNumFunc)
                                KeyBD.keybd_event((byte)Keys.D0, 0, 0, 0);
                            else
                            {

                            }
                            break;
                        case CONTROL_EQ_SEND_CODE:
                            KeyBD.keybd_event((byte)Keys.Tab, 0, 0, 0);
                            break;
                        case CONTROL_ST_REPT_SEND_CODE:
                            KeyBD.keybd_event((byte)Keys.Enter, 0, 0, 0);
                            break;
                        case CONTROL_1_SEND_CODE:

                            if (!controlNumFunc)
                                KeyBD.keybd_event((byte)Keys.D1, 0, 0, 0);
                            else
                            {

                            }
                            break;
                        case CONTROL_2_SEND_CODE:
                            if (!controlNumFunc)
                                KeyBD.keybd_event((byte)Keys.D2, 0, 0, 0);
                            else
                            {

                            }
                            break;
                        case CONTROL_3_SEND_CODE:
                            if (!controlNumFunc)
                                KeyBD.keybd_event((byte)Keys.D3, 0, 0, 0);
                            else
                            {

                            }
                            break;
                        case CONTROL_4_SEND_CODE:
                            if (!controlNumFunc)
                                KeyBD.keybd_event((byte)Keys.D4, 0, 0, 0);
                            else
                            {
                                KeyBD.keybd_event((byte)Keys.Left, 0, 0, 0);
                            }
                            break;
                        case CONTROL_5_SEND_CODE:
                            if (!controlNumFunc)
                                KeyBD.keybd_event((byte)Keys.D5, 0, 0, 0);
                            else
                            {

                            }
                            break;
                        case CONTROL_6_SEND_CODE:
                            if (!controlNumFunc)
                                KeyBD.keybd_event((byte)Keys.D6, 0, 0, 0);
                            else
                            {
                                KeyBD.keybd_event((byte)Keys.Right, 0, 0, 0);
                            }
                            break;
                        case CONTROL_7_SEND_CODE:
                            if (!controlNumFunc)
                                KeyBD.keybd_event((byte)Keys.D7, 0, 0, 0);
                            else
                            {

                            }
                            break;
                        case CONTROL_8_SEND_CODE:
                            if (!controlNumFunc)
                                KeyBD.keybd_event((byte)Keys.D8, 0, 0, 0);
                            else
                            {

                            }
                            break;
                        case CONTROL_9_SEND_CODE:
                            if (!controlNumFunc)
                                KeyBD.keybd_event((byte)Keys.D9, 0, 0, 0);
                            else
                            {

                            }
                            break;

                    }


                }
                catch (TimeoutException) { }
                catch (IOException)
                {
                    ConnectSerialPort();
                }
                catch (NullReferenceException)
                {
                    ConnectSerialPort();
                }
            }
        }
        // Write thread method
        public void Write()
        {
            while (true)
            {
                byte[] data = SerialWriteQueue.Take();
                if (_serialPort.IsOpen)
                    _serialPort.Write(data, 0, data.Length);

            }
        }
        private void StopAllLedOperations()
        {
            PartyTimer.Stop();
            ColorTimer.Stop();
            AmbilightTimer.Stop();
            RainbowTimer.Stop();
        }
        private void ConnectSerialPort()
        {
            while (true)
            {
                try
                {
                    _serialPort = new SerialPort(PORT_NAME, BAUD_RATE);
                    _serialPort.Handshake = Handshake.None;

                    _serialPort.Open();
                    Invoke(new Action(() => { notifyIcon1.Text = "Control (Connected)"; label1.Text = "Connected"; }));
                    break;
                }
                catch (IOException)
                {
                    Invoke(new Action(() => { notifyIcon1.Text = "Control (Disconnected)"; label1.Text = "Disconnected"; }));
                }
                Thread.Sleep(1000);
            }
        }

        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
            {
                if (!this.Visible)
                    Show();
                else
                {
                    Visible = false;
                }
            }
        }
        private void Exit_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);

        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Visible = false;
        }

        private void ColorButton_Click(object sender, EventArgs e)
        {

            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                panel1.BackColor = colorDialog1.Color;
                StopAllLedOperations();
                ledMode = LedMode.Color;
                WriteLedColorMode(panel1.BackColor);
                UngrayAllButtons();
                ColorButton.BackColor = Color.Gray;
                ColorTimer.Start();
            }


        }
        // Write to arduino methods
        public void WriteLedColorMode(Color color)
        {
            byte[] data = new byte[] { COLOR_RECV_CODE, ledBrightness, color.R, color.G, color.B };
            SerialWriteQueue.Add(data);
        }
        public void WriteLedColorMode(Color color, byte brightness)
        {
            byte[] data = new byte[] { COLOR_RECV_CODE, brightness, color.R, color.G, color.B };
            SerialWriteQueue.Add(data);
        }

        public void WriteLedDynamicMode(Color[] color)
        {
            byte[] data = new byte[LEDNUM * 3 + 2];
            data[0] = DYNAMIC_RECV_CODE;
            data[1] = ledBrightness;
            int j = 0;
            for (int i = 2; i < data.Length; i += 3)
            {
                data[i] = color[j].R;
                data[i + 1] = color[j].G;
                data[i + 2] = color[j].B;
                j++;
            }
            SerialWriteQueue.Add(data);
        }
        public void WriteLedDynamicMode(Color[] color, byte brightness)
        {
            byte[] data = new byte[LEDNUM * 3 + 2];
            data[0] = DYNAMIC_RECV_CODE;
            data[1] = brightness;
            int j = 0;
            for (int i = 2; i < data.Length; i += 3)
            {
                data[i] = color[j].R;
                data[i + 1] = color[j].G;
                data[i + 2] = color[j].B;
                j++;
            }
            SerialWriteQueue.Add(data);
        }
        //Turn off led
        public void LedOff()
        {
            StopAllLedOperations();
            panel1.BackColor = OFF_COLOR;
            WriteLedColorMode(OFF_COLOR, 0);
            UngrayAllButtons();
            OffButton.BackColor = Color.Gray;
        }
        private void OffButton_Click(object sender, EventArgs e)
        {
            LedOff();
        }


        private void BrightnessBar_ValueChanged(object sender, EventArgs e)
        {

        }

        private void BrightnessBar_MouseCaptureChanged(object sender, EventArgs e)
        {
            ledBrightness = (byte)BrightnessBar.Value;
            if (ledMode == LedMode.Color)
            {
                WriteLedColorMode(panel1.BackColor);
            }
        }
        private byte getSpectogramBrightness()
        {
            double lvl = device.AudioMeterInformation.MasterPeakValue;
            double mul = (lvl / (ledBrightness / 255.0 * 1.0));
            return (byte)MathTools.Clamp((int)(255 * mul), 0, 255);
        }
        private void RainbowTimer_Tick(object sender, EventArgs e)
        {
            if (spectogramBrightness)
            {
                WriteLedColorMode(ColorTools.ColorFromHSV(rainbowLastH, 1, 1), getSpectogramBrightness());
            }
            else
            {
                WriteLedColorMode(ColorTools.ColorFromHSV(rainbowLastH, 1, 1));
            }
            rainbowLastH++;
            if (rainbowLastH >= 360)
                rainbowLastH = 0;
        }
        // Update Ambilight led mode
        private void AmbilightTimer_Tick(object sender, EventArgs e)
        {
            //if (ambilightTask == null || ambilightTask?.Status == TaskStatus.RanToCompletion) {
            //ambilightTask?.Dispose();
            Color[] pixels = new Color[LEDNUM];
            //ambilightTask = Task.Run(() => {
            Bitmap image = ColorTools.GetImageFromScreen(new Rectangle(0, 0, division.Width, division.Height));
            if (image != null)
            {


                BmpPixelSnoop bmp = new BmpPixelSnoop(image);
                for (int l = 0; l < ambilightRectangles.Length; l++)
                {

                    long rSum = 0;
                    long gSum = 0;
                    long bSum = 0;
                    int num = 0;
                    for (int i = 0; i < ambilightRectangles[l].Width; i += 8)
                    {
                        for (int j = 0; j < ambilightRectangles[l].Height; j += 8)
                        {

                            Color pixel = bmp.GetPixel(i + ambilightRectangles[l].Location.X, j + ambilightRectangles[l].Location.Y);
                            rSum += pixel.R;
                            gSum += pixel.G;
                            bSum += pixel.B;
                            num++;
                        }
                    }

                    pixels[l] = Color.FromArgb((int)(rSum / num), (int)(gSum / num), (int)(bSum / num));


                }
                bmp.Dispose();

                /*for (int l = 0; l < ambilightRectangles.Length; l++)
                {

                    long rSum = 0;
                    long gSum = 0;
                    long bSum = 0;
                    int num = 0;
                    for (int i = 0; i < ambilightRectangles[l].Width; i += 8)
                    {
                        for (int j = 0; j < ambilightRectangles[l].Height; j += 8)
                        {

                            Color pixel = image.GetPixel(i + ambilightRectangles[l].Location.X, j + ambilightRectangles[l].Location.Y);
                            rSum += pixel.R;
                            gSum += pixel.G;
                            bSum += pixel.B;
                            num++;
                        }
                    }

                    pixels[l] = Color.FromArgb((int)(rSum / num), (int)(gSum / num), (int)(bSum / num));


                }
                */

                image.Dispose();
            }


            //Invoke(new Action(() => {
            if (spectogramBrightness)
            {
                WriteLedDynamicMode(pixels, getSpectogramBrightness());
            }
            else
            {
                WriteLedDynamicMode(pixels);
            }
            //}));

            //});

            //}
        }


        private void RainbowButton_Click(object sender, EventArgs e)
        {
            StopAllLedOperations();
            rainbowLastH = 0;
            RainbowTimer.Start();
            ledMode = LedMode.Rainbow;
            UngrayAllButtons();
            RainbowButton.BackColor = Color.Gray;
        }
        
        private void AmbilightButton_Click(object sender, EventArgs e)
        {
            StopAllLedOperations();
            AmbilightTimer.Start();
            ledMode = LedMode.Ambilight;
            UngrayAllButtons();
            AmbilightButton.BackColor = Color.Gray;

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void UngrayAllButtons()
        {
            AmbilightButton.BackColor = Color.LightGray;
            ColorButton.BackColor = Color.LightGray;
            OffButton.BackColor = Color.LightGray;
            RainbowButton.BackColor = Color.LightGray;
            PartyButton.BackColor = Color.LightGray;
        }
        // Update Color led mode
        private void ColorTimer_Tick(object sender, EventArgs e)
        {
            if (spectogramBrightness)
            {
                WriteLedColorMode(panel1.BackColor, getSpectogramBrightness());
            }
        }

        private void SpectogramButton_Click(object sender, EventArgs e)
        {
            spectogramBrightness = !spectogramBrightness;
            SpectogramButton.BackColor = spectogramBrightness ? Color.Gray : Color.LightGray;
            if (!spectogramBrightness && ledMode == LedMode.Color)
                WriteLedColorMode(panel1.BackColor);
        }

        private void Party_Click(object sender, EventArgs e)
        {
            StopAllLedOperations();
            partyLastH = 0;
            PartyTimer.Start();
            ledMode = LedMode.Party;
            UngrayAllButtons();
            PartyButton.BackColor = Color.Gray;
        }
        // Update Party led mode
        private void PartyTimer_Tick(object sender, EventArgs e)
        {
            if (spectogramBrightness)
            {
                int b = getSpectogramBrightness();
                WriteLedColorMode(ColorTools.ColorFromHSV((partyLastH + b) % 360, 1, 1), (byte)b);
                partyLastH += 3;
                if (partyLastH >= 360)
                    partyLastH = partyLastH - 360;
            }
            else
            {
                WriteLedColorMode(ColorTools.ColorFromHSV(partyLastH, 1, 1));
                partyLastH += 37;
                if (partyLastH >= 360)
                    partyLastH = partyLastH - 360;
            }

        }
        // Reset Audio Device
        private void ResetAudioB_Click(object sender, EventArgs e)
        {
            MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
            device = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Console);
        }
        // Turbo button press
        private void AmbilightTurboC_CheckedChanged(object sender, EventArgs e)
        {
            if (AmbilightTurboC.Checked)
            {
                AmbilightTimer.Interval = 60;
            }
            else
            {
                AmbilightTimer.Interval = 80;
            }
        }
    }
    // Class to send keystrokes to the PC
    public class KeyBD
    {
        [DllImport("user32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

    }
   
    enum LedMode
    {
        Off,
        Color,
        Rainbow,
        Ambilight,
        Party
    }
    public static class ColorTools
    {
        public static Color ColorFromHSV(double hue, double saturation, double value)
        {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value = value * 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - f * saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

            if (hi == 0)
                return Color.FromArgb(255, v, t, p);
            else if (hi == 1)
                return Color.FromArgb(255, q, v, p);
            else if (hi == 2)
                return Color.FromArgb(255, p, v, t);
            else if (hi == 3)
                return Color.FromArgb(255, p, q, v);
            else if (hi == 4)
                return Color.FromArgb(255, t, p, v);
            else
                return Color.FromArgb(255, v, p, q);
        }
        public static Bitmap GetImageFromScreen(Rectangle rect)
        {
            Bitmap bmp = new Bitmap(rect.Width, rect.Height, PixelFormat.Format32bppArgb);
            try
            {
                using (Graphics g = Graphics.FromImage(bmp))
                    g.CopyFromScreen(rect.Left, rect.Top, 0, 0, bmp.Size, CopyPixelOperation.SourceCopy);
            }
            catch (Win32Exception)
            {
                return null;
            }
            return bmp;
        }
    }
    public static class MathTools
    {
        public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
        {
            if (val.CompareTo(min) < 0) return min;
            else if (val.CompareTo(max) > 0) return max;
            else return val;
        }
    }
    // Class to divide the screen into Rectangle regions for taking Ambilight pixel data
    public class ScreenAmbilightRegions
    {
        int rectangleWidth;
        int rectangleHeight;
        int rectangleWidthBoundry;
        int rectangleSpace;
        int rectangleNum = 57;
        int verticalRectangleWidth;
        int verticalRectangleHeight;
        int verticalRectangleHeightBoundry;
        int verticalRectangleSpace;
        int verticalRectangleNum = 32;
        double verticalRectangleOffset = 0.8;
        double rectangleOffset = 0.8;

        public int Height { get; private set; }
        public int Width { get; private set; }

        public Rectangle[] getRectangles()
        {
            Height = Screen.PrimaryScreen.Bounds.Height;
            Width = Screen.PrimaryScreen.Bounds.Width;
            rectangleWidth = Width / 80;
            rectangleHeight = Height / 29;
            rectangleWidthBoundry = Width / 38;
            rectangleSpace = (Width - ((2 * rectangleWidthBoundry) + (rectangleNum * rectangleWidth))) / rectangleNum;


            verticalRectangleWidth = Width / 40;
            verticalRectangleHeight = Height / 58;
            verticalRectangleHeightBoundry = Height / 34;
            verticalRectangleSpace = (Height - (2 * verticalRectangleHeightBoundry) - (verticalRectangleNum * verticalRectangleHeight)) / verticalRectangleNum;
            List<Rectangle> rectangles1 = new List<Rectangle>();
            List<Rectangle> rectangles2 = new List<Rectangle>();
            List<Rectangle> rectangles3 = new List<Rectangle>();
            List<Rectangle> rectangles4 = new List<Rectangle>();
            // Create rectangles for each of the 4 edges of the screen
            for (int i = 0; i < rectangleNum; i++)
            {
                Rectangle rec = new Rectangle((int)Math.Round(rectangleWidthBoundry + (i * (rectangleWidth + rectangleSpace + rectangleOffset))), 0, rectangleWidth, rectangleHeight);
                rectangles1.Add(rec);

            }
            for (int i = 0; i < rectangleNum; i++)
            {
                Rectangle rec = new Rectangle((int)Math.Round(rectangleWidthBoundry + (i * (rectangleWidth + rectangleSpace + rectangleOffset))), Height - rectangleHeight - 1, rectangleWidth, rectangleHeight);
                rectangles2.Add(rec);

            }
            for (int i = 0; i < verticalRectangleNum; i++)
            {
                Rectangle rec = new Rectangle(0, (int)Math.Round(verticalRectangleHeightBoundry + (i * (verticalRectangleHeight + verticalRectangleSpace + verticalRectangleOffset))), verticalRectangleWidth, verticalRectangleHeight);
                rectangles3.Add(rec);

            }
            for (int i = 0; i < verticalRectangleNum; i++)
            {
                Rectangle rec = new Rectangle(Width - verticalRectangleWidth - 1, (int)Math.Round(verticalRectangleHeightBoundry + (i * (verticalRectangleHeight + verticalRectangleSpace + verticalRectangleOffset))), verticalRectangleWidth, verticalRectangleHeight);
                rectangles4.Add(rec);

            }
            rectangles4.Reverse();
            rectangles1.Reverse();
            List<Rectangle> rectangles = new List<Rectangle>(rectangles4);
            rectangles.AddRange(rectangles1);
            rectangles.AddRange(rectangles3);
            rectangles.AddRange(rectangles2);
            return rectangles.ToArray();
        }
    }

    unsafe class BmpPixelSnoop : IDisposable
    {
        // A reference to the bitmap to be wrapped
        private readonly Bitmap wrappedBitmap;

        // The bitmap's data (once it has been locked)
        private BitmapData data = null;

        // Pointer to the first pixel
        private readonly byte* scan0;

        // Number of bytes per pixel
        private readonly int depth;

        // Number of bytes in an image row
        private readonly int stride;

        // The bitmap's width
        private readonly int width;

        // The bitmap's height
        private readonly int height;

        /// 

        /// Constructs a BmpPixelSnoop object, the bitmap
        /// object to be wraped is passed as a parameter.
        /// 

        /// The bitmap to snoop
        public BmpPixelSnoop(Bitmap bitmap)
        {
            wrappedBitmap = bitmap ?? throw new ArgumentException("Bitmap parameter cannot be null", "bitmap");

            // Currently works only for: PixelFormat.Format32bppArgb
            if (wrappedBitmap.PixelFormat != PixelFormat.Format32bppArgb)
                throw new System.ArgumentException("Only PixelFormat.Format32bppArgb is supported", "bitmap");

            // Record the width & height
            width = wrappedBitmap.Width;
            height = wrappedBitmap.Height;

            // So now we need to lock the bitmap so that we can gain access
            // to it's raw pixel data.  It will be unlocked when this snoop is 
            // disposed.
            var rect = new Rectangle(0, 0, wrappedBitmap.Width, wrappedBitmap.Height);

            try
            {
                data = wrappedBitmap.LockBits(rect, ImageLockMode.ReadWrite, wrappedBitmap.PixelFormat);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Could not lock bitmap, is it already being snooped somewhere else?", ex);
            }

            // Calculate number of bytes per pixel
            depth = Bitmap.GetPixelFormatSize(data.PixelFormat) / 8; // bits per channel

            // Get pointer to first pixel
            scan0 = (byte*)data.Scan0.ToPointer();

            // Get the number of bytes in an image row
            // this will be used when determining a pixel's
            // memory address.
            stride = data.Stride;
        }

        /// 

        /// Disposes BmpPixelSnoop object
        /// 

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// 

        /// Disposes BmpPixelSnoop object, we unlock
        /// the wrapped bitmap.
        /// 

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (wrappedBitmap != null)
                    wrappedBitmap.UnlockBits(data);
            }
            // free native resources if there are any.
        }

        /// 

        /// Calculate the pointer to a pixel at (x, x)
        /// 

        /// The pixel's x coordinate
        /// The pixel's y coordinate
        /// A byte* pointer to the pixel's data
        private byte* PixelPointer(int x, int y)
        {
            return scan0 + y * stride + x * depth;
        }

        /// 

        /// Snoop's implemetation of GetPixel() which is similar to
        /// Bitmap's GetPixel() but should be faster.
        /// 

        /// The pixel's x coordinate
        /// The pixel's y coordinate
        /// The pixel's colour
        public System.Drawing.Color GetPixel(int x, int y)
        {
            // Better do the 'decent thing' and bounds check x & y
            if (x < 0 || y < 0 || x >= width || y >= width)
                throw new ArgumentException("x or y coordinate is out of range");

            int a, r, g, b;

            // Get a pointer to this pixel
            byte* p = PixelPointer(x, y);

            // Pull out its colour data
            b = *p++;
            g = *p++;
            r = *p++;
            a = *p;

            // And return a color value for it (this is quite slow
            // but allows us to look like Bitmap.GetPixel())
            return System.Drawing.Color.FromArgb(a, r, g, b);
        }

        /// 

        /// Sets the passed colour to the pixel at (x, y)
        /// 

        /// The pixel's x coordinate
        /// The pixel's y coordinate
        /// The value to be assigned to the pixel
        public void SetPixel(int x, int y, System.Drawing.Color col)
        {
            // Better do the 'decent thing' and bounds check x & y
            if (x < 0 || y < 0 || x >= width || y >= width)
                throw new ArgumentException("x or y coordinate is out of range");

            // Get a pointer to this pixel
            byte* p = PixelPointer(x, y);

            // Set the data
            *p++ = col.B;
            *p++ = col.G;
            *p++ = col.R;
            *p = col.A;
        }

        /// 

        /// The bitmap's width
        /// 

        public int Width { get { return width; } }

        // The bitmap's height
        public int Height { get { return height; } }
    }

}

