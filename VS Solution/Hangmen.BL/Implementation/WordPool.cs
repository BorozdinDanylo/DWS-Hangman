using Hangmen.BL.Interfaces;

namespace Hangmen.BL.Implementation;

public sealed class WordPool: IWordPool
{

    private string[] _wordsPool = 
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
        if(index<0 || index >= _wordsPool.Length)
            throw new ArgumentOutOfRangeException(nameof(index), "Index cannot be negative or bigger than list size");

        return _wordsPool[index];
    }
    public string MaskWordFromIndex(int index, int ratio)
    {
        if (index < 0 || index >= _wordsPool.Length)
            throw new ArgumentOutOfRangeException(nameof(index), "Index cannot be negative or bigger than list size");

        if (ratio <= 0 || ratio >= 100)
            throw new ArgumentOutOfRangeException(nameof(ratio), "Ratio cannot be lower or equal zero or bigger than 100");


        string originalWord = _wordsPool[index];
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
        return _random.Next(_wordsPool.Length);
    }
    public string RevealLetter(string maskedWord, int poolIndex, char input)
    {
        if (poolIndex < 0 || poolIndex >= _wordsPool.Length)
            throw new ArgumentOutOfRangeException(nameof(poolIndex), "Index cannot be negative or bigger than list size");

        if (maskedWord.Length != _wordsPool[poolIndex].Length)
            throw new ArgumentException("Words must be the same length.");

        char[] maskedChars = maskedWord.ToCharArray();
        char[] revealedChars = _wordsPool[poolIndex].ToCharArray();

        for (int i = 0; i < revealedChars.Length; i++)
        {
            if (string.Equals(revealedChars[i].ToString(),input.ToString(),StringComparison.OrdinalIgnoreCase) && maskedChars[i] == '_')
            {
                maskedChars[i] = revealedChars[i];
            }
        }

        return new string(maskedChars);
    }
    public bool ContainsLetter(int index, char letter)
    {
        if(index<0 || index >= _wordsPool.Length)
            throw new ArgumentOutOfRangeException(nameof(index), "Index cannot be negative or bigger than list size");

        return _wordsPool[index].Contains(letter,StringComparison.OrdinalIgnoreCase);
    }
}
