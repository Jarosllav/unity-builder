using UnityEngine;

namespace nobodyworks.builder.input
{
    public interface IInputProvider
    {
        Vector2 GetMove();
        Vector2 GetLook();
    }
}