using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPlayerController : PlayerController
{
    protected override void Init()
    {
        base.Init();
    }

    /*void LateUpdate()
    {
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y + 2, _cameraFocus);
    }*/

}
