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
        BossAttack[] bossAttacks = GetComponentsInChildren<BossAttack>();
        for (int i = 0; i < bossAttacks.Length; i++) {
            if (null != bossAttacks[i] && !_attacks.Contains(bossAttacks[i])){
                _attacks.Add(bossAttacks[i]);
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
