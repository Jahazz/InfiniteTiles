using AYellowpaper;
using InfiniteTiles.Weapon;
using System.Collections.Generic;
using UnityEngine;

namespace InfiniteTiles.Character
{
    public class BaseCharacter<BaseCharacterStatsType, BaseCharacterDataType> : MonoBehaviour, IDamageable
        where BaseCharacterDataType : BaseCharacterData
        where BaseCharacterStatsType : BaseCharacterStats<BaseCharacterDataType>, new()
    {
        [field: Space]
        [field: Header(nameof(BaseCharacter<BaseCharacterStatsType, BaseCharacterDataType>))]
        [field: SerializeField]
        private float GroundDetectorRayLenght { get; set; }
        [field: SerializeField]
        private BaseCharacterDataType CharacterDataScriptableObject { get; set; }
        [RequireInterface(typeof(ITargetable))]
        public List<MonoBehaviour> WeaponsCollection; //HACK has to use variable instead of property for package to work. Kept the uppercase for name consistency

        public BaseCharacterStatsType CharacterStats { get; private set; }
        private bool IsAlive { get; set; } = true;

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

        protected virtual void FixedUpdate ()
        {
            KeepCharacterOnGround();
        }

        protected virtual void OnDestroy ()
        {
            DetachFromStatsEvents();
        }

        protected virtual void Start ()
        {
            Initialize();
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

            if (Physics.Raycast(ray, out hit, GroundDetectorRayLenght))
            {
                if (hit.transform.tag == GROUND_TAG)
                {
                    transform.position = hit.point;
                }
            }
        }
    }
}

