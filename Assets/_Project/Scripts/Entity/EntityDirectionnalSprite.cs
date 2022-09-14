using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityDirectionnalSprite : MonoBehaviour {
    [System.Serializable]
    struct DirectionSprite {
        public Vector2 direction;
        public Sprite sprite;
        public bool flipped;
    }

    [SerializeField] List<DirectionSprite> _sprites;
    [SerializeField] SpriteRenderer _renderer;

    private void Reset() {
        _renderer = GetComponent<SpriteRenderer>();
    }

    public void ChangeSprite(Vector2 direction) {
        DirectionSprite? sprite = Get(direction);
        if (sprite == null) { return; }
        _renderer.sprite = sprite.Value.sprite;
        _renderer.flipX = sprite.Value.flipped;
    }

    private DirectionSprite? Get(Vector2 direction) {
        if (_sprites.Count <= 0) { return null; }
        if (_sprites.Count == 1) { return _sprites[0]; }

        float min = Vector2.Angle(direction, _sprites[0].direction);
        float delta;
        DirectionSprite output = _sprites[0];

        for (int i = 1; i < _sprites.Count; i++) {
            delta = Vector2.Angle(direction, _sprites[i].direction);
            if (min > delta) {
                min = delta;
                output = _sprites[i];
            }
        }

        return output;
    }
}
