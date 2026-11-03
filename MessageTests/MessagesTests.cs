using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class MessagesTests
{
    [TestMethod]
    [DataRow(Languages.English, "Start Game Activity Diagram", "You are walking in the mountains...Enter 'n' for North. Enter 's' for South. Enter 'e' for exit game", "The dragon says 'I am ", "The dragon laughs as your eyesight fades... The End.")]
    [DataRow(Languages.French, "Start Game Activity Diagram", "Vous marchez dans les montagnes... Entrez 'n' pour le Nord. Entrez 's' pour le Sud. Entrez 'e' pour quitter le jeu", "Le dragon dit : « Je suis ", "Le dragon rit tandis que votre vue s'éteint... Fin.")]
    [DataRow(Languages.Spanish, "Start Game Activity Diagram", "Caminas por las montañas... Introduce 'n' para ir al Norte. Introduce 's' para ir al Sur. Introduce 'e' para salir del juego", "El dragón dice: «Soy ", "El dragón se ríe mientras tu vista se apaga... Fin.")]
    public void ReadDictionary_LoadsBundledLanguageFiles(Languages language, string firstMessage, string thirdMessage, string messageAfterGap, string finalMessage)
    {
        var messages = new Messages();

        messages.ReadDictionary(language);

        Assert.AreEqual(firstMessage, messages.GetMessage(1));
        Assert.AreEqual(thirdMessage, messages.GetMessage(3));
        Assert.AreEqual(messageAfterGap, messages.GetMessage(6));
        Assert.AreEqual(finalMessage, messages.GetMessage(59));
        AssertMissingMessage(messages, 60);
    }

    [TestMethod]
    public void ReadDictionary_SwitchesBetweenBundledLanguages()
    {
        var messages = new Messages();
        messages.ReadDictionary(Languages.English);

        Assert.AreEqual("Start Game Activity Diagram", messages.GetMessage(1));

        messages.ReadDictionary(Languages.French);

        Assert.AreEqual("Start Game Activity Diagram", messages.GetMessage(1));
        Assert.AreEqual("Bienvenue dans le jeu « Choisis ton aventure »", messages.GetMessage(2));
    }

    [TestMethod]
    public void ReadDictionary_StillAcceptsLanguageFilePath()
    {
        var messages = new Messages();

        messages.ReadDictionary(GetLanguageFile("Spanish.json"));

        Assert.AreEqual("Bienvenido al juego «Elige tu propia aventura»", messages.GetMessage(2));
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