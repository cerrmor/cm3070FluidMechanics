using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Obi;


public class FluidFlowControl : MonoBehaviour
{
    public ObiEmitter emitter;

    private void Awake()
    {
        var emitter = GetComponent<ObiEmitter>();
    }
    
    public void turnOnFluid()
    {
        
        emitter.speed = 1.7f;
    }

    public void turnOffFluid()
    {
        emitter.speed = 0.0f;
    }
}
