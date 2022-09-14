using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TwoDSideEntity : MonoBehaviour {
    [SerializeField] Controller _controller;
    List<ColliderTwoD> _colliders;
    [SerializeField] ColliderTwoD _rightCollider;
    [SerializeField] ColliderTwoD _botCollider;
    [SerializeField] ColliderTwoD _leftCollider;
    [SerializeField] ColliderTwoD _topCollider;

    [SerializeField] float _acceleration;
    [SerializeField] float _maxSpeed;
    [SerializeField] float _minSpeedToFriction;
    [SerializeField] float _groundFriction;
    [SerializeField] float _wallFriction;
    [SerializeField] float _comebackFriction;
    [SerializeField] float _airFriction;
    [SerializeField] float _jumpForce;
    bool _canJumpAgain = true;

    [SerializeField] Vector2 _direction;
    [SerializeField] Vector2 _strenghts;
    [SerializeField] bool _jump;
    Vector2 Velocity { get { return _rbTwoD.velocity; } set { _rbTwoD.velocity = value; } }
    float VelocityX { get { return _rbTwoD.velocity.x; } set { _rbTwoD.velocity = new Vector2(value, _rbTwoD.velocity.y); } }
    float VelocityY { get { return _rbTwoD.velocity.y; } set { _rbTwoD.velocity = new Vector2(_rbTwoD.velocity.x, value); } }


    public float XDirection { get { return _direction.x; } set { _direction.x = Mathf.Clamp(value, -1, 1); } }
    public float YDirection { get { return _direction.y; } set { _direction.y = Mathf.Clamp(value, -1, 1); } }


    Rigidbody2D _rbTwoD;

    private void Start() {
        _rbTwoD = GetComponent<Rigidbody2D>();
        _colliders = new List<ColliderTwoD>();
        _colliders.AddRange(GetComponentsInChildren<ColliderTwoD>());
    }

    private void Update() {
        GetInput();
        Displacement();
        Friction();
        Jump();
        ApplyPhysics();
    }

    void Displacement() {
        if (Mathf.Abs(VelocityX) < _maxSpeed) {
            _strenghts.x += XDirection * _acceleration;
        }
    }

    void Friction() {
        if (XDirection == 0) {
            if (Mathf.Abs(VelocityX) > _minSpeedToFriction) {
                if (IsInAir()) {
                    _strenghts.x += Mathf.Sign(VelocityX) == 1 ? -_airFriction : _airFriction;
                } else {
                    _strenghts.x += Mathf.Sign(VelocityX) == 1 ? -_groundFriction : _groundFriction;
                    Debug.Log("friction");
                }
            } else if (!IsInAir() && Mathf.Abs(VelocityX) <= _minSpeedToFriction) {
                VelocityX = 0;
            }
        } else if (XDirection != Mathf.Sign(VelocityX)) {
            _strenghts.x += Mathf.Sign(VelocityX) == 1 ? -_comebackFriction : _comebackFriction;
        }

        if (Mathf.Abs(VelocityY) > _minSpeedToFriction) {
            if (IsInAir()) {
                _strenghts.y += Mathf.Sign(VelocityY) == 1 ? -_airFriction : _airFriction;
            } else {
                _strenghts.y += Mathf.Sign(VelocityY) == 1 ? -_wallFriction : _wallFriction;
            }
        }
    }

    void Jump() {
        if (_jump && _canJumpAgain) {
            if (_rightCollider.Contact && !_leftCollider.Contact && !_botCollider.Contact) {
                Velocity = Vector2.zero;
                _rbTwoD.AddForce((Vector2.up + Vector2.left).normalized * _jumpForce);
                _canJumpAgain = false;
                StartCoroutine(WhileColliderContact(new ColliderTwoD[] { _rightCollider }));
            } else if (!_rightCollider.Contact && _leftCollider.Contact && !_botCollider.Contact) {
                Velocity = Vector2.zero;
                _rbTwoD.AddForce((Vector2.up + Vector2.right).normalized * _jumpForce);
                _canJumpAgain = false;
                StartCoroutine(WhileColliderContact(new ColliderTwoD[] { _leftCollider }));
            } else if (_botCollider.Contact) {
                VelocityY = 0;
                _rbTwoD.AddForce(Vector2.up * _jumpForce);
                _canJumpAgain = false;
                StartCoroutine(WhileColliderContact(new ColliderTwoD[] { _botCollider }));
            }
        }
    }

    void ApplyPhysics() {
        Velocity += _strenghts * Time.deltaTime;
        Debug.Log("S " + _strenghts);
        Debug.Log("SDT " + _strenghts * Time.deltaTime);
        Debug.Log("DT " + Time.deltaTime);
        Debug.Log("V " + Velocity);
        //Debug.Log("A " + IsInAir());
        if ((_strenghts * Time.deltaTime) != Vector2.zero) {
            _strenghts = Vector2.zero;
        }
    }

    bool IsInAir() {
        bool isInAir = true;
        for (int i = 0; i < _colliders.Count; i++) {
            if (_colliders[i].Contact) {
                isInAir = false;
                break;
            }
        }
        return isInAir;
    }

    void GetInput() {
        if (_controller) {
            _direction = _controller.Direction;
            _jump = _controller.Space;
        }
    }

    IEnumerator WhileColliderContact(ColliderTwoD[] colliders) {
        bool noContact = false;
        while (!noContact) {
            noContact = true;
            for (int i = 0; i < colliders.Length; i++) {
                if (colliders[i].Contact) {
                    noContact = false;
                }
            }
            yield return null;
        }
        _canJumpAgain = true;
    }
}





