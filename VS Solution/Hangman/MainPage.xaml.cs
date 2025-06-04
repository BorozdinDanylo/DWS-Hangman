using Hangmen.BL.Interfaces;
using Hangmen.BL.Implementation;

namespace Hangman
{
    public partial class MainPage : ContentPage
    {
        // IWordPool wordPool = new WordPool();
        // IGameManager gameManager = new GameManager(wordPool);

        public MainPage()
        {
            InitializeComponent();
        }

        private void CheckLetterCommand(object? sender, EventArgs e)
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
        private void EnterLetterCommand(object? sender, EventArgs e) { }
        private void ExitGameCommand(object? sender, EventArgs e) { }
        private void RestartGameCommand(object? sender, EventArgs e) { }
    }
}
