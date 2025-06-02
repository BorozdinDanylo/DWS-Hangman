using Hangmen.BL.Interfaces;

namespace Hangmen.BL.Implementation;

public class GameManager(IWordPool wordPool) : IGameManager
{
    private readonly IWordPool _wordPool = wordPool ?? throw new ArgumentNullException(nameof(wordPool));

    private int _wordPoolIndex;
    private string _rawWordValue;
    private string _maskedWordValue;
    private GameState _gameState = GameState.NotStarted;

    private int _maxAttempts = 0;
    private int _mistakesCount = 0;

    private List<GuessedLetterData> _guessedLetters = new();



    public event GameStateChangedEventHandler GameStateChanged;
    public event WrongGuessEventHandler WrongGuessMade;
    public event CorrectGuessChangedEventHandler CorrectGuessMade;
    public event InvaildInputEventHandler InvaildInputReceived;

    public List<char> GetCorrectGuessedLetters()
    {
        return _guessedLetters.Where(l => l.LetterType == GuessedLetterType.Correct).Select(l => l.Letter).ToList();
    }
    public List<char> GetGuessedLetters()
    {
        return _guessedLetters.Select(l => l.Letter).ToList();
    }
    public List<char> GetWrongGuessedLetters()
    {
        return _guessedLetters.Where(l => l.LetterType == GuessedLetterType.Wrong).Select(l => l.Letter).ToList();
    }
    public GameState GetGameState()
    {
        return _gameState;
    }


    public string GetMaskedWord()
    {
        return _maskedWordValue;
    }

    public string GetRawWord()
    {
        return _rawWordValue;
    }

    public int GetRemainingAttempts()
    {
        return _mistakesCount;
    }


    public bool TryInputLetter(string stringLetter)
    {
        if (!(_gameState == GameState.Started || _gameState == GameState.InProgress))
            return false;



        _gameState = GameState.InProgress;

        if (string.IsNullOrEmpty(stringLetter) || string.IsNullOrWhiteSpace(stringLetter))
        {
            InvaildInputReceived.Invoke(InvaildInputType.EmptyInput);
            return false;
        }
        else if (stringLetter.Length > 1)
        {
            InvaildInputReceived.Invoke(InvaildInputType.MoreThanOneCharacter);
            return false;
        }

        char letter = stringLetter[0];

        if (!char.IsLetter(letter))
        {
            InvaildInputReceived.Invoke(InvaildInputType.InvalidCharacter);
            return false;
        }
        else if (_guessedLetters.Any(l => l.Letter == letter))
        {
            InvaildInputReceived.Invoke(InvaildInputType.AlreadyGuessed);
            return false;
        }

        if (wordPool.ContainsLetter(_wordPoolIndex, letter))
        {
            _maskedWordValue = wordPool.RevealLetter(_maskedWordValue, _wordPoolIndex, letter);

            _guessedLetters.Add(new(letter, GuessedLetterType.Correct));

            EvaluateGameState();

            CorrectGuessMade?.Invoke(_gameState == GameState.Won, _maskedWordValue, _guessedLetters);
        }
        else
        {
            _mistakesCount++;

            _guessedLetters.Add(new(letter, GuessedLetterType.Wrong));

            EvaluateGameState();

            WrongGuessMade?.Invoke(_maxAttempts - _mistakesCount, _guessedLetters);
        }

        GameStateChanged?.Invoke(_gameState);
        return true;
    }

    private void EvaluateGameState()
    {
        if (_mistakesCount < _maxAttempts && string.Equals(_rawWordValue, _maskedWordValue))
        {
            _gameState = GameState.Won;
        }
        else if (_mistakesCount >= _maxAttempts)
        {
            _gameState = GameState.Lost;
        }
    }

    public int GetWordLength()
    {
        return _rawWordValue.Length;
    }
    public void ResetGame()
    {
        if (!(_gameState != GameState.Won || _gameState != GameState.Lost))
            return;

        _guessedLetters.Clear();

        _rawWordValue = string.Empty;
        _maskedWordValue = string.Empty;
        _wordPoolIndex = -1;
        _mistakesCount = 0;
        _gameState = GameState.Reseted;

        GameStateChanged?.Invoke(_gameState);
    }

    public void StartNewGame(int maxAttempts, int ratio)
    {
        if (!(_gameState == GameState.NotStarted || _gameState == GameState.Reseted))
            return;

        _gameState = GameState.Started;

        _wordPoolIndex = _wordPool.GetRandomWord();
        _rawWordValue = _wordPool.GetWordFromIndex(_wordPoolIndex);
        _maskedWordValue = _wordPool.MaskWordFromIndex(_wordPoolIndex, ratio);
        _maxAttempts = maxAttempts;

        GameStateChanged?.Invoke(_gameState);

    }
}
