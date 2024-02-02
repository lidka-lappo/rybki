using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace rybki
{
    public class Fish
    {
        public Image image { get;  set; }
        public int Width { get; set; }
        public int Height { get;  set; }
        public int Speed { get; set; }
        public Point InitialPosition { get;  set; }

        public bool Poisonous { get; set; }

        public bool ToRight { get;  set; }
        public bool IsDead { get;  set; }
        public int Points { get; set; }
        public Fish(string imagePath, int speed, bool toright, int score, Point initialPosition, bool pois = false)
        {
            image = new Image();
            Poisonous = pois;
            image.Source = new BitmapImage(new Uri(imagePath, UriKind.Relative));
            image.Width = score * 10;
            image.Height = score * 10;
            Points = score;
            Speed = speed;
            InitialPosition = initialPosition;
            IsDead = false;
            ToRight = toright;

            ResetPosition();
        }

        public void Move()
        {
            if (ToRight)
            {
                image.Margin = new Thickness(image.Margin.Left + Speed, image.Margin.Top, 0, 0);
            }
            else
            {
                image.Margin = new Thickness(image.Margin.Left - Speed, image.Margin.Top, 0, 0);
            }
        }

        public void InitializePosition()
        {
            image.Margin = new Thickness(InitialPosition.X, InitialPosition.Y, 0, 0);
            if (!ToRight)
            {
                image.RenderTransformOrigin = new Point(0.5, 0.5);
                image.RenderTransform = new ScaleTransform(-1, 1);
            }
        }

        public void Dead()
        {
            IsDead = true;
            image.Visibility = Visibility.Hidden;
        }

        public void ResetPosition()
        {
            image.RenderTransformOrigin = new Point(0.5, 0.5);
            ScaleTransform flipTrans = new ScaleTransform();
            IsDead = false;

            if (ToRight)
            {
                flipTrans.ScaleX = -1;
                ToRight = false;            
            }
            else
            {
                flipTrans.ScaleX = 1;
                ToRight = true;
               
            }

            image.RenderTransform = flipTrans;
        }

         public bool CollidesWith(Fish otherFish)
    {
        Rect fishBounds = new Rect(image.Margin.Left, image.Margin.Top, image.Width, image.Height);
        Rect otherFishBounds = new Rect(otherFish.image.Margin.Left, otherFish.image.Margin.Top, otherFish.image.Width, otherFish.image.Height);

        return fishBounds.IntersectsWith(otherFishBounds);
    }
    }
}
