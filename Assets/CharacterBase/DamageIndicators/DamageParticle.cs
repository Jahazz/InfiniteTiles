using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageParticle : MonoBehaviour
{
    [field: SerializeField]
    private TMP_Text BoundText { get; set; }

    private float EndTime { get; set; }

    private const float TIME_TO_DISPOSE = 1.0f;
    private const float SCALE_MULTIPLIER = 0.95f;
    private const float TRANSLATION_MULTIPLAYER = 0.1f;

    public void Initialize (int damageValue, bool isCrit)
    {
        string text = damageValue.ToString();

        if (isCrit == true)
        {
            BoundText.fontStyle = FontStyles.Bold;
            text += "!";
        }

        BoundText.text = text;

        EndTime = Time.time + TIME_TO_DISPOSE;
    }

    public void Update ()
    {
        transform.Translate(Vector3.up* TRANSLATION_MULTIPLAYER);
        transform.LookAt(Camera.main.transform, Vector3.up);
        transform.Rotate(0, 180, 0);
        transform.localScale *= SCALE_MULTIPLIER;

        if (EndTime < Time.time )
        {
            Destroy(gameObject);
        }
    }
}
