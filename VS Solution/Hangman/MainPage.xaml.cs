namespace Hangman
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private void CheckLetter(object? sender, EventArgs e) { }
        private void EnterLetter(object? sender, EventArgs e) { }
        private void ResetGame(object? sender, EventArgs e) { }
        private void ExitGame(object? sender, EventArgs e) { }
    }
}
