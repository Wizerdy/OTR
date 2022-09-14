using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller2DSideOldInput : Controller {
    void Update() {
        _direction.x = Input.GetAxisRaw("Horizontal");
        _direction.y = Input.GetAxisRaw("Vertical");
        _space = Input.GetKey("space");
    }
}
