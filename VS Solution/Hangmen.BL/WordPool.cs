namespace Hangmen.BL;

public sealed class WordPool: IWordPool
{

    private string[] _words = 
    {
        "Apfel",
        "Hund",
        "Blume",
        "Tisch",
        "Fenster",
        "Stuhl",
        "Buch",
        "Wasser",
        "Haus",
        "Schule",
        "Freund",
        "Katze",
        "Baum",
        "Straße",
        "Auto",
        "Wolke",
        "Zug",
        "Brot",
        "Lampe",
        "Uhr"
    };


    private Random _random = new Random();

    public string GetWordFromIndex(int index)
    {
        if(index<0 || index >= _words.Length)
            throw new ArgumentOutOfRangeException(nameof(index), "Index cannot be negative or bigger than list size");

        return _words[index];
    }
    public string MaskWordFromIndex(int index, int ratio)
    {
        if (index < 0 || index >= _words.Length)
            throw new ArgumentOutOfRangeException(nameof(index), "Index cannot be negative or bigger than list size");

        if (ratio <= 0 || ratio >= 100)
            throw new ArgumentOutOfRangeException(nameof(ratio), "Ratio cannot be lower or equal zero or bigger than 100");


        string originalWord = _words[index];
        char[] maskedChars = originalWord.ToCharArray();

        int wordLength = originalWord.Length;
        int numberOfLettersToMask = (int)Math.Round(wordLength * (ratio / 100.0));

        // Випадковий вибір позицій для маскування
        IEnumerable<int> positionsToMask = Enumerable
            .Range(0, wordLength)
            .OrderBy(_ => _random.Next())
            .Take(numberOfLettersToMask);

        foreach (int position in positionsToMask)
        {
            maskedChars[position] = '_';
        }

        return new string(maskedChars);
    }
    public int GetRandomWord()
    {
        return _random.Next(_words.Length);
    }
    public string RevealLetter(string maskedWord, int index, char input)
    {
        if (index < 0 || index >= _words.Length)
            throw new ArgumentOutOfRangeException(nameof(index), "Index cannot be negative or bigger than list size");

        if (maskedWord.Length != _words[index].Length)
            throw new ArgumentException("Words must be the same length.");

        char[] revealedChars = maskedWord.ToCharArray();

        for (int i = 0; i < _words[index].Length; i++)
        {
            if (_words[index][i] == input && maskedWord[i] == '_')
            {
                revealedChars[i] = input;
            }
        }

        return new string(revealedChars);
    }
    public bool ContainsLetter(int index, char letter)
    {
        if(index<0 || index >= _words.Length)
            throw new ArgumentOutOfRangeException(nameof(index), "Index cannot be negative or bigger than list size");

        return _words[index].Contains(letter);
    }
}
