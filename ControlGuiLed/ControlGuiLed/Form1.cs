
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

        

        // Number of leds constant
        private const int LEDNUM = 178;
        // Control serial recieve codes




        private byte ledBrightness = 200;
        private int rainbowLastH = 0;
        private Rectangle[] ambilightRectangles;
        private Color OFF_COLOR = Color.Black;
        private LedMode ledMode = LedMode.Color;
        
        private ScreenAmbilightRegions division;
        // Audio device
        private MMDevice device;
        private bool spectogramBrightness = false;
        private int partyLastH = 0;
        //private Task ambilightTask = null;
        private System.Threading.Timer AmbilightTimer;
        private int AmbilightTimerRegularInterval = 60;
        private int AmbilightTurboTimerInterval = 40;
        private int AmbilightTimerInterval = 60;
        private int AmbilightTimerDue = Timeout.Infinite;
        private System.Threading.Timer RainbowTimer;
        private int RainbowTimerInterval = 60;
        private System.Threading.Timer ColorTimer;
        private int ColorTimerInterval = 40;
        private System.Threading.Timer PartyTimer;
        private int PartyTimerInterval = 60;
        public Color[] OFF_COLOR_ARR = new Color[LEDNUM];
        private AutoResetEvent AmbilightTimerFinishedEvent = new AutoResetEvent(false);
        private SerialManager serialManager;
        public Form1()
        {
            InitializeComponent();
            AmbilightTimer = new System.Threading.Timer((o) => { AmbilightTimer_Tick(); });
            RainbowTimer = new System.Threading.Timer((o) => { RainbowTimer_Tick(); });
            ColorTimer = new System.Threading.Timer((o) => { ColorTimer_Tick(); });
            PartyTimer = new System.Threading.Timer((o) => { PartyTimer_Tick(); });
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
            serialManager = new SerialManager();
            serialManager.SerialStart();
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

        // Read Thread method

        // Write thread method


        public void SetConnected(bool connected)
        {
            if (connected)
            {
                
                this.Invoke((MethodInvoker)delegate
                {
                    notifyIcon1.Text = "Control (Connected)"; label1.Text = "Connected";
                    this.Text = "ControlGuiLed - Connected";
                    
                });
            }
            else
            {
                this.Invoke((MethodInvoker)delegate
                {
                    notifyIcon1.Text = "Control (Disconnected)"; label1.Text = "Disconnected";
                    this.Text = "ControlGuiLed - Disconnected";
                    
                });
            }
        }
        private void StopAllLedOperations()
        {
            PartyTimer.Change(Timeout.Infinite, Timeout.Infinite);
            ColorTimer.Change(Timeout.Infinite, Timeout.Infinite);
            AmbilightTimerDue = Timeout.Infinite;
            AmbilightTimer.Change(AmbilightTimerDue, Timeout.Infinite);
            if (ledMode == LedMode.Ambilight) { 
                AmbilightTimerFinishedEvent.Reset();
                AmbilightTimerFinishedEvent.WaitOne(500);
            }
            RainbowTimer.Change(Timeout.Infinite, Timeout.Infinite);
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
                ColorTimer.Change(0, ColorTimerInterval);
            }


        }
        // Write to arduino methods
        public void WriteLedColorMode(Color color)
        {
            byte[] data = new byte[] { SerialManager.COLOR_RECV_CODE, ledBrightness, color.R, color.G, color.B };
            serialManager.SerialWriteQueue.Add(data);
        }
        public void WriteLedColorMode(Color color, byte brightness)
        {
            byte[] data = new byte[] { SerialManager.COLOR_RECV_CODE, brightness, color.R, color.G, color.B };
            serialManager.SerialWriteQueue.Add(data);
        }

        public void WriteLedDynamicMode(Color[] color)
        {
            byte[] data = new byte[LEDNUM * 3 + 2];
            data[0] = SerialManager.DYNAMIC_RECV_CODE;
            data[1] = ledBrightness;
            int j = 0;
            for (int i = 2; i < data.Length; i += 3)
            {
                data[i] = color[j].R;
                data[i + 1] = color[j].G;
                data[i + 2] = color[j].B;
                j++;
            }
            serialManager.SerialWriteQueue.Add(data);
        }
        public void WriteLedDynamicMode(Color[] color, byte brightness)
        {
            byte[] data = new byte[LEDNUM * 3 + 2];
            data[0] = SerialManager.DYNAMIC_RECV_CODE;
            data[1] = brightness;
            int j = 0;
            for (int i = 2; i < data.Length; i += 3)
            {
                data[i] = color[j].R;
                data[i + 1] = color[j].G;
                data[i + 2] = color[j].B;
                j++;
            }
            serialManager.SerialWriteQueue.Add(data);
        }
        //Turn off led
        public void LedOff()
        {
            StopAllLedOperations();
            ledMode = LedMode.Off;
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
            AutoResetEvent _event = new AutoResetEvent(false);
            double lvl = 0;
            double mul = 0;
            this.Invoke
                (new Action(() =>
                {
                    lvl = device.AudioMeterInformation.MasterPeakValue;
                    mul = (lvl / (ledBrightness / 255.0));
                    _event.Set();
                }));
            _event.WaitOne();
            return (byte)MathTools.Clamp((int)(255 * mul), 0, 255);
        }
        private void RainbowTimer_Tick()
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
        private void AmbilightTimer_Tick()
        {
            //if (ambilightTask == null || ambilightTask?.Status == TaskStatus.RanToCompletion) {
            //ambilightTask?.Dispose();
            Color[] pixels = new Color[LEDNUM];
            //ambilightTask = Task.Run(() => {
            using (Bitmap image = ColorTools.GetImageFromScreen(new Rectangle(0, 0, division.Width, division.Height)))
            {
                if (image != null)
                {


                    using (BmpPixelSnoop bmp = new BmpPixelSnoop(image))
                    {
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
                    }

                    

                    
                }
                AmbilightTimerFinishedEvent.Set();
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
            RainbowTimer.Change(0, RainbowTimerInterval);
            ledMode = LedMode.Rainbow;
            UngrayAllButtons();
            RainbowButton.BackColor = Color.Gray;
        }

        private void AmbilightButton_Click(object sender, EventArgs e)
        {
            StopAllLedOperations();
            AmbilightTimerDue = 0;
            AmbilightTimer.Change(AmbilightTimerDue, AmbilightTimerInterval);
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
        private void ColorTimer_Tick()
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
            PartyTimer.Change(0, PartyTimerInterval);
            ledMode = LedMode.Party;
            UngrayAllButtons();
            PartyButton.BackColor = Color.Gray;
        }
        // Update Party led mode
        private void PartyTimer_Tick()
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
                AmbilightTimerInterval = AmbilightTurboTimerInterval;
            }
            else
            {
                AmbilightTimerInterval = AmbilightTimerRegularInterval;
            }
            AmbilightTimer.Change(AmbilightTimerDue, AmbilightTimerInterval);
        }

    }

}

