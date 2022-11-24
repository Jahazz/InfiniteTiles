using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSmoothFollow : MonoBehaviour
{
    [field: SerializeField]
    private Transform TransformToFollow { get; set; }
    [field: SerializeField]
    private float Speed { get; set; }
    private Vector3 InitialOffset { get; set; }

    protected virtual void Start ()
    {
        InitialOffset = transform.position - TransformToFollow.position;
    }

    private void LateUpdate ()
    {
        SmoothFollow();
    }

    public void SmoothFollow ()
    {
        transform.position = Vector3.Lerp(transform.position, TransformToFollow.position + InitialOffset, Speed);
    }
}
