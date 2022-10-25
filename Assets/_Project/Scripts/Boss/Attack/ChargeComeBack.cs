using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ChargeComeBack : BossAttack {
    [SerializeField] float _speed;
    [SerializeField] float _duration;
    [SerializeField] EntityMovement _entityMovement;
    [SerializeField] EntityAbilities _ea;
    [SerializeField] bool _firsthit = false;
    public override void Activate(EntityAbilities ea, Transform target) {
        _firsthit = true;
        _ea = ea;
        _entityMovement = ea.Get<EntityMovement>();
        _entityMovement.CreateMovement(_duration, _speed, target.position - ea.gameObject.transform.position);
    }

    public override void Activate(EntityAbilities ea, Transform[] targets) {

    }

    public void Hit(Collision2D collision) {
        if (collision.transform.tag == "Wall" && _entityMovement != null) {
            _entityMovement.StopMovement();
            if (_firsthit) {
                _firsthit = false;
                _entityMovement.CreateMovement(_duration, _speed, Vector3.zero - _ea.gameObject.transform.position);
            }
        }
    }


}
