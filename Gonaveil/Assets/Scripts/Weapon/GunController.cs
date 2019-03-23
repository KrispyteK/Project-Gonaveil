﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{

    public enum TriggerStates { Idle, Primary, Secondary }
    public TriggerStates triggerState;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(triggerState == TriggerStates.Idle)
        {
            if (InputManager.GetButton("Fire1"))
            {
                triggerState = TriggerStates.Primary;
            }
            if (InputManager.GetButton("Fire2"))
            {
                triggerState = TriggerStates.Secondary;
            }
        }else if(triggerState == TriggerStates.Primary)
        {
            if (InputManager.GetButtonUp("Fire1"))
            {
                triggerState = TriggerStates.Idle;
            }
        }
        else if(triggerState == TriggerStates.Secondary)
        {
            if (InputManager.GetButtonUp("Fire2"))
            {
                triggerState = TriggerStates.Idle;
            }
        }
    }
}
