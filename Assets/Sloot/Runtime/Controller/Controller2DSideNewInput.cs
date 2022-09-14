using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Sloot {
    public class Controller2DSideNewInput : Controller {
        PlayerControls playerControls;
        private void Awake() {
            playerControls = new PlayerControls();
            playerControls._2DSide.Enable();
            playerControls._2DSide.Movement.performed += ctx => _direction.x = ctx.ReadValue<Vector2>().x;
            playerControls._2DSide.Movement.performed += ctx => _direction.y = ctx.ReadValue<Vector2>().y;
            playerControls._2DSide.Jump.performed += ctx => _space = ctx.ReadValue<float>() == 1 ? true : false;
            playerControls._2DSide.Jump.performed += ctx => StartCoroutine(FalseAfterFrames(1));
            playerControls._2DSide.Movement.canceled += ctx => _direction.x = 0;
            playerControls._2DSide.Movement.canceled += ctx => _direction.y = 0;
            playerControls._2DSide.Jump.canceled += ctx => _space = false;
        }

        IEnumerator FalseAfterFrames(int frames) {
            for (int i = 0; i < frames; i++) {
                yield return null;
            }
            _space = false;
        }
    }
}