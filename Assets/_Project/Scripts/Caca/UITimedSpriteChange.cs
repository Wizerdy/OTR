using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITimedSpriteChange : MonoBehaviour {
    [SerializeField] Image _target;
    [SerializeField] List<Sprite> _textures;
    [SerializeField] int _index = 0;
    [SerializeField] float _time = 2f;

    float _timer = 0f;

    void Update() {
        if (_timer < _time) {
            _timer += Time.deltaTime;
        } else {
            ++_index;
            _index %= _textures.Count;
            _timer = 0f;
            ChangeSprite(_index);
        }
    }

    void ChangeSprite(int index) {
        _target.sprite = _textures[index];
    }
}
