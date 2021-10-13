using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class Input : MonoBehaviour
{
    [HideInInspector] public Vector2 moveVector;
    [HideInInspector] public bool dash;
    [HideInInspector] public bool interact;
    [HideInInspector] public bool grapple;
    //[HideInInspector] public bool quit;
    [HideInInspector] public bool restart;
    [HideInInspector] public bool detatch;
    

    private void Update()
    {
        moveVector.x = (Keyboard.current.aKey.isPressed ? -1f : 0f) + (Keyboard.current.dKey.isPressed ? 1f : 0f);
        moveVector.y = (Keyboard.current.sKey.isPressed ? -1f : 0f) + (Keyboard.current.wKey.isPressed ? 1f : 0f);
        
        if (moveVector.magnitude > 1)
        {
            moveVector = moveVector.normalized;
        }
        
        dash = Keyboard.current.leftShiftKey.wasPressedThisFrame;
        interact = Keyboard.current.fKey.wasPressedThisFrame;
        grapple = Mouse.current.leftButton.wasPressedThisFrame;
        detatch = Keyboard.current.spaceKey.wasPressedThisFrame;
        
        
        restart = Keyboard.current.rKey.wasPressedThisFrame;
    }
}