namespace Hangmen.BL;

public interface IWordPool
{
    public int GetRandomWord();
    public string GetWordFromIndex(int index);
    public string MaskWordFromIndex(int index, int ratio);
    public string RevealLetter(string maskedWord, int index, char input);
    public bool ContainsLetter(int index, char letter);
}
