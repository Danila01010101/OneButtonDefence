using System.Collections;

public class SkinDetectorInitializer : IGameInitializerStep
{
    public SkinChangeDetector Instance { get; private set; }

    public IEnumerator Initialize()
    {
        Instance = new SkinChangeDetector();
        yield break;
    }
}