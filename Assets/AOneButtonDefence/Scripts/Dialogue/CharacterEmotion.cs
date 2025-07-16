using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// https://www.youtube.com/shorts/828Rob8Ldd0
[CreateAssetMenu(fileName = "New CharacterEmotion", menuName = "Data/CharacterEmotion Data", order = 51)]
public class CharacterEmotion : ScriptableObject
{
    [SerializeField] private Sprite emotion;

    public Sprite Emotion()
    {
        return emotion;
    }
}
