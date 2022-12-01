using UnityEngine;
using ToolsBoxEngine;

public class PixelMap : ScriptableObject {
    public SerializedDictionary<Color32, Vector2Int> lookup = new();
    public Color32[] data;
}