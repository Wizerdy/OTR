using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine;
using System.Linq;

public class BossPhase : MonoBehaviour {
    [SerializeField] List<BossAttack> attacks;
    public BossAttack GetAnAttack() {
        return attacks[Tools.Ponder(attacks.Select(x => x.Weight).ToArray())];
    }
}
