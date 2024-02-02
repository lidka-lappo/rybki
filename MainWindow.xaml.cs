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
using System;
using System.Collections.Generic;
using System.Windows.Threading;
using static System.Formats.Asn1.AsnWriter;
using System.Reflection.Emit;
using static System.Net.Mime.MediaTypeNames;
using static rybki.MainWindow;
using System.DirectoryServices;
using System.IO;

namespace rybki
{
    public partial class MainWindow : Window
    {
        private List<Fish> fishes_list;

        private MainFish mainFish;
        private bool isStarted = false;
        private int score = 0;
        private int highScore = 0;
        private int size_of_main_fish = 2;

        public MainWindow()
        {
            
            InitializeComponent();
            InitializeMainFish();
            InitializeFishes();
            MainScreen();
            level_comboBox.SelectedIndex = 0;
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval = TimeSpan.FromMilliseconds(50);
            timer.Start();
            //highScore = ReadHighScore();
            //highScoreLabel.Content = "High Score: " + highScore;
            KeyDown += Window_KeyDown;
            KeyUp += Window_KeyUp;
           
        }
        private void MainScreen()
        {

            aquariumCanvas.Visibility = Visibility.Visible;
            Main_menu_canvas.Visibility = Visibility.Visible;
            Dead_canvas.Visibility = Visibility.Collapsed;
            mainFish.Image.Visibility = Visibility.Hidden;
            labels_canvas.Visibility = Visibility.Hidden;
            highScore = ReadHighScore();
            highScoreLabel.Content = " High Score: " + highScore; 


        }


        private void StartGame()
        {
            mainFish.IsDead = false;
            mainFish.Points = size_of_main_fish;   
            mainFish.ResetPosition();
            Main_menu_canvas.Visibility = Visibility.Collapsed;
            aquariumCanvas.Visibility = Visibility.Visible;
            mainFish.Image.Visibility = Visibility.Visible;
            labels_canvas.Visibility = Visibility.Visible;
            
            mainFish.SetNormal(0);
            isStarted = true;
            score = 0;
            scoreLabel.Content = "Score: 0" ;
            level_label.Content = "Level: " + level_comboBox.Text;
        }

        private void GameOver()
        {
            mainFish.IsDead = true;
            mainFish.SetNormal(180);
            Dead_canvas.Visibility = Visibility.Visible;
            labels_canvas.Visibility = Visibility.Collapsed;
            highScoreLabel.Visibility = Visibility.Visible;
            new_high_score_label.Visibility = Visibility.Hidden;
            //Main_menu_canvas.Visibility = Visibility.Collapsed;
            aquariumCanvas.Visibility = Visibility.Visible;
            isStarted = false;
            your_score_label.Content = "Your score: " + score;
            if (highScore<score)
            {
                new_high_score_label.Visibility = Visibility.Visible;
                SaveHighScore(score);
            }
            score = 0;
            scoreLabel.Content = "Score: 0";
           

        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            MoveFishes();
        }



        private void InitializeFishes()
        {
            fishes_list = new List<Fish>();
            if (level_comboBox.Text == "easy")
            {
                GenerateEasyFishList();
            }
            else if (level_comboBox.Text == "medium")
            {
                GenerateMediumFishList();
            }
            else if (level_comboBox.Text == "hard")
            {
                GenerateHardFishList();
            }
            else
            {
                GenerateRandomFishList();
            }

            for (int i = 0; i < fishes_list.Count; i++)
            {
                aquariumCanvas.Children.Add(fishes_list[i].image);
                fishes_list[i].InitializePosition();
            }
        }

        private void InitializeMainFish()
        {

            mainFish = new MainFish("/rybki;component/Resources/main_fish.png", 10, true, size_of_main_fish, new Point(50, 50));
            aquariumCanvas.Children.Add(mainFish.Image);
            mainFish.InitializePosition();
        }


        private void MoveFishes()
        {
            foreach (Fish fish in fishes_list)
            {
                fish.Move();
                if (isStarted)
                {
                    if (!fish.IsDead)
                    {

                        if (CheckCollision(mainFish, fish))
                        {
                            if (mainFish.Points >= fish.Points && fish.Poisonous != true)
                            {
                                HandleCollision(mainFish, fish);
                                mainFish.Eat(fish.Points);
                            }
                            else
                            {
                                GameOver();
                            }

                        }


                    }
                }
                if (fish.image.Margin.Left < (0 - fish.image.ActualWidth*2) || fish.image.Margin.Left > (aquariumCanvas.Width + fish.image.ActualWidth +10))
                {
                    fish.image.Visibility = Visibility.Visible;
                    fish.ResetPosition();
                }
            }
        }
        private bool CheckCollision(MainFish mainFish, Fish otherFish)
        {
            Rect mainFishRect = new Rect(mainFish.Image.Margin.Left, mainFish.Image.Margin.Top, mainFish.Image.ActualWidth, mainFish.Image.ActualHeight);
            Rect otherFishRect = new Rect(otherFish.image.Margin.Left, otherFish.image.Margin.Top, otherFish.image.ActualWidth, otherFish.image.ActualHeight);

            return mainFishRect.IntersectsWith(otherFishRect);

        }

