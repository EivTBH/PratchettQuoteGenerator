using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Newtonsoft.Json;

namespace PratchettQuoteGenerator
{
    /// <summary>
    /// Main page view model.
    /// </summary>
    public class MainPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<Quote> _quotes;
        private ObservableCollection<string> _characterNames;
        private BitmapImage _characterImageSource;
        private string _selectedCharacter;
        private string _selectedQuoteText;

        private ObservableCollection<FontFamily> _availableFonts;
        private FontFamily _selectedFont;

        public bool IsQuoteTextEmpty => string.IsNullOrWhiteSpace(SelectedQuoteText);
        public ICommand CopyQuoteCommand { get; private set; }
        private string _buttonText = "CLIP QUOTE"; 

        public ObservableCollection<FontFamily> AvailableFonts
        {
            // Update the available fonts
            get => _availableFonts;
            set
            {
                _availableFonts = value;
                OnPropertyChanged(nameof(AvailableFonts));
            }
        }

        public FontFamily SelectedFont
        {
            // Update the selected font
            get => _selectedFont;
            set
            {
                _selectedFont = value;
                OnPropertyChanged(nameof(SelectedFont));
            }
        }

        /// <summary>
        /// Controls the text of the button to show user feedback
        /// </summary>
        public string ButtonText
        {
            // Update the button text
            get => _buttonText;
            set
            {
                _buttonText = value;
                OnPropertyChanged(nameof(ButtonText));
            }
        }

        /// <summary>
        /// Copies the quote to the clipboard
        /// </summary>
        private async void CopyQuoteToClipboard()
        {
            var dataPackage = new DataPackage();
            dataPackage.RequestedOperation = DataPackageOperation.Copy;
            dataPackage.SetText(SelectedQuoteText);
            Clipboard.SetContent(dataPackage);
            ButtonText = "Copied!";
            await Task.Delay(1000); // Wait for 2 seconds
            ButtonText = "Clip Quote";
        }

        /// <summary>
        /// Collection of quotes. It is used to display the quotes for the selected character.
        /// </summary>
        public ObservableCollection<Quote> Quotes
        {
            get => _quotes;
            set
            {
                // Update the quotes collection
                if (_quotes != value)
                {
                    _quotes = value;
                    OnPropertyChanged(nameof(Quotes));
                }
            }
        }

        /// <summary>
        /// Collection of character names. It is used to populate the character dropdown.
        /// </summary>
        public ObservableCollection<string> CharacterNames
        {
            get => _characterNames;
            set
            {
                if (_characterNames != value)
                {
                    // Update the character names collection
                    _characterNames = value;
                    OnPropertyChanged(nameof(CharacterNames));
                }
            }
        }

        /// <summary>
        /// Character image source. It is used to display the character image.
        /// </summary>
        public BitmapImage CharacterImageSource
        {
            get => _characterImageSource;
            set
            {
                // Update the character image source
                if (_characterImageSource != value)
                {
                    _characterImageSource = value;
                    OnPropertyChanged(nameof(CharacterImageSource));
                }
            }
        }

        /// <summary>
        /// Selected character name. It is used to filter the quotes for the selected character.
        /// </summary>
        public string SelectedCharacter
        {
            get => _selectedCharacter;
            set
            {
                // Update the selected character
                if (_selectedCharacter == value) return;
                _selectedCharacter = value;
                OnPropertyChanged(nameof(SelectedCharacter));
                UpdateCharacterQuotes();
            }
        }

