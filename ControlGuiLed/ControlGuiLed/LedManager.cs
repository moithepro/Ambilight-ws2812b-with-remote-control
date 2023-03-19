using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGuiLed
{
    internal class LedManager
    {
        private SerialManager _serialManager;
        // Number of leds constant
        public const int LEDNUM = 178;
        public byte ledBrightness = 200;
        public static Color OFF_COLOR = Color.Black;
        public Color[] OFF_COLOR_ARR = new Color[LEDNUM];
        public LedManager(SerialManager serialManager)
        {
            _serialManager = serialManager;
            for (int i = 0; i < OFF_COLOR_ARR.Length; i++)
            {
                OFF_COLOR_ARR[i] = OFF_COLOR;
            }
        }
        public void WriteLedColorMode(Color color)
        {
            byte[] data = new byte[] { SerialManager.COLOR_RECV_CODE, ledBrightness, color.R, color.G, color.B };
            _serialManager.SerialWriteQueue.Add(data);
        }
        public void WriteLedColorMode(Color color, byte brightness)
        {
            byte[] data = new byte[] { SerialManager.COLOR_RECV_CODE, brightness, color.R, color.G, color.B };
            _serialManager.SerialWriteQueue.Add(data);
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
            _serialManager.SerialWriteQueue.Add(data);
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
            _serialManager.SerialWriteQueue.Add(data);
        }
    }
}
