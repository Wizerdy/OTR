using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine;
using System.Linq;

public class BossPhase : MonoBehaviour {
    [SerializeField] List<BossAttack> _attacks;
    [SerializeField] bool _isDebugging;
    public BossAttack GetAnAttack() {
        return _attacks[Tools.Ponder(_attacks.Select(x => x.Weight).ToArray())];
    }

    private void Start() {
        for (int i = 0; i < transform.childCount; i++) {
            if (!_attacks.Contains(transform.GetChild(i).GetComponent<BossAttack>())){
                _attacks.Add(transform.GetChild(i).GetComponent<BossAttack>());
            }
        }
    }
    public BossAttack Get(string attackName) {
        for (int i = 0; i < _attacks.Count; i++) {
            if (_attacks[i].GetType().Name == attackName) {
                return _attacks[i];
            }
        }
        if (_isDebugging)
            Debug.LogError("string not found");
        return GetAnAttack();
    }
}
