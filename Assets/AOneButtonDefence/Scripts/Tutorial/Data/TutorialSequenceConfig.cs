using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Tutorials/Sequence Config")]
public class TutorialSequenceConfig : ScriptableObject
{
    [Serializable]
    public class TutorialEntry
    {
        [HideInInspector] public int index;
        public TutorialObject tutorialObject;
        public string messagePreview;
    }

    public List<TutorialEntry> tutorials = new List<TutorialEntry>();
}