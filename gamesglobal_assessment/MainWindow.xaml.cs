using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace gamesglobal_assessment
{
    public partial class MainWindow : Window
    {
        private List<Path> wheelSegments;
        private Dictionary<Path, double> segmentPrizes;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void SpinButton_Click(object sender, RoutedEventArgs e)
        {
            InitializeWheelSegments();

            ((Button)sender).IsEnabled = false;

            double winAmount = Convert.ToDouble(WinAmountInput.Text);

            // Randomize prizes
            RandomizePrizes();

            RotateWheel(winAmount);
        }

        private void InitializeWheelSegments()
        {
            wheelSegments = new List<Path>();
            segmentPrizes = new Dictionary<Path, double>();

            foreach (var child in WheelCanvas.Children)
            {
                if (child is Path path)
                {
                    wheelSegments.Add(path);
                }
            }
        }

        private void RotateWheel(double winAmount)
        {
            DoubleAnimation rotateAnimation = new DoubleAnimation
            {
                To = 3600,
                Duration = TimeSpan.FromSeconds(10),
                FillBehavior = FillBehavior.HoldEnd
            };

            rotateAnimation.Completed += (sender, e) =>
            {
                SpinButton.IsEnabled = true;

                bool isWinner = DetermineWin();

                if (isWinner)
                {
                    double winValue = CalculateWin();
                    MessageBox.Show($"You won ${winValue}");
                }
                else
                {
                    MessageBox.Show("Sorry, you didn't win this time.");
                }
            };

            WheelImage.RenderTransform = new RotateTransform();
            WheelImage.RenderTransformOrigin = new Point(0.5, 0.5);
            WheelImage.RenderTransform.BeginAnimation(RotateTransform.AngleProperty, rotateAnimation);
        }

        private void RandomizePrizes()
        {
            List<double> potentialPrizes = new List<double> { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };

            Random random = new Random();
            potentialPrizes = potentialPrizes.OrderBy(x => random.Next()).ToList();

            for (int i = 0; i < wheelSegments.Count; i++)
            {
                segmentPrizes[wheelSegments[i]] = potentialPrizes[i];
            }
        }

        private bool DetermineWin()
        {
            Random random = new Random();
            return random.Next(2) == 0;
        }

        private double CalculateWin()
        {
            double winAmount = 0;

            foreach (var segment in wheelSegments)
            {
                winAmount += segmentPrizes[segment];
            }

            return winAmount;
        }
    }
}
