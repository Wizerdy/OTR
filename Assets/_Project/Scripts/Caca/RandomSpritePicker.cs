using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpritePicker : MonoBehaviour {
    [SerializeField] List<Sprite> _textures;

    void Start() {
        GetComponent<SpriteRenderer>().sprite = _textures[Random.Range(0, _textures.Count)];
    }

    void Update() {

    }
}
