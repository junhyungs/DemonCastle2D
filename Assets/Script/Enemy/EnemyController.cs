using DemonCastle.Combat;
using UnityEngine;
using DemonCastle.Navigation;
using System;

namespace DemonCastle.Enemy
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private Hurtbox _hurtbox;
        [SerializeField] private ContactHitbox _contactHitbox;
        [SerializeField] private Collider2D _bodyCollider;
        [SerializeField] private HitFlash _hitFlash;
        [SerializeField] private LayerMask _neighborLayers;
        [SerializeField] private EnemyStateId _currentStateForDebug;

        private Rigidbody2D _rigidbody;
        private EnemyBrain _brain;
        private Health _health;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            enabled = false;
        }

        public void Initialize(EnemyDefinition definition, Collider2D target,
            INavigator navigator, EnemyStateId initialState = EnemyStateId.Idle)
        {
            if (_hurtbox == null) throw new InvalidOperationException($"SerializeField 누락 : {nameof(_hurtbox)}");
            if (_contactHitbox == null) throw new InvalidOperationException($"SerializeField 누락 : {nameof(_contactHitbox)}");
            if (_bodyCollider == null) throw new InvalidOperationException($"SerializeField 누락 : {nameof(_bodyCollider)}");
            if (_neighborLayers.value == 0) throw new ArgumentException(nameof(_neighborLayers));
            if (definition == null) throw new ArgumentNullException(nameof(definition));
            if (_hitFlash == null) throw new InvalidOperationException($"SerializeField 누락 : {nameof(_hitFlash)}");

            _health = new Health(definition.MaxHealth);
            _health.Damaged += OnDamaged;
            _health.Died += OnDied;

            var invincibility = new Invincibility(_health, definition.HitInvincibleDuration);
            _hurtbox.Initialize(invincibility);
            _contactHitbox.Initialize(definition.ContactDamage);

            var neighbors = new NeighborSensor(_bodyCollider, _neighborLayers, definition.SeparationRadius);
            var context = new EnemyContext(_rigidbody, _bodyCollider, definition, target, navigator, _rigidbody.position, neighbors);
            _brain = new EnemyBrain(context, invincibility, initialState);
            _hitFlash.Initialize(_health);

            enabled = true;
        }

        private void Update()
        {
            _brain.Tick(Time.deltaTime);
            _currentStateForDebug = _brain.CurrentState;
        }

        private void FixedUpdate()
        {
            _brain.FixedTick(Time.fixedDeltaTime);
        }

        private void OnDied() => Destroy(gameObject);
        private void OnDamaged(DamageInfo hit) => _brain.ReceiveHit(hit);

        //TODO 개발용 테스트 코드 이후 삭제필요.
        [ContextMenu("디버그/피해 1")]
        private void DebugTakeDamage() => _health?.TakeDamage(new DamageInfo(1));
    }
}

