using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ColorDataSO", menuName = "GameData/Material Collection")]
public class ColorDataSO : ScriptableObject
{
    [SerializeField] private Material[] materials;

    public Material GetMaterial(ColorType colorType)
    {
        return materials[(int)colorType];
    }


}