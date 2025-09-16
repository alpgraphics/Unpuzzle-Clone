using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum BoxColor
{
    Red,
    Yellow,
    Green,
    Blue
}
[CreateAssetMenu(fileName = "BoxColorData", menuName = "BoxData")]

public class BoxColorData : ScriptableObject
{
    [System.Serializable]
    public class ColorInfo
    {
        public BoxColor colorType;
        public Color color;
    }

    [Header("Color Mappings")] public List<ColorInfo> colorMappings = new();
    
    public Color GetColor(BoxColor boxColor)
    {
        foreach (var colorMapping in colorMappings)
        {
            if(colorMapping.colorType == boxColor)
                return colorMapping.color;
        }
        
        return Color.white;
    }
}