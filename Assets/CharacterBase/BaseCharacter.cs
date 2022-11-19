using AYellowpaper;
using InfiniteTiles.Weapon;
using System.Collections.Generic;
using UnityEngine;

namespace InfiniteTiles.Character
{
    public class BaseCharacter<BaseCharacterStatsType, BaseCharacterDataType> : MonoBehaviour, IDamageable, IBaseCharacter
        where BaseCharacterDataType : BaseCharacterData
        where BaseCharacterStatsType : BaseCharacterStats<BaseCharacterDataType>, new()
    {
        [field: Space]
        [field: Header(nameof(BaseCharacter<BaseCharacterStatsType, BaseCharacterDataType>))]
        [field: SerializeField]
        public Rigidbody ConnectedRigidbody { get; set; }
        [field: SerializeField]
        public Transform RotationTransform { get; set; }
        [RequireInterface(typeof(ITargetable))]
        public List<MonoBehaviour> weaponsCollection; //HACK has to use variable instead of property for package to work. Kept the uppercase for name consistency
        [field: SerializeField]
        private float GroundDetectorRayLenght { get; set; }

        public BaseCharacterStatsType CharacterStats { get; private set; }
        private bool IsAlive { get; set; } = true;
        [field: SerializeField]
        private BaseCharacterDataType CharacterDataScriptableObject { get; set; }
        public List<MonoBehaviour> WeaponsCollection { get => weaponsCollection; set => weaponsCollection = value; }
        public float CurrentCharacterSpeed { get; set; }

        private const string GROUND_TAG = "Ground";

        public void Initialize ()
        {
            CharacterStats = new BaseCharacterStatsType();
            CharacterStats.InitializeBaseData(CharacterDataScriptableObject);
            AttachToStatsEvents();
            InitializeWeapons();
        }

        public Transform GetTargetTransform ()
        {
            return transform;
        }

        public float GetCurrentCharacterSpeed ()
        {
            return ConnectedRigidbody.velocity.magnitude;
        }

        protected virtual void OnDestroy ()
        {
            DetachFromStatsEvents();
        }

        protected virtual void Start ()
        {
            Initialize();
        }

        protected virtual void Update ()
        {
            UpdateCharacterSpeed();
        }

        protected virtual void LateUpdate ()
        {
            KeepCharacterOnGround();
        }

        protected virtual void InitializeWeapons ()
        {

        }

        protected virtual void AttachToStatsEvents ()
        {
            CharacterStats.Health.CurrentValue.OnVariableChange += OnHealthChange;
        }

        protected virtual void DetachFromStatsEvents ()
        {
            CharacterStats.Health.CurrentValue.OnVariableChange -= OnHealthChange;
        }

        protected void CheckIfIsDead ()
        {
            if (CharacterStats.Health.CurrentValue.PresentValue <= 0 && IsAlive == true)
            {
                Die();
            }
        }

        protected virtual void Die ()
        {
            IsAlive = false;
            Debug.Log("Died");
        }

        public void GetDamaged (int damageValue)
        {
            CharacterStats.Health.CurrentValue.PresentValue -= damageValue;
            Debug.Log("GetDamaged for "+damageValue);
        }

        private void OnHealthChange (int value)
        {
            CheckIfIsDead();
        }

        private void KeepCharacterOnGround ()
        {
            RaycastHit hit;
            Ray ray = new Ray(transform.position + Vector3.up, Vector3.down);
            Debug.DrawRay(transform.position + Vector3.up, Vector3.down * GroundDetectorRayLenght, Color.green);
            if (Physics.Raycast(ray, out hit, GroundDetectorRayLenght))
            {
                if (hit.transform.tag == GROUND_TAG)
                {
                    transform.position = hit.point;
                    Debug.Log("GroundHit" + CharacterStats.CharacterName.PresentValue);
                }
            }
        }

        private void UpdateCharacterSpeed ()
        {
            ConnectedRigidbody.AddForce((CurrentCharacterSpeed * RotationTransform.forward) - ConnectedRigidbody.velocity, ForceMode.VelocityChange);
        }
    }
}

