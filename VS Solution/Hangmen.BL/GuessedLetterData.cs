namespace Hangmen.BL;

public enum GuessedLetterType
{
    Correct,
    Wrong
}

public class GuessedLetterData
{
    public char Letter { get; set; }
    public GuessedLetterType LetterType { get; set; }
    public GuessedLetterData(char letter, GuessedLetterType letterType)
    {
        Letter = letter;
        LetterType = letterType;
    }
}
