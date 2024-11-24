public class GameSaver
{
    public static GameSaver Instance;

    public Data Data { get; private set; }

    public GameSaver()
    {
        Instance = this;
    }

    public static void Save() => DataParser.Save(GameStartData.SaveFileName, Instance.Data);

    public Data Load() => Data = DataParser.Load(GameStartData.SaveFileName);
}