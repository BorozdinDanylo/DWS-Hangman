using Hangmen.BL.Interfaces;
using Hangmen.BL.Implementation;

namespace Hangman
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void CheckLetter(object? sender, EventArgs e)
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
        private void EnterLetter(object? sender, EventArgs e) { }
        private void ResetGame(object? sender, EventArgs e) { }
        private void ExitGame(object? sender, EventArgs e) { }
    }
}
