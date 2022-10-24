using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine;

public class DistanceModifier : DamageModifier {
    [SerializeField] Vector4 _radiuses = new Vector4(2f, 4f, 5f, 7f);

    AditionalDamageByDistance _module = null;

    protected override bool Usable(int value, GameObject source) {
        _module = source.GetComponent<AditionalDamageByDistance>();
        if (_module == null) { return false; }

        return true;
    }

    protected override int Modify(int amount) {
        amount = base.Modify(amount);
        if (_module == null) { return amount; }

        float distance = (transform.Position2D() - _module.StartPosition).magnitude;
        float percentage;

        if (distance > _radiuses.y && distance < _radiuses.z) {
            percentage = 1f;
        } else {
            percentage = (distance - _radiuses.x) / (_radiuses.y - _radiuses.x);
            if (percentage > 1f) {
                percentage = 1f - ((distance - _radiuses.z) / (_radiuses.w - _radiuses.z));
            }
        }

        //Debug.Log(amount + _module.ComputeDamages(Mathf.Max(0f, Mathf.Min(1f, percentage))) + " .. p:" + percentage + " .. d:" + distance);
        return amount + _module.ComputeDamages(Mathf.Max(0f, Mathf.Min(1f, percentage)));
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, _radiuses.y);
        Gizmos.DrawWireSphere(transform.position, _radiuses.z);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _radiuses.x);
        Gizmos.DrawWireSphere(transform.position, _radiuses.w);
    }
}
