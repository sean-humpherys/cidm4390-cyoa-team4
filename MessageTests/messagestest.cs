using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class MessagesTests
{
    [TestMethod]
    [DataRow(Languages.English, "one", "three")]
    [DataRow(Languages.French, "un", "trois")]
    [DataRow(Languages.Spanish, "uno", "tres")]
    public void ReadDictionary_LoadsBundledLanguageFiles(Languages language, string firstMessage, string thirdMessage)
    {
        var messages = new Messages();

        messages.ReadDictionary(language);

        Assert.AreEqual(firstMessage, messages.GetMessage(1));
        Assert.AreEqual(thirdMessage, messages.GetMessage(3));
    }

    [TestMethod]
    public void ReadDictionary_SwitchesBetweenBundledLanguages()
    {
        var messages = new Messages();
        messages.ReadDictionary(Languages.English);

        Assert.AreEqual("one", messages.GetMessage(1));

        messages.ReadDictionary(Languages.French);

        Assert.AreEqual("un", messages.GetMessage(1));
        Assert.AreEqual("deux", messages.GetMessage(2));
    }

    [TestMethod]
    public void ReadDictionary_StillAcceptsLanguageFilePath()
    {
        var messages = new Messages();

        messages.ReadDictionary(GetLanguageFile("spanish.txt"));

        Assert.AreEqual("dos", messages.GetMessage(2));
    }

    [TestMethod]
    public void GetMessage_ThrowsForUnknownMessageNumber()
    {
        var messages = new Messages();
        messages.ReadDictionary(Languages.English);

        AssertMissingMessage(messages, 0);
    }

    private static string GetLanguageFile(string fileName) =>
        Path.Combine(AppContext.BaseDirectory, "Languages", fileName);

    private static void AssertMissingMessage(Messages messages, int messageNumber)
    {
        try
        {
            messages.GetMessage(messageNumber);
            Assert.Fail("Expected a KeyNotFoundException for an unknown message number.");
        }
        catch (KeyNotFoundException)
        {
        }
    }
}