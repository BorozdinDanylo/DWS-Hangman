using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Hangmen.BL;
using Hangmen.BL.Interfaces;
namespace Hangman
{
    public partial class MainPage : ContentPage
    {
        private IGameManager _gameManager;
        private string[] hangman_images =
        {
            "hangman_sprite_1.png",
            "hangman_sprite_2.png",
            "hangman_sprite_3.png",
            "hangman_sprite_4.png",
            "hangman_sprite_5.png",
            "hangman_sprite_6.png",
            "hangman_sprite_7.png"
        };
        private bool isQuitGame = false;
        private int maxAttempts = 6;
        private int ratio = 80;
        public MainPage(IGameManager gameManager)
        {
            InitializeComponent();

            _gameManager = gameManager ?? throw new ArgumentNullException(nameof(gameManager));


            _gameManager.GameStateChanged += (state) =>
            {
                HandelGameState(state);
            };

            _gameManager.WrongGuessMade += (leftGuesses, guessedLetters) =>
            {
                HandelIncorrectGuess(leftGuesses, guessedLetters);
            };

            _gameManager.CorrectGuessMade += (won, updatedMaskedWord, guessedLetters) =>
            {
                HandelCorrectGuess(updatedMaskedWord);
            };

            _gameManager.InvaildInputReceived += async (invaildInputType) =>
            {
                await HandelDetectedInvalidInput(invaildInputType);
            };
        }

        private void HandelGameState(GameState state)
        {
            switch (state)
            {
                case GameState.Started:
                    InitGameUI();
                    break;
                case GameState.Reseted:
                    if (!isQuitGame)
                    {
                        _gameManager.StartNewGame(maxAttempts, ratio);
                    }
                    else
                    {
                        ResetGameUI();
                    }

                    isQuitGame = false;
                    break;

                case GameState.Won:
                    InitGameOverUI("Congratulations! You won the game!");
                    break;
                case GameState.Lost:
                    InitGameOverUI($"You lost the game. Better luck next time! \n The word you have failed to guess is '{_gameManager.GetRawWord()}'");         
                    break;
            }
        }

        private void InitGameOverUI(string text)
        {
            LetterInput.IsVisible = false;
            ResetButton.IsVisible = true;
            EnterLetter.IsVisible = false;
            GameOver.Text = text;
            GameOver.IsVisible = true;
        }

        private void HandelIncorrectGuess(int leftGuesses, List<GuessedLetterData> guessedLetters)
        {
            IEnumerable<char> wrongLetters = guessedLetters.Where(l => l.LetterType == GuessedLetterType.Wrong).Select(l => l.Letter);

            WrongLetters.Text = string.Join(", ", wrongLetters);
            WrongsCounter.Text = $"Wrong guesses left: {leftGuesses}";

            LetterInput.Text = string.Empty;

            ProgresImage.Source = hangman_images[maxAttempts - leftGuesses];
        }
        private void HandelCorrectGuess(string updatedMaskedWord)
        {
            LetterInput.Text = string.Empty;
            ShowedWord.Text = updatedMaskedWord;
        }
        private async Task HandelDetectedInvalidInput(InvaildInputType invaildInputType)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            string text = string.Empty;
            ToastDuration duration = ToastDuration.Short;
            double fontSize = 14;

            switch (invaildInputType)
            {
                case InvaildInputType.EmptyInput:
                    text = "You entered an empty input. Please retry.";
                    break;
                case InvaildInputType.InvalidCharacter:
                    text = "You entered an invalid character. Please retry.";
                    break;
                case InvaildInputType.AlreadyGuessed:
                    text = "You already guessed this letter. Please take another one.";
                    break;
            }
            var toast = Toast.Make(text, duration, fontSize);
            LetterInput.Text = string.Empty;
            await toast.Show(cancellationTokenSource.Token);

        }

        private void InitGameUI()
        {
            StartImage.IsVisible = false;
            TitleLabel.IsVisible = false;
            StartButton.IsVisible = false;
            ExitAppButton.IsVisible = false;
            GameOver.IsVisible = false;

            ShowedWord.IsVisible = true;
            WrongsCounter.IsVisible = true;
            ProgresImage.IsVisible = true;
            LetterInput.IsVisible = true;
            EnterLetter.IsVisible = true;
            ExitGameButton.IsVisible = true;
            WrongLettersTitle.IsVisible = true;
            WrongLetters.IsVisible = true;
            ResetButton.IsVisible = false;

          
            WrongsCounter.Text = $"Wrong guesses left: {maxAttempts}";

            ProgresImage.Source = hangman_images[0];
            ShowedWord.Text = _gameManager.GetMaskedWord();
        }
        private void ResetGameUI()
        {
            WrongLetters.Text = string.Empty;
            LetterInput.Text = string.Empty;

            TitleLabel.IsVisible = true;
            StartImage.IsVisible = true;
            StartButton.IsVisible = true;
            ExitAppButton.IsVisible = true;

            WrongsCounter.IsVisible = false;
            GameOver.IsVisible = false;
            ProgresImage.IsVisible = false;
            ShowedWord.IsVisible = false;
            LetterInput.IsVisible = false;
            EnterLetter.IsVisible = false;
            ExitGameButton.IsVisible = false;
            WrongLettersTitle.IsVisible = false;
            WrongLetters.IsVisible = false;
            ResetButton.IsVisible = false;
        }
        private void VaildateLetterInput()
        {
            string? letter = LetterInput.Text;

            if (letter == null)
            {
                return;
            }

            if (letter.Length != 1 || !char.IsLetter(letter[0]))
            {
                LetterInput.TextColor = Colors.Red;
            }
            else
            {
                LetterInput.TextColor = Colors.Black;
            }


        }

        private void StartGame_Clicked(object? sender, EventArgs e)
        {
            _gameManager.StartNewGame(maxAttempts, ratio);
        }
        private void InputLetter_TextChanged(object? sender, EventArgs e)
        {
            VaildateLetterInput();
        }
        private void ExitGame_Clicked(object? sender, EventArgs e)
        {
            isQuitGame = true;
            _gameManager.ResetGame();
        }
        private void ExitApp_Clicked(object? sender, EventArgs e)
        {
            Application.Current?.Quit();
        }
        private void EnterLetter_Clicked(object sender, EventArgs e)
        {
            _gameManager.TryInputLetter(LetterInput.Text);
        }
        private void ResetButton_Clicked(object sender, EventArgs e)
        {
            _gameManager.ResetGame();
        }
    }
}
