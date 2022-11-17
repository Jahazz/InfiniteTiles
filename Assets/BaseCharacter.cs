using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacter : MonoBehaviour
{
    private const string GROUND_TAG = "Ground";

    protected virtual void FixedUpdate ()
    {
        KeepCharacterOnGround();
    }

    private void KeepCharacterOnGround ()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position + Vector3.up, Vector3.down);

        if (Physics.Raycast(ray, out hit, 1000))
        {
            if (hit.transform.tag == GROUND_TAG)
            {
                transform.position = hit.point;
            }
        }
    }
}
