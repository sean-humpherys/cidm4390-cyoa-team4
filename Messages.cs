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
			$"{language}.json");
		ReadDictionary(languageFilePath);
	}

	public void ReadDictionary(string languageFilePath)
	{
		string languageName = Path.GetFileNameWithoutExtension(languageFilePath);
		if (!Enum.TryParse(languageName, ignoreCase: true, out Languages language) || !Enum.IsDefined(language))
		{
			throw new ArgumentException("The language file name must match a Languages enum value.", nameof(languageFilePath));
		}

		languageDictionary = System.Text.Json.JsonSerializer.Deserialize<Dictionary<int, string>>(File.ReadAllText(languageFilePath))
			?? throw new InvalidDataException("The language file must contain a JSON object of numbered messages.");
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