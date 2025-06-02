namespace Hangmen.BL.Interfaces;
public enum GameState
{
    NotStarted,
    Started,
    Reseted,
    InProgress,
    Won,
    Lost
}

public enum InvaildInputType
{
    EmptyInput,
    MoreThanOneCharacter,
    InvalidCharacter,
    AlreadyGuessed
}

public delegate void GameStateChangedEventHandler(GameState state);
public delegate void WrongGuessEventHandler(int leftGuesses, List<GuessedLetterData> guessedLetters);
public delegate void CorrectGuessChangedEventHandler(bool won,string updatedMaskedWord,List<GuessedLetterData> guessedLetters);
public delegate void InvaildInputEventHandler(InvaildInputType invaildInputType);

public interface IGameManager
{
    public void StartNewGame(int maxAttempts, int ratio);
    public void ResetGame();

    public GameState GetGameState();

    public bool TryInputLetter(string letter);
    public int GetRemainingAttempts();
    public int GetWordLength();
    public string GetRawWord();
    public string GetMaskedWord();
    public List<char> GetGuessedLetters();
    public List<char> GetWrongGuessedLetters();
    public List<char> GetCorrectGuessedLetters();

    public event GameStateChangedEventHandler GameStateChanged;
    public event WrongGuessEventHandler WrongGuessMade;
    public event CorrectGuessChangedEventHandler CorrectGuessMade;
    public event InvaildInputEventHandler InvaildInputReceived;
}
