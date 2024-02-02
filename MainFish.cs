using System;
using System.Reflection.Metadata;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static System.Formats.Asn1.AsnWriter;

namespace rybki
{
    public class MainFish : Fish
    {
        public bool ToUp { get; set; }
        

        public Image Image { get { return image; } }
        public Point position { get; set; }

        public MainFish(string imagePath, int speed, bool toright, int score, Point initialPosition)
            : base(imagePath, speed, toright, score, initialPosition)
        { 
            image = new Image();
            image.Source = new BitmapImage(new Uri(imagePath, UriKind.Relative));
            image.Width = score*10;
            image.Height = score*10;
            Speed = speed;
            position = initialPosition;
            ToRight = true;
            Points = score;
            InitializePosition();
           // image.Background = Brushes.Transparent;
        }

        public new void ResetPosition()
        {
            position = new Point(0, 0);
            image.Width = Points * 10;
            image.Height = Points * 10;
            image.Visibility = Visibility.Hidden;
            InitializePosition();
        }


        public void MoveLeft()
        {
            image.Margin = new Thickness(image.Margin.Left - Speed, image.Margin.Top, 0, 0);
            if (ToRight)
            {
                image.RenderTransformOrigin = new Point(0.5, 0.5);
                image.RenderTransform = new ScaleTransform(-1, 1);
                
            }
            ToRight = false;

        }

        public void MoveRight()
        {
            image.Margin = new Thickness(image.Margin.Left + Speed, image.Margin.Top, 0, 0);
            if (!ToRight)
            {
                image.RenderTransformOrigin = new Point(0.5, 0.5);
                image.RenderTransform = new ScaleTransform(1, 1);

              
            }
            ToRight = true;


        }

        public void MoveDown()
        {
            image.Margin = new Thickness(image.Margin.Left, image.Margin.Top + Speed, 0, 0);
            if (!ToRight)
            {
                image.RenderTransformOrigin = new Point(0.5, 0.5);
                image.RenderTransform = new ScaleTransform(1, 1);


            }
            image.RenderTransform = new RotateTransform(90);
            ToRight = true;

        }

        public void MoveUp()
        {
            image.Margin = new Thickness(image.Margin.Left, image.Margin.Top - Speed, 0, 0);
            if (!ToRight)
            {
                image.RenderTransformOrigin = new Point(0.5, 0.5);
                image.RenderTransform = new ScaleTransform(1, 1);
            }
            image.RenderTransform = new RotateTransform(-90);
            ToRight = true;
        }

        public void SetNormal(int angle)
        {
            image.RenderTransformOrigin = new Point(0.5, 0.5);
            image.RenderTransform = new RotateTransform(angle);
        }


        public void Eat(int fishPoints)
        {
            Points += (int)(fishPoints/2);
            image.Width = Points * 10;
            image.Height = Points * 10;
        }
    }
}