        /// <summary>
        /// Selected quote text. It is used to display the quotes for the selected character.
        /// </summary>
        public string SelectedQuoteText
        {
            // Update the selected quote text
            get => _selectedQuoteText;
            set
            {
                if (_selectedQuoteText == value) return;
                _selectedQuoteText = value;
                OnPropertyChanged(nameof(SelectedQuoteText));
                (CopyQuoteCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Main page view model constructor.
        /// </summary>
        public MainPageViewModel()
        {
            // Initialize the quotes and character names collections
            _quotes = new ObservableCollection<Quote>();
            //Initialize the character names collection
            _characterNames = new ObservableCollection<string>();
            // Initialize the copy quote command
            CopyQuoteCommand = new RelayCommand(CopyQuoteToClipboard, () => !IsQuoteTextEmpty);
            //Add fonts to the dropdown
            AvailableFonts = new ObservableCollection<FontFamily>
            {
                new FontFamily("Select Font..."),
                new FontFamily("Times New Roman"),
                new FontFamily("Arial"),
                new FontFamily("Ink Free"),
                new FontFamily("MV Boli"),
                new FontFamily("Gabriola"),
                
            };

            SelectedFont = AvailableFonts[3]; // Sets default font
        }

        /// <summary>
        /// Loads quotes from a JSON file. The file is included in the project as a content file.
        /// </summary>
        public async void LoadQuotesFromFile()
        {
            // Load quotes from a JSON file
            try
            {
                var file =
                    await StorageFile.GetFileFromApplicationUriAsync(
                        new Uri("ms-appx:///Assets/pratchett_quotes_withchars.json"));
                var jsonContent = await FileIO.ReadTextAsync(file);
                var quotesFromJson = JsonConvert.DeserializeObject<List<Quote>>(jsonContent);
                if (quotesFromJson == null) return;
                // Update the quotes and character names collections
                Quotes = new ObservableCollection<Quote>(quotesFromJson);
                CharacterNames = new ObservableCollection<string>(Quotes.SelectMany(q => q.Characters).Distinct());
            }
            // If the file cannot be loaded, display a message. Also, display the exception message in the SelectedQuoteText property. THIS IS FOR TESTING PURPOSES ONLY.
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to load quotes: " + ex);
                SelectedQuoteText = "Failed to load quotes. Error: " + ex.Message;
            }
        }

        /// <summary>
        /// Dictionary of character-image mappings.
        /// </summary>
        private readonly Dictionary<string, string> _characterImages = new Dictionary<string, string>
        {
            //Dictionary with char name and image location
            { "Death", "ms-appx:///Assets/death.jpg" },
            { "Wee Mad Arthur", "ms-appx:///Assets/weemadarthur.jpg" },
            { "Samuel Vimes", "ms-appx:///Assets/samuelvimes.jpg" },
            { "Albert", "ms-appx:///Assets/albert.png" },
            { "Nanny Ogg", "ms-appx:///Assets/nannyogg.jpg" },
            { "Hogfather", "ms-appx:///Assets/hogfather.jpg" },
            { "Granny Weatherwax", "ms-appx:///Assets/grannyweatherwax.jpg" },
            { "Twoflower", "ms-appx:///Assets/twoflower.jpg" },
            { "Rincewind", "ms-appx:///Assets/rincewind.jpg" },
            { "Detritus", "ms-appx:///Assets/detritus.jpg" },
            { "Dorfl", "ms-appx:///Assets/dorfl.jpg" },
            { "The Librarian", "ms-appx:///Assets/thelibrarian.jpg" },
            { "Lu-Tze", "ms-appx:///Assets/lutze.jpg" },
            { "Lord Vetinari", "ms-appx:///Assets/lordvetinari.jpg" },
            { "Cheery", "ms-appx:///Assets/cheery.jpg" },
            { "Dwarfs", "ms-appx:///Assets/dwarfs.jpg" },
            { "Igor", "ms-appx:///Assets/igor.jpg" },
            { "Pin", "ms-appx:///Assets/pin.jpg" },
            { "Angua", "ms-appx:///Assets/angua.jpg" },
            { "Sam Vimes", "ms-appx:///Assets/samvimes.jpg" },
            { "The Duck Man", "ms-appx:///Assets/theduckman.jpg" },
            { "Nobby Nobbs", "ms-appx:///Assets/nobbynobbs.jpg" },
            { "Cuddy", "ms-appx:///Assets/cuddy.jpg" },
            { "The Luggage", "ms-appx:///Assets/theluggage.jpg" },
            { "Tiffany Aching", "ms-appx:///Assets/tiffanyaching.jpg" },
            { "Magrat Garlick", "ms-appx:///Assets/magratgarlick.jpg" },
            { "Fred Colon", "ms-appx:///Assets/fredcolon.jpg" },
            { "Adora Belle Dearheart", "ms-appx:///Assets/adorabelledearheart.jpg" },
            { "Gaspode", "ms-appx:///Assets/gaspode.jpg" },
            { "Greebo", "ms-appx:///Assets/greebo.jpg" },
            { "The Auditors", "ms-appx:///Assets/theauditors.png" },
            { "Ponder Stibbons", "ms-appx:///Assets/ponderstibbons.jpg" },
            { "Stibbons", "ms-appx:///Assets/stibbons.jpg" },
            { "Gytha Ogg", "ms-appx:///Assets/gythaogg.jpg" },
            { "Mr. Tulip", "ms-appx:///Assets/mrtulip.jpg" },
            { "Mustrum Ridcully", "ms-appx:///Assets/mustrumridcully.jpg" },
            { "Hex", "ms-appx:///Assets/hex.jpg" },
            { "Foul Ole Ron", "ms-appx:///Assets/fouloron.jpg" },
            { "Binky", "ms-appx:///Assets/binky.jpg" },
            { "The Bursar", "ms-appx:///Assets/thebursar.jpg" },
            { "Foul Ol' Ron", "ms-appx:///Assets/fouloron2.jpg" },
            { "Agnes Nitt", "ms-appx:///Assets/agnesnitt.jpg" },
            { "Leonard of Quirm", "ms-appx:///Assets/leonardofquirm.jpg" },
            { "Billy Slick", "ms-appx:///Assets/billyslick.jpg" },
            { "Dibbler", "ms-appx:///Assets/dibbler.jpg" },
            { "Reg Shoe", "ms-appx:///Assets/regshoe.jpg" },
            { "Otto Chriek", "ms-appx:///Assets/ottochriek.jpg" },
            { "Captain Carrot", "ms-appx:///Assets/captaincarrot.jpg" },
            { "Cheery Littlebottom", "ms-appx:///Assets/cheery.jpg" }
        };

        /// <summary>
        /// Updates the SelectedQuoteText with quotes from the selected character.
        /// </summary>
        public void UpdateCharacterQuotes()
        {
            // Update the SelectedQuoteText with quotes from the selected character
            if (!string.IsNullOrEmpty(SelectedCharacter))
            {
                var characterQuotes = Quotes
                    .Where(q => q.Characters != null && q.Characters.Contains(SelectedCharacter))
                    .Select(q =>
                    {
                        var formattedText = q.Text.TrimEnd(',', ' '); // Remove trailing commas and spaces
                        var bookText = q.Book?.TrimEnd(','); // Remove trailing comma from the book title
                        return
                            $"{formattedText}\n\n{q.Author}\n\n{bookText} \n\n ------ \n\n"; // Removed '-' before author's name
                    });

                SelectedQuoteText = characterQuotes.Any()
                    ? string.Join(Environment.NewLine + Environment.NewLine, characterQuotes)
                    : "No quotes available for this character.";

                // Update the character image if available
                CharacterImageSource = new BitmapImage(new Uri(_characterImages[SelectedCharacter], UriKind.Absolute));
            }
            else
            {
                // Reset the text and image if no character is selected
                SelectedQuoteText = "Select a character to display quotes.";
                CharacterImageSource = null;
            }
        }

        /// <summary>
        /// Gets a random quote from the quotes collection.
        /// </summary>
        public void GetRandomQuote()
        {
            if (Quotes.Any())
            {
                var random = new Random();
                var randomQuote = Quotes[random.Next(Quotes.Count)];
                var formattedText = randomQuote.Text.TrimEnd(',', ' ', '-');
                var bookText = randomQuote.Book?.TrimEnd(','); // Remove trailing comma from the book title
                SelectedQuoteText =
                    $"{formattedText}\n\n{randomQuote.Author}\n\n{bookText} \n\n ------ \n\n"; // Removed '-' before author's name
            }
            else
            {
                SelectedQuoteText = "No quotes available.";
            }
        }

        /// <summary>
        /// Invokes the PropertyChanged event. This notifies the UI that a property value has changed. In this instance, the UI is notified that the SelectedQuoteText property has changed.
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}