using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]

public class Grounder : MonoBehaviour
{
    public event Action<Collider> TouchedGround;
    public event Action<Collider> LeftGround;

    private void OnTriggerEnter(Collider other)
    {
        TouchedGround?.Invoke(other);
    }

    private void OnTriggerExit(Collider other)
    {
        LeftGround?.Invoke(other);
    }
}
