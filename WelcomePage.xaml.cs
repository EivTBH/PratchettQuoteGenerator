using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;


namespace PratchettQuoteGenerator
{
   
    public sealed partial class WelcomePage : Page
    {

        private bool isMuted = false;
        public WelcomePage()
        {
            this.InitializeComponent();
            
        }
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

        private void GoToMainPageButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to MainPage
            PlayButtonClickSound();
            Frame.Navigate(typeof(MainPage));
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

        private void PlayButtonClickSound()
        {
            if (ButtonSoundPlayer.CurrentState == MediaElementState.Playing)
            {
                ButtonSoundPlayer.Stop();
            }
            ButtonSoundPlayer.Play();
        }
    }
}
