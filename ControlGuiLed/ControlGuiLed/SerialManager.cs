using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ControlGuiLed
{
    
    public class SerialManager
    {
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
        public const int BAUD_RATE = 500000;
        // Serial port name
        public const string PORT_NAME = "COM3";
        private SerialPort _serialPort;
        public BlockingCollection<byte[]> SerialWriteQueue = new BlockingCollection<byte[]>();

        private Thread readThread;
        private Thread writeThread;
        private bool controlNumFunc = false;

        public void SerialStart()
        {
            readThread = new Thread(Read);
            readThread.Start();
            writeThread = new Thread(Write);
            writeThread.Start();
        }
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
        private void ConnectSerialPort()
        {
            while (true)
            {
                try
                {
                    _serialPort = new SerialPort(PORT_NAME, BAUD_RATE);
                    _serialPort.Handshake = Handshake.None;

                    _serialPort.Open();
                    Program.form1.SetConnected(true);
                    break;
                }
                catch (IOException)
                {
                    Program.form1.SetConnected(false);
                }
                Thread.Sleep(1000);
            }
        }
        public void Write()
        {
            while (true)
            {
                byte[] data = SerialWriteQueue.Take();
                if (_serialPort.IsOpen)
                    _serialPort.Write(data, 0, data.Length);

            }
        }

    }
}
