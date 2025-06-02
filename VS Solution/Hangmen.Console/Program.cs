using Hangmen.BL.Implementation;
using Hangmen.BL.Interfaces;
using static System.Net.Mime.MediaTypeNames;

bool debugMode = true;

IWordPool wordPool = new WordPool();
IGameManager gameManager = new GameManager(wordPool);

gameManager.GameStateChanged += (state) =>
{

    Console.WriteLine($"Game state changed to: {state}");

    switch (state)
    {
        case GameState.Started:
            Console.WriteLine("Game has not started yet.");

            if (debugMode)
            {
                Console.WriteLine($"Debug Mode: True :: Word '{gameManager.GetRawWord()}'");
            }

            Console.WriteLine($"The word to guess has {gameManager.GetWordLength()} letters.");

            ContinueInput();

            break;
        case GameState.Reseted:
            Console.WriteLine("\n");
            Console.WriteLine("Game has been reset.");
            Console.WriteLine("\n");

            gameManager.StartNewGame(7, 90);
            break;
        case GameState.InProgress:
            Console.WriteLine("Game is in progress.");
            break;
        case GameState.Won:
            Console.WriteLine("Congratulations! You won the game!");
            break;
        case GameState.Lost:
            Console.WriteLine("You lost the game. Better luck next time!");
            Console.WriteLine($"The word you have failed to guess is '{gameManager.GetRawWord()}'");
            break;


    }

    if (state == GameState.Won || state == GameState.Lost)
    {
        Console.WriteLine("You can reset the game via typ 'r' or close it with any other input");
        string input = Console.ReadLine();

        if (input.Equals("r", StringComparison.OrdinalIgnoreCase))
        {
            gameManager.ResetGame();
        }
        else
        {
            System.Environment.Exit(0);
        }

    }
};

gameManager.WrongGuessMade += (leftGuesses, guessedLetters) =>
{

    if (leftGuesses <= 0)
        return;

    Console.WriteLine($"Wrong guess made. You have {leftGuesses} guesses left.");
    Console.WriteLine($"Guessed letters: {string.Join(", ", guessedLetters.Select(l => l.Letter))}");

    ContinueInput();
};

gameManager.CorrectGuessMade += (won, updatedMaskedWord, guessedLetters) =>
{
    Console.WriteLine($"Correct guess made. Updated masked word: {updatedMaskedWord}");
    Console.WriteLine($"Guessed letters: {string.Join(", ", guessedLetters.Select(l => l.Letter))}");

    if (!won)
    {
        ContinueInput();
    }
       
};

gameManager.InvaildInputReceived += (invaildInputType) =>
{
    switch (invaildInputType)
    {
        case InvaildInputType.EmptyInput:
            Console.WriteLine("You entered an empty input. Please retry.");
            break;
        case InvaildInputType.InvalidCharacter:
            Console.WriteLine("You entered an invalid character. Please retry.");
            break;
        case InvaildInputType.AlreadyGuessed:
            Console.WriteLine("You already guessed this letter. Please take another one.");
            break;
    }

    ContinueInput();
};

gameManager.StartNewGame(7, 90);
void ContinueInput()
{
    bool loop = true;

    do
    {
        if(gameManager.GetGameState()==GameState.Reseted)
        {
            break;
        }

        Console.WriteLine(gameManager.GetMaskedWord());

        Console.WriteLine("Please enter your guessed letters:");

        string input = Console.ReadLine();

     

        bool sucessInput = gameManager.TryInputLetter(input);

        loop = gameManager.GetGameState() == GameState.InProgress;
    }
    while (loop);
}