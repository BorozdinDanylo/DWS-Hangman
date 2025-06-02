// See https://aka.ms/new-console-template for more information
using Hangmen.BL;

bool debugMode = false;

IWordPool wordPool = new WordPool();

int wordPoolIndex = wordPool.GetRandomWord();
string rawWordValue = wordPool.GetWordFromIndex(wordPoolIndex);
string maskedWordValue = wordPool.MaskWordFromIndex(wordPoolIndex, 90);

bool isGameWon = false;
bool isGameOver = false;
int mistakesCount = 0;
List<char> guessedLetters = new();
List<char> worngGuessedLetters = new();
List<char> correctGuessedLetters = new();

if (debugMode)
{
    Console.WriteLine(rawWordValue);
}

Console.WriteLine($"The word to guess has {rawWordValue.Length} letters.");
do
{


    Console.WriteLine(maskedWordValue);

    Console.WriteLine("Please enter your guessed letters:");

    string input = Console.ReadLine();

    if (input == null || input.Length != 1)
    {
        Console.WriteLine("Please enter a single letter.");
        continue;
    }
    else if (!char.IsLetter(input[0]))
    {
        Console.WriteLine("You entered a invaild input. Please retry");
        continue;
    }
    else if (guessedLetters.Contains(input[0]))
    {
        Console.WriteLine("You already guessed this letter. Please take another one");
        continue;
    }

    char guessedLetter = input[0];

    guessedLetters.Add(guessedLetter);

    if (wordPool.ContainsLetter(wordPoolIndex, guessedLetter))
    {
        Console.WriteLine("Good job! You guessed a letter.");

        maskedWordValue = wordPool.RevealLetter(maskedWordValue, wordPoolIndex, guessedLetter);

        correctGuessedLetters.Add(guessedLetter);
    }
    else
    {
        mistakesCount++;
        worngGuessedLetters.Add(guessedLetter);
        Console.WriteLine("Sorry, that letter is not in the word.");
        Console.WriteLine($"You have {7 - mistakesCount} mistakes left.");
    }

    isGameOver = worngGuessedLetters.Count >= 7;
    isGameWon = worngGuessedLetters.Count < 7 && string.Equals(rawWordValue, maskedWordValue);

    if (isGameOver)
    {
        Console.WriteLine("You have made too many mistakes. Game over!");
        Console.WriteLine($"The word you have failed to guess is '{rawWordValue}'");
        break;
    }
    else if (isGameWon)
    {
        Console.WriteLine("You won!");
        break;
    }

}
while (!isGameOver);


