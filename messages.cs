public class Messages
{
	private Languages? currentLanguage { get; set; }
	private Dictionary<int, string> languageDictionary = new();

	public void ReadDictionary(Languages language)
	{
		if (!Enum.IsDefined(language))
		{
			throw new ArgumentOutOfRangeException(nameof(language), language, "Unsupported language.");
		}

		string languageFilePath = Path.Combine(
			AppContext.BaseDirectory,
			"Languages",
			$"{language.ToString().ToLowerInvariant()}.txt");
		ReadDictionary(languageFilePath);
	}

	public void ReadDictionary(string languageFilePath)
	{
		string languageName = Path.GetFileNameWithoutExtension(languageFilePath);
		if (!Enum.TryParse(languageName, ignoreCase: true, out Languages language) || !Enum.IsDefined(language))
		{
			throw new ArgumentException("The language file name must match a Languages enum value.", nameof(languageFilePath));
		}

		string[] messages = File.ReadAllLines(languageFilePath);
		languageDictionary = messages
			.Select((message, index) => new { Message = message, Number = index + 1 })
			.ToDictionary(entry => entry.Number, entry => entry.Message);
		currentLanguage = language;
	}

	public string GetMessage(int whichMessage)
	{
		return languageDictionary[whichMessage];
	}
}


/*
To switch languages, call ReadDictionary with a Languages value, such as Languages.French.

To call a message, follow the example below:
messages.GetMessage(1); returns "one" in English, "un" in French, etc.
*/