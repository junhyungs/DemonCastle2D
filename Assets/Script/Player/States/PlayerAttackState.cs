using DemonCastle.Weapons;
using DemonCastle.Core;

namespace DemonCastle.Player
{
    public sealed class PlayerAttackState : PlayerGroundedState
    {
        private IWeapon Weapon => Context.Weapon;

        public PlayerAttackState(
            StateMachine<PlayerStateId> machine,
            PlayerContext context
            ) : base(machine, context) { }

        public override void Enter()
        {
            Context.Facing.Lock(Weapon.AttackDirection);
        }

        public override void Tick(float deltaTime)
        {
            if (TryStartRoll()) return;

            // 상태는 바꾸지 않고 무기에만 전달한다 → 다음 타 예약, 또는 창이 열렸으면 즉시 연결.
            if (WasAttackPressed()) Weapon.TryAttack(AttackDirection());

            if (!Weapon.IsAttacking)
            {
                ReturnToGround();
                return;
            }

            // 다음 타로 넘어가며 방향이 바뀌었을 수 있으니 매 프레임 무기 방향으로 맞춰 잠근다.
            Context.Facing.Lock(Weapon.AttackDirection);
        }

        public override void FixedTick(float fixedDeltaTime)
        {
            var speed = Context.Stats.MoveSpeed * Weapon.MoveMultiplier;
            Body.linearVelocity = PlayerMovement.CalculateVelocity(ReadMove(), speed);
        }

        public override void Exit()
        {
            Weapon.Cancel();
            Context.Facing.Unlock();
        }
    }
}

