using System.Collections;
using System.Collections.Generic;
using ToolsBoxEngine;
using UnityEngine;

public class Staff : Weapon
{
    [SerializeField] private GameObject projectile;
    [SerializeField] float _attackTime = 0.2f;
    [SerializeField] float _projectileSpeed = 0.2f;
    [SerializeField] private float chargeTime = 2.0f;

    private string _triggerName = "Staff_shoot";

    protected override void _OnStart() {
        _attacks.Add(AttackIndex.FIRST, IAttack);
    }

    protected IEnumerator IAttack(Vector2 direction) {
        if (_targetAnimator == null) { Debug.LogError(gameObject.name + " : Animator not set"); yield break; }

        _targetAnimator.SetTrigger(_triggerName);
        yield return new WaitForSeconds(chargeTime);
        GameObject proj = Instantiate(projectile, transform.position, Quaternion.identity);
        //coroutineCharge = CoroutinesManager.Start(Tools.Delay(() => proj.ReturnSpriteRenderer().color.r -= 0.1f , chargeTime));
        Debug.Log(direction);
        proj.GetComponent<Rigidbody2D>().velocity = direction * _projectileSpeed;

        yield return new WaitForSeconds(_attackTime);
    }
}