using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "New SkinData", menuName = "ScriptableObjects/new skin Data", order = 59)]
public class SkinData : ScriptableObject
{
    [Header("Внешний вид скина")]
    public Mesh Mesh;
    public Material Material;
    [Header("Иконка в UI")]
    public Sprite Icon; 
    [Header("Информация о скине")]
    public int Cost; 
    public string SkinName;
    [TextArea(3, 10)]
    public string SkinLore; 
}
