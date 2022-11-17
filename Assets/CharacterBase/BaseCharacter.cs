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

        public BaseCharacterStatsType CharacterStats { get; private set; }
        private bool IsAlive { get; set; } = true;

        private const string GROUND_TAG = "Ground";

        public void Initialize (BaseCharacterDataType characterData)
        {
            CharacterStats = new BaseCharacterStatsType();
            CharacterStats.InitializeBaseData(characterData);
            AttachToStatsEvents();
        }

        public float CalculateRangeBetweenTarget (Vector3 attacker)
        {
            return Vector3.Distance(transform.position, attacker);
        }

        protected virtual void FixedUpdate ()
        {
            KeepCharacterOnGround();
        }

        protected virtual void OnDestroy ()
        {
            DetachFromStatsEvents();
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
        }

        public void GetDamaged (int damageValue)
        {
            CharacterStats.Health.CurrentValue.PresentValue -= damageValue;
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

