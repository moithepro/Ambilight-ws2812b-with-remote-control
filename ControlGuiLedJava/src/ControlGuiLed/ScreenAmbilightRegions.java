package ControlGuiLed;

import java.awt.Dimension;
import java.awt.Rectangle;
import java.awt.Toolkit;
import java.util.ArrayList;
import java.util.Collections;
import java.util.List;

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

    public int Height;
    public int Width;

    public Rectangle[] getRectangles()
    {
    	Dimension screenSize = Toolkit.getDefaultToolkit().getScreenSize();
    	Width = (int) screenSize.getWidth();
    	Height = (int) screenSize.getHeight();
        rectangleWidth = Width / 80;
        rectangleHeight = Height / 29;
        rectangleWidthBoundry = Width / 38;
        rectangleSpace = (Width - ((2 * rectangleWidthBoundry) + (rectangleNum * rectangleWidth))) / rectangleNum;


        verticalRectangleWidth = Width / 40;
        verticalRectangleHeight = Height / 58;
        verticalRectangleHeightBoundry = Height / 34;
        verticalRectangleSpace = (Height - (2 * verticalRectangleHeightBoundry) - (verticalRectangleNum * verticalRectangleHeight)) / verticalRectangleNum;
        List<Rectangle> rectangles1 = new ArrayList<Rectangle>();
        List<Rectangle> rectangles2 = new ArrayList<Rectangle>();
        List<Rectangle> rectangles3 = new ArrayList<Rectangle>();
        List<Rectangle> rectangles4 = new ArrayList<Rectangle>();
        // Create rectangles for each of the 4 edges of the screen
        for (int i = 0; i < rectangleNum; i++)
        {
            Rectangle rec = new Rectangle((int)Math.round(rectangleWidthBoundry + (i * (rectangleWidth + rectangleSpace + rectangleOffset))), 0, rectangleWidth, rectangleHeight);
            rectangles1.add(rec);

        }
        for (int i = 0; i < rectangleNum; i++)
        {
            Rectangle rec = new Rectangle((int)Math.round(rectangleWidthBoundry + (i * (rectangleWidth + rectangleSpace + rectangleOffset))), Height - rectangleHeight - 1, rectangleWidth, rectangleHeight);
            rectangles2.add(rec);

        }
        for (int i = 0; i < verticalRectangleNum; i++)
        {
            Rectangle rec = new Rectangle(0, (int)Math.round(verticalRectangleHeightBoundry + (i * (verticalRectangleHeight + verticalRectangleSpace + verticalRectangleOffset))), verticalRectangleWidth, verticalRectangleHeight);
            rectangles3.add(rec);

        }
        for (int i = 0; i < verticalRectangleNum; i++)
        {
            Rectangle rec = new Rectangle(Width - verticalRectangleWidth - 1, (int)Math.round(verticalRectangleHeightBoundry + (i * (verticalRectangleHeight + verticalRectangleSpace + verticalRectangleOffset))), verticalRectangleWidth, verticalRectangleHeight);
            rectangles4.add(rec);

        }
        Collections.reverse(rectangles4);
        Collections.reverse(rectangles1);
        List<Rectangle> rectangles = new ArrayList<Rectangle>(rectangles4);
        rectangles.addAll(rectangles1);
        rectangles.addAll(rectangles3);
        rectangles.addAll(rectangles2);
        return rectangles.toArray(new Rectangle[rectangles.size()]);
    }
}