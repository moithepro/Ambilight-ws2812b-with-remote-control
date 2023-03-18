using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ControlGuiLed
{
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
}
