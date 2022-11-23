using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemOneShooter : MonoBehaviour
{
    [field: SerializeField]
    private List<ParticleSystem> ParticleSystemCollection { get; set; }
    [field: SerializeField]
    private float TimeToKillAfterPlay { get; set; }

    public void PlayOneShot ()
    {
        foreach (ParticleSystem particleSystem in ParticleSystemCollection)
        {
            particleSystem.Play();
            Destroy(gameObject, TimeToKillAfterPlay);
        }
    }
}
