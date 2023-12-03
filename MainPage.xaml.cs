///////////////////////////////////////////////////////////////////////////////
// FileName: MainPage.xaml.cs
// File Type: Visual C# Source file
// Author: Dan Padgett
// Created On: 27/10/2023
// Last Modified On: 03/12/2023
// Copy Rights: Dan Padgett
// Description: This file contains the code behind for the MainPage.xaml file. It contains the event
// handlers for the buttons and the code that loads the quotes from the file.
/////////////////////////////////////////////////////////////////////////////// 


using System;
using Windows.Foundation;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;

namespace PratchettQuoteGenerator
{
    public sealed partial class MainPage
    {
        private readonly MainPageViewModel _viewModel;

        public MainPage()
        {
            InitializeComponent(); // This is the line that loads the XAML file.
            _viewModel = new MainPageViewModel(); // This is the line that creates the ViewModel.
            DataContext = _viewModel; // This is the line that sets the DataContext of the page to the ViewModel.
            _viewModel.LoadQuotesFromFile(); // This is the line that loads the quotes from the file.
            ApplicationView.PreferredLaunchViewSize = new Size(1920, 1080); // This is the line that sets the size of the window.
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize; // This is the line that sets the window to the size specified above.
        }

        /// <summary>
        /// Button click event handler for the random quote button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RandomQuoteButton_Click(object sender, RoutedEventArgs e)
        {
            
            _viewModel.GetRandomQuote();
            PlayButtonClickSound();
        }
        ///// <summary>
        ///// Event handler for when the page is loaded.
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void Page_Loaded(object sender, RoutedEventArgs e)
        //{
        //    CustomPopup.IsOpen = true;
        //}

        ///// <summary>
        ///// Event handler for when the close popup button is clicked.
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void ClosePopup_Click(object sender, RoutedEventArgs e)
        //{
        //    CustomPopup.IsOpen = false;
        //}

        /// <summary>
        /// Event handler for when the pointer enters the button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            var rotateAnimation = new DoubleAnimation()
            {
                To = -360,
                Duration = new Duration(TimeSpan.FromSeconds(1)),
                EnableDependentAnimation = true,
            };
            Storyboard.SetTarget(rotateAnimation, ImageRotateTransform);
            Storyboard.SetTargetProperty(rotateAnimation, "Angle");

            var storyboard = new Storyboard();
            storyboard.Children.Add(rotateAnimation);
            storyboard.Begin();
        }

        /// <summary>
        /// Event handler for when the pointer exits the button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            ImageRotateTransform.Angle = 0;
        }

        //private void PopupContent_Loaded(object sender, RoutedEventArgs e)
        //{
        //    CenterPopup();
        //}

        //private void PopupContent_SizeChanged(object sender, SizeChangedEventArgs e)
        //{
        //    CenterPopup();
        //}

        //private void CenterPopup()
        //{
        //    var screenWidth = Window.Current.Bounds.Width;
        //    var screenHeight = Window.Current.Bounds.Height;

        //    var popupWidth = CustomPopup.Child.RenderSize.Width;
        //    var popupHeight = CustomPopup.Child.RenderSize.Height;

        //    CustomPopup.HorizontalOffset = (screenWidth - popupWidth) / 2;
        //    CustomPopup.VerticalOffset = (screenHeight - popupHeight) / 2;
        //}

        private void BackgroundMusic_MediaEnded(object sender, RoutedEventArgs e)
        {
            BackgroundMusic.Position = TimeSpan.Zero;
            BackgroundMusic.Play();
        }

        private void MuteButton_Click(object sender, RoutedEventArgs e)
        {
            // Assume BackgroundMusic is your MediaElement
            if (BackgroundMusic.IsMuted)
            {
                // Unmute the sound
                BackgroundMusic.IsMuted = false;
                // Change the button image to the unmute icon
                MuteImage.Source = new BitmapImage(new Uri(this.BaseUri, "Assets/mute3.png"));
            }
            else
            {
                // Mute the sound
                BackgroundMusic.IsMuted = true;
                // Change the button image to the mute icon
                MuteImage.Source = new BitmapImage(new Uri(this.BaseUri, "Assets/unmute3.png"));
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Check if the sound is already playing, stop it if it is.
            if (ButtonSoundPlayer.CurrentState == MediaElementState.Playing)
            {
                ButtonSoundPlayer.Stop();
            }

            // Play the button click sound.
            PlayButtonClickSound();
        }

        public void PlayButtonClickSound()
        {
            if (ButtonSoundPlayer.CurrentState == MediaElementState.Playing)
            {
                ButtonSoundPlayer.Stop();
            }
            ButtonSoundPlayer.Play();
        }

    }
}