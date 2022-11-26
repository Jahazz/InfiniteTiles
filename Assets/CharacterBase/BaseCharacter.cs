using AYellowpaper;
using InfiniteTiles.Weapon;
using System.Collections.Generic;
using UnityEngine;
using static IBaseCharacter;

namespace InfiniteTiles.Character
{
    public class BaseCharacter<BaseCharacterStatsType, BaseCharacterDataType> : MonoBehaviour, IDamageable, IBaseCharacter
        where BaseCharacterDataType : BaseCharacterData
        where BaseCharacterStatsType : BaseCharacterStats<BaseCharacterDataType>, new()
    {
        public event HitRecievedArguments OnHitRecieved;
        public event CharacterDeathArguments OnCharacterDeath;

        [field: Space]
        [field: Header(nameof(BaseCharacter<BaseCharacterStatsType, BaseCharacterDataType>))]
        [field: SerializeField]
        public Rigidbody ConnectedRigidbody { get; set; }
        [field: SerializeField]
        public Transform RotationTransform { get; set; }
        [RequireInterface(typeof(IBaseWeapon))]
        public List<MonoBehaviour> weaponsCollection; //HACK has to use variable instead of property for package to work. Kept the uppercase for name consistency
        [field: SerializeField]
        private float GroundDetectorRayLenght { get; set; }
        [field: SerializeField]
        private bool IsMovingByTranslation { get; set; }

        public BaseCharacterStatsType CharacterStats { get; private set; }
        private bool IsAlive { get; set; } = true;
        [field: SerializeField]
        private BaseCharacterDataType CharacterDataScriptableObject { get; set; }
        public List<MonoBehaviour> WeaponsCollection { get => weaponsCollection; set => weaponsCollection = value; }
        public float CurrentCharacterSpeed { get; set; }

        private int GroundLayerMask { get; set; }
        private const string GROUND_LAYER_NAME = "Ground";

        public void Initialize ()
        {
            GroundLayerMask = LayerMask.NameToLayer(GROUND_LAYER_NAME);
            CharacterStats = new BaseCharacterStatsType();
            CharacterStats.InitializeBaseData(CharacterDataScriptableObject);
            AttachToStatsEvents();
            InitializeWeapons();
        }

        public Transform GetTargetTransform ()
        {
            return transform;
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
            OnCharacterDeath?.Invoke();
            Debug.Log("Died");
        }

        public void GetDamaged (int damageValue)
        {
            CharacterStats.Health.CurrentValue.PresentValue -= damageValue;
            OnHitRecieved?.Invoke(this, damageValue, false);
        }

        private void OnHealthChange (int value)
        {
            CheckIfIsDead();
        }

        private void KeepCharacterOnGround ()
        {
            RaycastHit hit;
            Ray ray = new Ray(transform.position + Vector3.up, Vector3.down);

            if (Physics.Raycast(ray, out hit, GroundDetectorRayLenght, 1 << GroundLayerMask))
            {
                transform.position = new Vector3(transform.position.x,hit.point.y, transform.position.z);
            }
        }

        private void UpdateCharacterSpeed ()
        {
            ConnectedRigidbody.AddForce((CurrentCharacterSpeed * transform.forward) - ConnectedRigidbody.velocity, ForceMode.VelocityChange);
        }
    }
}

