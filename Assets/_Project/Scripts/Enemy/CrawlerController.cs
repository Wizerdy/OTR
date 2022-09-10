using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ToolsBoxEngine;
using System.Linq;

public class CrawlerController : MonoBehaviour {
    [SerializeField] CrawlerEntity _crawlerEntity;
    [SerializeField] EntityCollisionArea _detectionArea;
    [SerializeField] EntityCollisionArea _attackArea;
    [SerializeField] string _targetTag = "Player";

    void Start() {
        _detectionArea.OnAreaEnter += _ChaseNearestPlayer;
        _detectionArea.OnAreaExit += _StopNearestChase;
    }

    private void _ChaseNearestPlayer(GameObject obj) {
        if (obj.CompareTag(_targetTag)) {
            GameObject nearest = _detectionArea.Nearest(pair => pair.Key.CompareTag(_targetTag));
            _crawlerEntity.Chase(nearest.transform);
        }
    }

    private void _StopNearestChase(GameObject obj) {
        if (obj.CompareTag(_targetTag)) {
            GameObject nearest = _detectionArea.Nearest(pair => pair.Key.CompareTag(_targetTag));
            if (nearest != null) { _crawlerEntity.Chase(nearest.transform); }
            else { _crawlerEntity.Partol(); }
        }
    }
}
