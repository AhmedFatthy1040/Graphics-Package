using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GraphicsPackage;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    // Coordinate variables
    private int x1 = 100;
    private int y1 = 100;
    private int x2 = 300;
    private int y2 = 200;
    
    // Drawing color
    private Color lineColor = Colors.White;
    
    // WriteableBitmap for direct pixel manipulation
    private WriteableBitmap drawingBitmap;
    
    public MainWindow()
    {
        InitializeComponent();
        InitializeDrawingSurface();
    }
    
    /// <summary>
    /// Initializes the drawing surface with a WriteableBitmap
    /// </summary>
    private void InitializeDrawingSurface()
    {
        // Create a WriteableBitmap to draw on
        drawingBitmap = new WriteableBitmap(
            (int)DrawingCanvas.ActualWidth > 0 ? (int)DrawingCanvas.ActualWidth : 800,
            (int)DrawingCanvas.ActualHeight > 0 ? (int)DrawingCanvas.ActualHeight : 600,
            96, 96, PixelFormats.Bgr32, null);
            
        // Create an Image control to display the bitmap
        Image image = new Image();
        image.Source = drawingBitmap;
        DrawingCanvas.Children.Add(image);
        
        // Resize the bitmap when the canvas size changes
        DrawingCanvas.SizeChanged += (s, e) =>
        {
            if (e.NewSize.Width > 0 && e.NewSize.Height > 0)
            {
                drawingBitmap = new WriteableBitmap(
                    (int)e.NewSize.Width,
                    (int)e.NewSize.Height,
                    96, 96, PixelFormats.Bgr32, null);
                image.Source = drawingBitmap;
            }
        };
    }

    #region Event Handlers
    private void X1_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (int.TryParse(X1.Text, out int value))
        {
            x1 = value;
        }
    }

    private void Y1_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (int.TryParse(Y1.Text, out int value))
        {
            y1 = value;
        }
    }
    
    private void X2_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (int.TryParse(X2.Text, out int value))
        {
            x2 = value;
        }
    }

    private void Y2_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (int.TryParse(Y2.Text, out int value))
        {
            y2 = value;
        }
    }
    
    private void LineColor_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (LineColor.SelectedItem is ComboBoxItem selectedItem)
        {
            string colorTag = selectedItem.Tag.ToString();
            lineColor = (Color)ColorConverter.ConvertFromString(colorTag);
        }
    }

    private void DrawLineDDA_Click(object sender, RoutedEventArgs e)
    {
        DrawLineDDA(x1, y1, x2, y2, lineColor);
    }

    private void DrawLine_Bresenham_Click(object sender, RoutedEventArgs e)
    {
        DrawLineBresenham(x1, y1, x2, y2, lineColor);
    }
    
    private void ClearCanvas_Click(object sender, RoutedEventArgs e)
    {
        ClearDrawingSurface();
    }
    #endregion

    #region Drawing Algorithms
    /// <summary>
    /// Draws a line using the DDA (Digital Differential Analyzer) algorithm
    /// </summary>
    private void DrawLineDDA(int x1, int y1, int x2, int y2, Color color)
    {
        // Calculate the differences between the coordinates
        int dx = x2 - x1;
        int dy = y2 - y1;
        
        // Determine the number of steps to take
        int steps = Math.Abs(dx) > Math.Abs(dy) ? Math.Abs(dx) : Math.Abs(dy);
        
        // Calculate the increments for each step
        float xIncrement = dx / (float)steps;
        float yIncrement = dy / (float)steps;
        
        // Starting point
        float x = x1;
        float y = y1;
        
        // Use write pixels to avoid "freezing"
        drawingBitmap.Lock();
        
        // Draw each pixel in the line
        for (int i = 0; i <= steps; i++)
        {
            // Plot the pixel at the nearest integer coordinates
            SetPixel((int)Math.Round(x), (int)Math.Round(y), color);
            
            // Move to the next position
            x += xIncrement;
            y += yIncrement;
        }
        
        drawingBitmap.Unlock();
    }
    
    /// <summary>
    /// Draws a line using Bresenham's algorithm
    /// </summary>
    private void DrawLineBresenham(int x1, int y1, int x2, int y2, Color color)
    {
        // Calculate the differences between the coordinates
        int dx = Math.Abs(x2 - x1);
        int dy = Math.Abs(y2 - y1);
        
        // Determine the direction of drawing
        int sx = x1 < x2 ? 1 : -1;
        int sy = y1 < y2 ? 1 : -1;
        
        // Initialize the error term
        int err = dx - dy;
        
        drawingBitmap.Lock();
        
        // Continue until we reach the end point
        while (true)
        {
            // Plot the current pixel
            SetPixel(x1, y1, color);
            
            // Check if we've reached the end point
            if (x1 == x2 && y1 == y2)
                break;
            
            // Calculate the new error term
            int e2 = 2 * err;
            
            // Adjust the error term and coordinates based on the slope
            if (e2 > -dy)
            {
                err -= dy;
                x1 += sx;
            }
            
            if (e2 < dx)
            {
                err += dx;
                y1 += sy;
            }
        }
        
        drawingBitmap.Unlock();
    }
    
    /// <summary>
    /// Sets a pixel at the specified coordinates to the specified color
    /// </summary>
    private void SetPixel(int x, int y, Color color)
    {
        // Ensure the coordinates are within the bitmap bounds
        if (x < 0 || y < 0 || x >= drawingBitmap.PixelWidth || y >= drawingBitmap.PixelHeight)
            return;
            
        // Calculate the offset into the bitmap's pixel buffer
        int stride = drawingBitmap.BackBufferStride;
        int offset = y * stride + x * 4;
        
        // Update the pixel in the bitmap's buffer
        unsafe
        {
            byte* ptr = (byte*)drawingBitmap.BackBuffer.ToPointer() + offset;
            
            // Format is BGRA
            ptr[0] = color.B;
            ptr[1] = color.G;
            ptr[2] = color.R;
            ptr[3] = color.A;
        }
        
        // Mark the rectangle containing the pixel as dirty
        drawingBitmap.AddDirtyRect(new Int32Rect(x, y, 1, 1));
    }
    
    /// <summary>
    /// Clears the drawing surface
    /// </summary>
    private void ClearDrawingSurface()
    {
        drawingBitmap.Lock();
        
        // Fill the entire bitmap with black
        unsafe
        {
            byte* ptr = (byte*)drawingBitmap.BackBuffer.ToPointer();
            int totalSize = drawingBitmap.BackBufferStride * drawingBitmap.PixelHeight;
            
            for (int i = 0; i < totalSize; i++)
            {
                ptr[i] = 0;
            }
        }
        
        drawingBitmap.AddDirtyRect(new Int32Rect(0, 0, drawingBitmap.PixelWidth, drawingBitmap.PixelHeight));
        drawingBitmap.Unlock();
    }
    #endregion
}