using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chakram : Weapon {
    [SerializeField] ChakramShockwave _shockwave;
    [SerializeField] float _attackTime = 0.2f;
    [SerializeField] public int Stack;
    [SerializeField] SpriteRenderer sr;

    private void Start() {
        sr = GetComponentInChildren<SpriteRenderer>();
        ColorUpdate();
    }
    protected override IEnumerator IAttack(Vector2 direction) {
        if (_targetAnimator == null) { Debug.LogError(gameObject.name + " : Animator not set"); yield break; }

        Debug.Log(direction);
        ChakramShockwave newShockwave =  Instantiate(_shockwave, transform.position, Quaternion.identity);
        newShockwave._direction = direction;
        newShockwave._stack = Stack;
        Stack = 1;
        ColorUpdate();
        yield return new WaitForSeconds(_attackTime);
    }

    protected override void _OnPickup(EntityWeaponry weaponry) {
        Stack++;
        ColorUpdate();
    }

    void ColorUpdate() {
        sr.color = new Color(0,0, 1f / 5 * Stack);
        if(Stack >= 5) {
            transform.localScale = new Vector3(2, 2, 2);
        } else {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
