using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine;

public class Metamorpher : MonoBehaviour {
    [SerializeField] Texture2D _defaultSkin;
    [SerializeField] SpriteRenderer _renderer;

    Texture2D _baseSkin;
    Coroutine _routine;

    private void OnEnable() {
        _baseSkin = (Texture2D)_renderer.material.GetTexture("_SkinTex");
    }

    public void ChangeSkin(Texture2D skin) {
        _renderer.material.SetTexture("_SkinTex", skin);
    }

    public void ChangeSkinForTime(Texture2D skin, float time) {
        if (_routine != null) { StopCoroutine(_routine); }
        ChangeSkin(skin);
        _routine = StartCoroutine(Tools.Delay(() => ChangeSkin(_baseSkin), time));
    }

    public void ChangeSkinForTime(float time) {
        ChangeSkinForTime(_defaultSkin, time);
    }
}
