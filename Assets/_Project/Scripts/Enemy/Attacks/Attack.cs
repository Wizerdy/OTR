using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine;

[CreateAssetMenu(fileName = "New Attack", menuName = "Scriptable Object")]
public class Attack : ScriptableObject {
    [SerializeField] AmplitudeCurve _dash = new AmplitudeCurve();
    [SerializeField] float _chargeTime = 1f;

    public IEnumerator Use(GameObject caster, GameObject target, Vector2 direction) {
        EntityAbilities abilities = caster.GetComponent<EntityAbilities>();
        if (abilities == null) { Debug.LogError("No abilties found"); }
        EntityMovement movements = abilities.Get<EntityMovement>();
        if (movements == null) { Debug.LogError("No movements found"); }

        _dash.Reset();
        movements.CanMove = false;
        movements.Stop();

        yield return new WaitForSeconds(_chargeTime);

        while (_dash.Percentage < 1f) {
            UpdateDash(movements.Rigidbody, direction);
            yield return null;
        }
        movements.CanMove = true;
    }

    void UpdateDash(Rigidbody2D rb, Vector2 direction) {
        _dash.UpdateTimer(Time.deltaTime);
        float speed = _dash.Evaluate() * _dash.amplitude;

        rb.velocity = speed * direction;
    }
}
