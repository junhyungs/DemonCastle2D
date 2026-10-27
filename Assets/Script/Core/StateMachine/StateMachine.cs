using System;
using System.Collections.Generic;

namespace DemonCastle.Core
{
    public class StateMachine<TStateId> where TStateId : struct, Enum
    {
        private readonly Dictionary<TStateId, IState> _states = new Dictionary<TStateId, IState>();
        private IState _current;
        public TStateId CurrentId { get; private set; }
        public bool HasState => _current != null;

        public void Register(TStateId id, IState state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));
            if(_states.ContainsKey(id)) throw new InvalidOperationException($"'{id}' 상태가 이미 등록되어 있습니다.");
            _states.Add(id, state);
        }

        public void RegisterAll(Func<TStateId, IState> create)
        {
            if(create == null) throw new ArgumentNullException(nameof(create));
            foreach(var id in (TStateId[])Enum.GetValues(typeof(TStateId)))
            {
                IState state = create(id);
                if(state == null) throw new InvalidOperationException($"'{id}' 상태를 만들지 못했습니다.");
                Register(id, state);
            }
        }

        public void ChangeState(TStateId id)
        {
            if(!_states.TryGetValue(id, out var next)) throw new InvalidOperationException($"'{id}' 상태가 등록되어 있지 않습니다.");
            _current?.Exit();
            _current = next;
            CurrentId = id;
            _current.Enter();
        }

        public void Tick(float deltaTime)
        {
            _current?.Tick(deltaTime);
        }

        public void FixedTick(float fixedDeltaTime)
        {
            _current?.FixedTick(fixedDeltaTime);
        }
    }
}


