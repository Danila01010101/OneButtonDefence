using System.Collections;
using UnityEngine;

namespace AOneButtonDefence.Scripts.Initializators
{
    public class TutorialMaskInitializer : IGameInitializerStep
    {
        private SpotlightTutorialMask _prefab;
        private Canvas _parent;
        
        public TutorialMaskInitializer(SpotlightTutorialMask tutorialMaskPrefab, Canvas tutorialMaskCanvas)
        {
            _prefab = tutorialMaskPrefab;
            _parent = tutorialMaskCanvas;
        }
        
        public IEnumerator Initialize()
        {
            var tutorialManager = Object.Instantiate(_prefab, _parent.transform);
            yield break;
        }
    }
}