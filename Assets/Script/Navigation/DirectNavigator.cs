using UnityEngine;

namespace DemonCastle.Navigation
{
    public class DirectNavigator : INavigator
    {
        public Vector2 GetDirection(Vector2 from, Vector2 to) => (to - from).normalized;
    }
}

