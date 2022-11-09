using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponThreat : MonoBehaviour {
    [SerializeField] EntityWeaponry _weaponry;
    [SerializeField] EntityMenacePoint _menacePoints;

    void Start() {
        _weaponry.OnAttackHit += _AddThreatPoint;
    }

    private void OnDestroy() {
        _weaponry.OnAttackHit -= _AddThreatPoint;
    }

    void _AddThreatPoint(Weapon weapon, AttackIndex index, IHealth health, int damage) {
        _menacePoints.Add(weapon.GetAttack(index).threatPoint);
    }
}
