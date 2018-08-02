using Assets.Scripts;


/// <summary>
/// Главный класс из которого берутся данные
/// </summary>
public static class Data
{


    public static void StartBugSelection()
    {
        BugsCollection.StartExecution();
    }

    public static BugCollection BugsCollection;
}