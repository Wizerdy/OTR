using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staff : Weapon
{
    [SerializeField] private GameObject projectile;
    [SerializeField] float _attackTime = 0.2f;
    [SerializeField] float _projectileSpeed = 0.2f;

    Coroutine coroutineCharge = null;
    private float chargeValue = 0.0f;
    string _triggerName = "Staff_shoot";

    protected override IEnumerator IAttack(Vector2 direction) {
        if (_targetAnimator == null) { Debug.LogError(gameObject.name + " : Animator not set"); yield break; }

        _targetAnimator.SetTrigger(_triggerName);
        Instantiate(projectile, transform.position, Quaternion.identity);
        //coroutineCharge = CoroutinesManager.Start(Tools.Delay(() => _comboIndex = 0, _comboTimer));

        projectile.GetComponent<Rigidbody2D>().velocity = direction * _projectileSpeed;
        //projectile.GetComponent<Rigidbody2D>().AddForce(direction * _projectileSpeed, ForceMode2D.Force);
        Debug.Log(direction * _projectileSpeed);
        Debug.Log(projectile.GetComponent<Rigidbody2D>().velocity);

        yield return new WaitForSeconds(_attackTime);
    }

}
