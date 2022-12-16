using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ToolsBoxEngine;
using ToolsBoxEngine.BetterEvents;
using UnityEngine.Events;

public class Cage : MonoBehaviour {
    [SerializeField] GameObject _mask;
    [SerializeField] GameObject _spike;
    Transform _topLeft;
    Transform _botRight;
    Vector2 _position;
    Vector2 _size;
    float _duration;
    int _damagesEveryTick;
    int _damagesBonusEveryTick;
    float _tick;
    Timer _beforeDie;
    Mesh _mesh;
    MeshFilter _filter;
    PolygonCollider2D _polygonCollider;
    Dictionary<IHealth, Vector2> _hits;
    [SerializeField] BetterEvent _onCageStart = new BetterEvent();
    public event UnityAction OnCageStart { add => _onCageStart += value; remove => _onCageStart -= value; }
    [SerializeField] BetterEvent _onCageDamage = new BetterEvent();
    public event UnityAction OnCageDamage { add => _onCageDamage += value; remove => _onCageDamage -= value; }

    public Cage ChangeTopLeft(Transform topLeft) {
        _topLeft = topLeft;
        return this;
    }

    public Cage ChangeBotRight(Transform botRight) {
        _botRight = botRight;
        return this;
    }

    public Cage ChangePosition(Vector2 position) {
        _position = position;
        return this;
    }

    public Cage ChangeSize(Vector2 size) {
        _size = size;
        return this;
    }

    public Cage ChangeDuration(float duration) {
        _duration = duration;
        return this;
    }

    public Cage ChangeDamages(int damages) {
        _damagesEveryTick = damages;
        return this;
    }

    public Cage ChangeDamagesBonus(int damagesBonus) {
        _damagesBonusEveryTick = damagesBonus;
        return this;
    }

    public Cage ChangeTick(float tick) {
        _tick = tick;
        return this;
    }

    private void Start() {
        transform.position = _position;
        MakeMesh();
        _mask.transform.position = _position;
        _mask.transform.localScale = new Vector3(_size.x, _size.y, 1);
        _spike.transform.position = _position;
        _spike.GetComponent<Animator>().SetTrigger("Up");
        StartCoroutine(Tools.Delay(() => _spike.GetComponent<SpriteRenderer>().size = new Vector2(_botRight.transform.position.x - _topLeft.transform.position.x, _topLeft.transform.position.y - _botRight.transform.position.y), 0.1f));
        StartCoroutine(Tools.Delay(() => Die(), _duration));
        _onCageStart?.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            IHealth found = collision.gameObject.GetRoot().GetComponent<IHealth>();
            if (found == null) {
                return;
            }
            if (!_hits.ContainsKey(found)) {
                _hits.Add(found, Vector2.zero);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        // Debug.Log("Exit : " + collision.gameObject.GetRoot());
        if (collision.CompareTag("Player")) {
            IHealth found = collision.gameObject.GetRoot().GetComponent<IHealth>();
            if (found == null) {
                return;
            }
            if (_hits.ContainsKey(found)) {
                _hits.Remove(found);
            }
        }
    }

    private void Update() {
        if (_hits.Count != 0) {
            foreach (KeyValuePair<IHealth, Vector2> keys in _hits.ToList()) {
                Vector2 time = keys.Value;
                if (keys.Key.IsDead) { keys.Value.Set(keys.Value.x, 0f); continue; }
                time.x += Time.deltaTime;
                if (time.x >= _tick) {
                    time.x -= _tick;
                    if (time.y == 0) {
                        time.y += _damagesEveryTick;
                    } else {
                        time.y += _damagesBonusEveryTick;
                    }
                    keys.Key.TakeDamage((int)time.y, gameObject);
                    _onCageDamage?.Invoke();
                }
                _hits[keys.Key] = time;
            }
        }
    }

    void Die() {
        gameObject.SetActive(false);
        Destroy(gameObject);
    }

    void MakeMesh() {
        _mesh = new Mesh();
        _mesh.vertices = new Vector3[] {
             new Vector3(_topLeft.position.x,_topLeft.position.y,0),
            new Vector3( _botRight.position.x, _topLeft.position.y,0),
            new Vector3( _position.x - _size.x / 2, _position.y + _size.y / 2,0),
            new Vector3( _position.x + _size.x / 2, _position.y + _size.y / 2,0),
            new Vector3( _position.x + _size.x / 2,_position.y - _size.y / 2,0),
            new Vector3(_botRight.position.x,_botRight.position.y,0),
            new Vector3( _position.x - _size.x / 2,_position.y - _size.y / 2,0),
            new Vector3( _topLeft.position.x,_botRight.position.y,0),
        };

        _mesh.triangles = new int[] {
                0,1,2,
                2,1,3,
                3,1,4,
                4,1,5,
                6,4,5,
                7,6,5,
                0,2,6,
                0,6,7,
            };
        _hits = new Dictionary<IHealth, Vector2>();
        _filter = GetComponent<MeshFilter>();
        _filter.mesh = _mesh;
        _beforeDie = new Timer(this, _duration, false);
        _beforeDie.OnActivate += Die;
        Vector2[] points = new Vector2[] {
             new Vector3(_topLeft.position.x,_topLeft.position.y,0),
            new Vector2( _botRight.position.x, _topLeft.position.y),
            new Vector3(_botRight.position.x,_botRight.position.y,0),
            new Vector2( _topLeft.position.x, _botRight.position.y),
            new Vector2( _position.x - _size.x / 2,_position.y - _size.y / 2),
            new Vector2( _position.x + _size.x / 2,_position.y - _size.y / 2),
            new Vector2( _position.x + _size.x / 2, _position.y + _size.y / 2),
            new Vector2( _position.x - _size.x / 2, _position.y + _size.y / 2),
            new Vector2( _position.x - _size.x / 2,_position.y - _size.y / 2),
            new Vector2( _topLeft.position.x, _botRight.position.y),
             new Vector3(_topLeft.position.x,_topLeft.position.y,0),
        };
        _polygonCollider = GetComponent<PolygonCollider2D>();
        _polygonCollider.points = points;
        BoxCollider2D[] boxCollider2D = GetComponentsInChildren<BoxCollider2D>();
        Vector2 bigSize = new Vector2(Mathf.Abs(_topLeft.position.x - _botRight.position.x), Mathf.Abs(_topLeft.position.y - _botRight.position.y));
        boxCollider2D[0].size = new Vector2((bigSize.x - _size.x) / 2, bigSize.y);
        boxCollider2D[0].gameObject.transform.position = new Vector2(-((bigSize.x - _size.x) / 4 + _size.x / 2), 0);
        boxCollider2D[1].size = new Vector2((bigSize.x - _size.x) / 2, bigSize.y);
        boxCollider2D[1].gameObject.transform.position = new Vector2((bigSize.x - _size.x) / 4 + _size.x / 2, 0);
        boxCollider2D[2].size = new Vector2(_size.x, (bigSize.y - _size.y) / 2);
        boxCollider2D[2].gameObject.transform.position = new Vector2(0, (bigSize.y - _size.y) / 4 + _size.y / 2);
        boxCollider2D[3].size = new Vector2(_size.x, (bigSize.y - _size.y) / 2);
        boxCollider2D[3].gameObject.transform.position = new Vector2(0, -((bigSize.y - _size.y) / 4 + _size.y / 2));
    }
}
