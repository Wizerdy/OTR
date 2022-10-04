using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeShield : Weapon
{
    [SerializeField] private float aggroPointGenerated = 0.5f;
    [SerializeField] private float parryCooldown = 0.5f;
    [SerializeField] private float armorPointCurrent = 10f;
    [SerializeField] private float armorPointRegenerationRate = 1.5f;
    [SerializeField] private float armorPointRegenerationValue = 1f;
    [SerializeField] private float armorPointMax = 10f;
    [SerializeField] private float armorPointOnPickUp = 5f;
    [SerializeField] DirectionalModifier _directionalModifier;
    [SerializeField] float _attackTime = 0.2f;
    [SerializeField, Range(0f, 1f)] float _aimingSpeed = 0.5f;

    float _baseSpeed = 1f;
    bool _aiming = false;

    string _boolName_aim = "AxeShield_Slash";

    string _triggerName_attack = "AxeShield_Slash";
    protected override void _OnStart() {
        _baseSpeed = MoveSpeed;
    }

    protected override void _OnAim(Vector2 direction) {
        if (direction == Vector2.zero) {
            if (_aiming) { StopAiming(); }
            return;
        }

        if (!_aiming) { StartAiming(); }
        UpdateAim(direction);
    }
    private void StartAiming() {
        _aiming = true;
        MoveSpeed = _aimingSpeed;
        _directionalModifier.enabled = true;
        _targetAnimator.SetBool(_boolName_aim, true);
    }

    private void UpdateAim(Vector2 direction) {
        _directionalModifier.Direction = direction;
    }

    private void StopAiming() {
        _aiming = false;
        _directionalModifier.enabled = false;
        _targetAnimator.SetBool(_boolName_aim, false);
        MoveSpeed = _baseSpeed;
    }
    protected override void _OnPickup(EntityWeaponry weaponry) {
        armorPointCurrent = armorPointOnPickUp;

        weaponry.Health.AddDamageModifier(_directionalModifier);
    }

    protected override void _OnDrop(EntityHolding holding) {
        armorPointCurrent = 0;
        _targetAnimator.SetBool(_boolName_aim, false);
        _aiming = false;
        MoveSpeed = _baseSpeed;
    }

    protected override void _OnDrop(EntityWeaponry weaponry) {
        weaponry.Health.RemoveDamageModifier(_directionalModifier);
    }
    protected override IEnumerator IAttack(Vector2 direction) {
        if (_targetAnimator == null) { Debug.LogError(gameObject.name + " : Animator not set"); yield break; }
        if (!_aiming) { yield break; }

        _targetAnimator.SetTrigger(_triggerName_attack);
        yield return new WaitForSeconds(_attackTime);
    }
}
