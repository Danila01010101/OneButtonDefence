using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "New SkinData", menuName = "ScriptableObjects/new skin Data", order = 59)]
public class SkinData : ScriptableObject
{
    [Header("������� ��� �����")]
    public Mesh Mesh;
    public Material Material;
    public AudioClip DeathSound;
    public AudioClip UnlockMusic;
    [Header("������ � UI")]
    public Sprite Icon; 
    [Header("���������� � �����")]
    public int Cost; 
    public string SkinName;
    [TextArea(3, 10)]
    public string SkinLore;
    [Header("���� �������������")]
    public bool Unlocked;
}
