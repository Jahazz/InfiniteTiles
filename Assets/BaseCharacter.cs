using UnityEngine;

public class BaseCharacter : MonoBehaviour
{
    [field: Space]
    [field: Header(nameof(BaseCharacter))]
    [field: SerializeField]
    private float GroundDetectorRayLenght { get; set; }

    private const string GROUND_TAG = "Ground";

    protected virtual void FixedUpdate ()
    {
        KeepCharacterOnGround();
    }

    private void KeepCharacterOnGround ()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position + Vector3.up, Vector3.down);

        if (Physics.Raycast(ray, out hit, GroundDetectorRayLenght))
        {
            if (hit.transform.tag == GROUND_TAG)
            {
                transform.position = hit.point;
            }
        }
    }
}
