using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using Obi;


public class FluidFlowControl : MonoBehaviour
{
    [SerializeField] private ObiEmitter emitter;
    [SerializeField] private CircularDrive drive;
    [SerializeField] private float FluidFlowSpeed = 1.7f;

    private float num = 0.0f;

    private void Awake()
    {
        var emitter = GetComponent<ObiEmitter>();
        var drive = GetComponent<CircularDrive>();
    }
    
    public void turnOnFluid()
    {
        
        emitter.speed = 1.7f;
    }

    public void turnOffFluid()
    {
        emitter.speed = 0.0f;
    }

    private void FixedUpdate()
    {
        if (drive != null && num != map(drive.outAngle, 0.0f, 45.0f, 0.0f, FluidFlowSpeed))
        {
            num = map(drive.outAngle, 0.0f, 45.0f, 0.0f, FluidFlowSpeed);
            emitter.speed = num;
        }
    }
    private float map(float from, float fromMin, float fromMax, float toMin, float toMax)
    {
        var fromAbs = from - fromMin;
        var fromMaxAbs = fromMax - fromMin;

        var normal = fromAbs / fromMaxAbs;

        var toMaxAbs = toMax - toMin;
        var toAbs = toMaxAbs * normal;

        var to = toAbs + toMin;

        return to;
    }
}
