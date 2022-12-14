using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine.BetterEvents;
using System;

public class CallEventBossCharge : MonoBehaviour {
    [SerializeField] BetterEvent<Vector2> _onWallHit = new BetterEvent<Vector2>();

    void Start() {
        BossCharge.OnWallHit += _OnWallHitInvoke;
    }

    private void _OnWallHitInvoke(Vector2 position) {
        _onWallHit.Invoke(position);
    }
}
