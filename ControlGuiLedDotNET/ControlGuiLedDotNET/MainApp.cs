
using NAudio.CoreAudioApi;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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

    public partial class MainApp : Form
    {

        private int rainbowLastH = 0;
        private Rectangle[] ambilightRectangles;
        private LedMode ledMode = LedMode.Color;

        private ScreenAmbilightRegions division;
        // Audio device
        private MMDevice device;
        private bool spectogramBrightness = false;
        private int partyLastH = 0;
        //private Task ambilightTask = null;
        private UpdateTimer AmbilightTimer;
        private int AmbilightTimerInterval = 60;
        private int AmbilightTimerDue = Timeout.Infinite;
        private UpdateTimer RainbowTimer;
        private int RainbowTimerInterval = 60;
        private UpdateTimer ColorTimer;
        private int ColorTimerInterval = 40;
        private UpdateTimer PartyTimer;
        private int PartyTimerInterval = 60;

        private AutoResetEvent AmbilightTimerFinishedEvent = new AutoResetEvent(false);
        private SerialManager serialManager;
        private LedManager ledManager;
        public MainApp()
        {
            InitializeComponent();
            serialManager = new SerialManager(this);
            ledManager = new LedManager(serialManager);


            panel1.BackColor = LedManager.OFF_COLOR;
            colorDialog1.Color = LedManager.OFF_COLOR;
            ContextMenuStrip contextMenu = new ContextMenuStrip();
            ToolStripMenuItem exit = new ToolStripMenuItem();
            exit.Text = "Exit";
            exit.Click += new EventHandler(Exit_Click);
            contextMenu.Items.Add(exit);
            notifyIcon1.ContextMenuStrip = contextMenu;
            BrightnessBar.Value = ledManager.ledBrightness;

            division = new ScreenAmbilightRegions();
            ambilightRectangles = division.getRectangles();

            MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
            device = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Console);
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnShutdown);


            serialManager.SerialStart();
            AmbilightTimer = new UpdateTimer(UpdateTimer.CallbackType.Ambilight, this);
            RainbowTimer = new UpdateTimer(UpdateTimer.CallbackType.Rainbow, this);
            ColorTimer = new UpdateTimer(UpdateTimer.CallbackType.Color, this);
            PartyTimer = new UpdateTimer(UpdateTimer.CallbackType.Party, this);

        }

        // Turn Off led on shutdown
        private void OnShutdown(object sender, EventArgs e)
        {
            ledManager.WriteLedColorMode(LedManager.OFF_COLOR, 0);
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
            if (ledMode == LedMode.Ambilight)
            {
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
            StopAllLedOperations();
            ledManager.WriteLedColorMode(LedManager.OFF_COLOR, 0);
            Process.GetCurrentProcess().Kill();
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
                ledManager.WriteLedColorMode(panel1.BackColor);
                UngrayAllButtons();
                ColorButton.BackColor = Color.Gray;
                ColorTimer.Change(0, ColorTimerInterval);
            }


        }
        // Write to arduino methods

        //Turn off led
        public void LedOff()
        {
            StopAllLedOperations();
            ledMode = LedMode.Off;
            panel1.BackColor = LedManager.OFF_COLOR;
            ledManager.WriteLedColorMode(LedManager.OFF_COLOR, 0);
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
            ledManager.ledBrightness = (byte)BrightnessBar.Value;
            if (ledMode == LedMode.Color)
            {
                ledManager.WriteLedColorMode(panel1.BackColor);
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
                    mul = (lvl / (ledManager.ledBrightness / 255.0));
                    _event.Set();
                }));
            _event.WaitOne();
            return (byte)MathTools.Clamp((int)(255 * mul), 0, 255);
        }
        public void RainbowTimer_Tick()
        {
            if (spectogramBrightness)
            {
                ledManager.WriteLedColorMode(ColorTools.ColorFromHSV(rainbowLastH, 1, 1), getSpectogramBrightness());
            }
            else
            {
                ledManager.WriteLedColorMode(ColorTools.ColorFromHSV(rainbowLastH, 1, 1));
            }
            rainbowLastH++;
            if (rainbowLastH >= 360)
                rainbowLastH = 0;
        }
        // Update Ambilight led mode
        public void AmbilightTimer_Tick()
        {
            //if (ambilightTask == null || ambilightTask?.Status == TaskStatus.RanToCompletion) {
            //ambilightTask?.Dispose();
            byte[][] pixels = new byte[LedManager.LEDNUM][];
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

                                    byte[] pixel = bmp.GetPixel(i + ambilightRectangles[l].Location.X, j + ambilightRectangles[l].Location.Y);
                                    rSum += pixel[1];
                                    gSum += pixel[2];
                                    bSum += pixel[3];
                                    num++;
                                }
                            }

                            pixels[l] = new byte[] { ((byte)(rSum / num)), ((byte)(gSum / num)), ((byte)(bSum / num)) };


                        }
                    }




                }
                AmbilightTimerFinishedEvent.Set();
            }


            //Invoke(new Action(() => {
            if (spectogramBrightness)
            {
                ledManager.WriteLedDynamicMode(pixels, getSpectogramBrightness());
            }
            else
            {
                ledManager.WriteLedDynamicMode(pixels);
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
        public void ColorTimer_Tick()
        {
            if (spectogramBrightness)
            {
                ledManager.WriteLedColorMode(panel1.BackColor, getSpectogramBrightness());
            }
        }

        private void SpectogramButton_Click(object sender, EventArgs e)
        {
            spectogramBrightness = !spectogramBrightness;
            SpectogramButton.BackColor = spectogramBrightness ? Color.Gray : Color.LightGray;
            if (!spectogramBrightness && ledMode == LedMode.Color)
                ledManager.WriteLedColorMode(panel1.BackColor);
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
        public void PartyTimer_Tick()
        {
            if (spectogramBrightness)
            {
                int b = getSpectogramBrightness();
                ledManager.WriteLedColorMode(ColorTools.ColorFromHSV((partyLastH + b) % 360, 1, 1), (byte)b);
                partyLastH += 3;
                if (partyLastH >= 360)
                    partyLastH = partyLastH - 360;
            }
            else
            {
                ledManager.WriteLedColorMode(ColorTools.ColorFromHSV(partyLastH, 1, 1));
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

        private void ambilightInterval_Scroll(object sender, EventArgs e)
        {

        }

        private void ambilightInterval_ValueChanged(object sender, EventArgs e)
        {
            AmbilightTimerInterval = ambilightInterval.Value;
            AmbilightTimer.Change(AmbilightTimerDue, AmbilightTimerInterval);
            label3.Text = "Ambilight Interval (ms): " + AmbilightTimerInterval;
        }
    }

}

