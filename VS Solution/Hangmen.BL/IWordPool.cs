namespace Hangmen.BL;

public interface IWordPool
{
    public int GetRandomWord();
    public string GetWordFromIndex(int index);

    public string MaskWordFromIndex(int index, int ratio);

    public bool ContainsLetter(int poolWordIndex, char letter);
    public string RevealLetter(string maskedWord, int poolWorldIndex, char input);
}
