using System.Collections.Generic;
using UnityEngine;

public interface IBaseCharacter
{
    public delegate void OnHitRecievedArguments ();
    public event OnHitRecievedArguments OnHitRecieved;
    public Transform RotationTransform { get; set; }
    public Rigidbody ConnectedRigidbody { get; set; }
    public float CurrentCharacterSpeed { get; set; }
    public List<MonoBehaviour> WeaponsCollection { get; set; }
}
