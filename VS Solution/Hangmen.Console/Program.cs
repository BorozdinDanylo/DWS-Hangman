// See https://aka.ms/new-console-template for more information
using Hangmen.BL;

IWordPool wordPool = new WordPool();

Console.WriteLine(wordPool.GetWordFromIndex(wordPool.GetRandomWord()));

