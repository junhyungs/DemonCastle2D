using DemonCastle.Combat;
using DemonCastle.Controls;
using DemonCastle.Weapons;
using System;
using UnityEngine;

namespace DemonCastle.Player
{
    public sealed class PlayerContext
    {
        public GameControls.PlayerActions Input { get; } 
        public Rigidbody2D Body { get; }
        public IAimInput Aim { get; }
        public PlayerStats Stats { get; }
        public RollAbility Roll { get; }
        public IWeapon Weapon { get; }
        public Invincibility Invincibility { get; }

        public PlayerContext(
            GameControls.PlayerActions input,
            Rigidbody2D body,
            IAimInput aim,
            PlayerStats stats,
            RollAbility roll,
            IWeapon weapon,
            Invincibility invincibility
            )
        {
            Input = input;
            Body = body != null ? body : throw new ArgumentNullException(nameof(body));
            Aim = aim ?? throw new ArgumentNullException(nameof(aim));
            Stats = stats ?? throw new ArgumentNullException(nameof(stats));
            Roll = roll ?? throw new ArgumentNullException(nameof(roll));
            Weapon = weapon ?? throw new ArgumentNullException(nameof(weapon));
            Invincibility = invincibility ?? throw new ArgumentNullException(nameof(invincibility));
        }
    }
}

