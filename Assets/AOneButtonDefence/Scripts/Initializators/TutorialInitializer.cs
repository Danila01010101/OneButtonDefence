using UnityEngine;
using System.Collections;

public class TutorialInitializer : IGameInitializerStep
{
    private TutorialManager _prefab;
    private Canvas _parent;

    public TutorialInitializer(TutorialManager prefab, Canvas parent)
    {
        _prefab = prefab;
        _parent = parent;
    }

    public IEnumerator Initialize()
    {
        var tutorialManager = Object.Instantiate(_prefab, _parent.transform);
        tutorialManager.Initialize(_parent);
        var tutorial = new BasicMechanicsTutorial(tutorialManager);
        yield break;
    }
}