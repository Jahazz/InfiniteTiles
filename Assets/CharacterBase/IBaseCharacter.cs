using System.Collections.Generic;
using UnityEngine;

public interface IBaseCharacter
{
    public delegate void HitRecievedArguments (IBaseCharacter target, int hitValue, bool isCrit);
    public event HitRecievedArguments OnHitRecieved;

    public delegate void CharacterDeathArguments (IBaseCharacter target);
    public event CharacterDeathArguments OnCharacterDeath;

    public Transform RotationTransform { get; set; }
    public Rigidbody ConnectedRigidbody { get; set; }
    public float CurrentCharacterSpeed { get; set; }
    public List<MonoBehaviour> WeaponsCollection { get; set; }
    public bool IsAlive { get; set; }
}
