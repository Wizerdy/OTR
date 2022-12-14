using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine.BetterEvents;
using UnityEngine.Events;

public class ComingAnimation : MonoBehaviour {
    [SerializeField] BetterEvent _animationComingEnd = new BetterEvent();
    public event UnityAction AnimationComingEnd { add => _animationComingEnd += value; remove => _animationComingEnd -= value; }

    public void ComingAnimationEnd() {
        _animationComingEnd?.Invoke();
    }
}