        private void HandleCollision(MainFish mainFish, Fish otherFish)
        {
            otherFish.IsDead = true;
            otherFish.image.Visibility = Visibility.Hidden;
            //aquariumCanvas.Children.Remove(otherFish.image);
            score += otherFish.Points;
            scoreLabel.Content = "Score: " + score;
        }


        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            StartGame();
        }
        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            
            if (isStarted)
            {
                switch (e.Key)
                {
                    case Key.Left:
                      //  mainFish.SetNormal(0);
                        break;

                    case Key.Right:
                        mainFish.SetNormal(0);
                        break;

                    case Key.Up:
                        mainFish.SetNormal(0);
                        break;

                    case Key.Down:
                        mainFish.SetNormal(0);
                        break;
                }
            }
            
        }

            private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (isStarted)
            {
                switch (e.Key)
                {
                    case Key.Left:
                        mainFish.MoveLeft();
                        break;

                    case Key.Right:
                        mainFish.MoveRight();
                        break;

                    case Key.Up:
                        mainFish.MoveUp();
                        break;

                    case Key.Down:
                        mainFish.MoveDown();
                        break;
                }
            }
        }

        private int ReadHighScore()
        { 
             try
            {
                string filePath = "highscore.txt";

               
                if (File.Exists(filePath))
                {
                    
                    string highScoreString = File.ReadAllText(filePath);

                    
                    if (int.TryParse(highScoreString, out int highScore))
                    {
                        return highScore;
                    }
                }
             }
            catch (Exception ex)
            {
                
                Console.WriteLine("Error reading high score: " + ex.Message);
            }

            
            return 0;
         }

        private void SaveHighScore(int score)
        {
            try
            {
                string filePath = "highscore.txt";

               
                if (!File.Exists(filePath))
                {
                   
                    File.Create(filePath).Close();
                }

               
                int currentHighScore = ReadHighScore();

                
                if (score > currentHighScore)
                {
                    
                    File.WriteAllText(filePath, score.ToString());
                }
            }
            catch (Exception ex)
            {
                
                Console.WriteLine("Error saving high score: " + ex.Message);
            }
        }

        private void back_to_meanu_button_Click(object sender, RoutedEventArgs e)
        {
            Dead_canvas.Visibility = Visibility.Collapsed;
            Main_menu_canvas.Visibility = Visibility.Visible;
            aquariumCanvas.Visibility = Visibility.Visible;
            mainFish.Image.Visibility = Visibility.Collapsed;



        }

        private void Set_Level_Click(object sender, RoutedEventArgs e)
        {
            foreach (Fish fish in fishes_list)
            {
                aquariumCanvas.Children.Remove(fish.image);
            }
            fishes_list.Clear();
            InitializeFishes();
            mainFish.ResetPosition();
        }
        private void GenerateEasyFishList()
        {
            size_of_main_fish = 5;
            Random random = new Random();
            fishes_list.Add(new Fish("/rybki;component/Resources/gold_fish.png", 4, true, 1, new Point(200, 300)));
            for (int i = 0; i < 20; i++)
            {
                int speed = random.Next(1, 6);
                bool toRight = random.Next(2) == 0;
                int points = random.Next(1, 5);
                Point initialPosition = new Point(random.Next((int)aquariumCanvas.Width), random.Next((int)aquariumCanvas.Height));
                fishes_list.Add(new Fish("/rybki;component/Resources/gold_fish.png", speed, toRight, points, initialPosition));
            }
        }

        private void GenerateMediumFishList()
        {
            size_of_main_fish = 2;
            fishes_list.Add(new Fish("/rybki;component/Resources/gold_fish.png", 5, true, 5, new Point(0, 100)));
            fishes_list.Add(new Fish("/rybki;component/Resources/gold_fish.png", 8, true, 3, new Point(50, 150)));
            fishes_list.Add(new Fish("/rybki;component/Resources/gold_fish.png", 3, true, 4, new Point(100, 200)));
            fishes_list.Add(new Fish("/rybki;component/Resources/gold_fish.png", 6, true, 6, new Point(150, 250)));
            fishes_list.Add(new Fish("/rybki;component/Resources/gold_fish.png", 4, true, 1, new Point(200, 300)));


            fishes_list.Add(new Fish("/rybki;component/Resources/salmon.png", 15, true, 5, new Point(100, 800)));
            fishes_list.Add(new Fish("/rybki;component/Resources/salmon.png", 15, false, 5, new Point(200, 700)));
            fishes_list.Add(new Fish("/rybki;component/Resources/salmon.png", 15, true, 6, new Point(300, 600)));
            fishes_list.Add(new Fish("/rybki;component/Resources/salmon.png", 15, false, 6, new Point(400, 500)));
            fishes_list.Add(new Fish("/rybki;component/Resources/salmon.png", 15, true, 7, new Point(500, 400)));
            fishes_list.Add(new Fish("/rybki;component/Resources/salmon.png", 15, false, 7, new Point(600, 300)));



            fishes_list.Add(new Fish("/rybki;component/Resources/shark.png", 10, false, 1, new Point(500, 700)));

        }

        private void GenerateHardFishList()
        {
            size_of_main_fish = 2;
            Random random = new Random();

            fishes_list.Add(new Fish("/rybki;component/Resources/gold_fish.png", 5, true, 5, new Point(0, 100)));
            fishes_list.Add(new Fish("/rybki;component/Resources/gold_fish.png", 8, true, 3, new Point(50, 150)));
            fishes_list.Add(new Fish("/rybki;component/Resources/gold_fish.png", 3, true, 4, new Point(100, 200)));
            fishes_list.Add(new Fish("/rybki;component/Resources/gold_fish.png", 6, true, 6, new Point(150, 250)));
            fishes_list.Add(new Fish("/rybki;component/Resources/gold_fish.png", 4, true, 1, new Point(200, 300)));
            for (int i = 0; i < 2; i++)
            {
                int speed = random.Next(1, 6);
                bool toRight = random.Next(2) == 0;
                int points = random.Next(6, 10);
                Point initialPosition = new Point(random.Next((int)aquariumCanvas.Width), random.Next((int)aquariumCanvas.Height));

                fishes_list.Add(new Fish("/rybki;component/Resources/puff_fish.png", speed, toRight, points, initialPosition, true));
            }

            for (int i = 0; i < 4; i++)
            {
                int speed = random.Next(1, 4);
                bool toRight = random.Next(2) == 0;
                int points = random.Next(5, 10);
                Point initialPosition = new Point(random.Next((int)aquariumCanvas.Width), random.Next((int)aquariumCanvas.Height));

                fishes_list.Add(new Fish("/rybki;component/Resources/salmon.png", speed, toRight, points, initialPosition));
            }
            for (int i = 0; i < 5; i++)
            {
                int speed = 10;
                bool toRight = random.Next(2) == 0;
                int points = 20;
                Point initialPosition = new Point(random.Next((int)aquariumCanvas.Width), random.Next((int)aquariumCanvas.Height));

                fishes_list.Add(new Fish("/rybki;component/Resources/shark.png", speed, toRight, points, initialPosition));
            }

        }

        private void GenerateRandomFishList()
        {
            size_of_main_fish = 2;
            Random random = new Random();
            for (int i = 0; i < 10; i++)
            {
                int speed = random.Next(1, 6);
                bool toRight = random.Next(2) == 0;
                int points = random.Next(1, 4);
                Point initialPosition = new Point(random.Next((int)aquariumCanvas.Width), random.Next((int)aquariumCanvas.Height));

                fishes_list.Add(new Fish("/rybki;component/Resources/gold_fish.png", speed, toRight, points, initialPosition));
            }

            for (int i = 0; i < 2; i++)
            {
                int speed = random.Next(1, 6);
                bool toRight = random.Next(2) == 0;
                int points = random.Next(6, 10);
                Point initialPosition = new Point(random.Next((int)aquariumCanvas.Width), random.Next((int)aquariumCanvas.Height));

                fishes_list.Add(new Fish("/rybki;component/Resources/puff_fish.png", speed, toRight, points, initialPosition, true));
            }

            for (int i = 0; i < 4; i++)
            {
                int speed = random.Next(1, 4);
                bool toRight = random.Next(2) == 0;
                int points = random.Next(5, 10);
                Point initialPosition = new Point(random.Next((int)aquariumCanvas.Width), random.Next((int)aquariumCanvas.Height));

                fishes_list.Add(new Fish("/rybki;component/Resources/salmon.png", speed, toRight, points, initialPosition));
            }
            for (int i = 0; i < 2; i++)
            {
                int speed = 10;
                bool toRight = random.Next(2) == 0;
                int points = 20;
                Point initialPosition = new Point(random.Next((int)aquariumCanvas.Width), random.Next((int)aquariumCanvas.Height));

                fishes_list.Add(new Fish("/rybki;component/Resources/shark.png", speed, toRight, points, initialPosition));
            }

        }

        private void exit_button_Click(object sender, RoutedEventArgs e)
        {
            GameOver();
        }
    }
}
