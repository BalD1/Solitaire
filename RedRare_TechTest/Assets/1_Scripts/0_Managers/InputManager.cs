using BalDUtilities.MouseUtils;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputManager : EventHandlerMono
{
    private Camera mainCam;

    [SerializeField] private LayerMask clickableLayerMask;

    protected override void EventRegister()
    {
    }

    protected override void EventUnRegister()
    {
    }

    protected override void Start()
    {
        base.Start();

        mainCam = Camera.main;
    }

    private void OnPause(InputValue value)
    {
        if (value.isPressed) this.Pause();
    }

    private void OnClick(InputValue value)
    {
        // get raw and world mouse positions
        Vector3 rawMousePos = Input.mousePosition;
        Vector3 worldMousePos = mainCam.ScreenToWorldPoint(rawMousePos);

        rawMousePos.z = Mathf.Infinity;
        worldMousePos.z = 0;

        // call a raycast from mouse
        RaycastHit2D hit = Physics2D.Raycast(worldMousePos, Vector2.zero, clickableLayerMask);

        // check if clicked on a clickable
        IClickable clickable = hit ?
                               hit.transform.GetComponent<IClickable>() :
                               null;

        // mouse down
        if (value.isPressed)
        {
            this.MouseDown(worldMousePos);
            if (clickable != null) this.ClickableDown(clickable);
        }
        // mouse up
        else
        {
            this.MouseUp(worldMousePos);
            if (clickable != null) this.ClickableUp(clickable);
        }
    }
}

