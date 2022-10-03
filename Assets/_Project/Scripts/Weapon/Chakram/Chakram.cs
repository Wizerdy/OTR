using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chakram : Weapon {
    int stack;
    [SerializeField] ChakramShockwave _shockwave;
    [SerializeField] float _attackTime = 0.2f;
    [SerializeField] int maxStack;
    [SerializeField] SpriteRenderer sr;
    [Header("Particles")]
    [SerializeField] GameObject ps;
    [Header("Chose colors")]
    [SerializeField] bool useChosenColor;
    [SerializeField] Color[] chosenColors;
    [Header("Chakram scale")]
    [SerializeField] float scaleAddedToEachStack = 0.1f;

    private void Reset() {
        chosenColors = new Color[maxStack];
    }

    protected override void _OnStart() {
        sr = GetComponentInChildren<SpriteRenderer>();
        stack = 1;
        ColorUpdate();
        _attacks.Add(AttackIndex.FIRST, IAttack);
    }

    protected IEnumerator IAttack(Vector2 direction) {
        if (_targetAnimator == null) { Debug.LogError(gameObject.name + " : Animator not set"); yield break; }

        Debug.Log(direction);
        ChakramShockwave newShockwave =  Instantiate(_shockwave, transform.position, Quaternion.identity);
        newShockwave._direction = direction;
        newShockwave._stack = stack;
        stack = 1;
        ColorUpdate();
        yield return new WaitForSeconds(_attackTime);
    }

    protected override void _OnPickup(EntityWeaponry weaponry) {
        if (stack < maxStack) {
            stack++;
        }
        ColorUpdate();
    }

    protected void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag != "Player") {
            ParticleSystem.MainModule settings = ps.GetComponent<ParticleSystem>().main;
            settings.startColor = sr.color;
            GameObject obj = Instantiate(ps.gameObject, transform.position, Quaternion.identity);
            Destroy(obj, ps.GetComponent<ParticleSystem>().main.duration + 0.1f);
        }
    }

    void ColorUpdate() {
        if (!useChosenColor) {
            sr.color = new Color(1 - (1f / 5 * stack), 1 - (1f / 5 * stack), 1 - (1f / 5 * stack));
        } else {
            sr.color = chosenColors[stack - 1];
        }

        if(stack == 1) {
            transform.localScale = new Vector3(1, 1, 1);
        } else if (stack < maxStack) {
            transform.localScale = new Vector3(transform.localScale.x + scaleAddedToEachStack, transform.localScale.y + scaleAddedToEachStack, transform.localScale.z + scaleAddedToEachStack);
        }
    }
}
