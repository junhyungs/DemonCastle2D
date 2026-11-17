using UnityEngine;

namespace DemonCastle.Navigation
{
    public interface INavigator
    {
        Vector2 GetDirection(Vector2 from, Vector2 to);
    }
}

