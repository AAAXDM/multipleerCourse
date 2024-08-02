using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class InputManager : MonoBehaviour
{
    [Inject] GameManager gameManager;
    InputController input;
    Camera cam;

    bool isX;

    void Awake()
    {
        input = new InputController();
        cam = Camera.main;
        isX = gameManager.IsX;
    }

    void OnEnable()
    {
        input.Enable();
        input.Touch.TouchInput.performed += ctx => StartTouch(ctx);
    }

    void OnDisable()
    {
        input.Touch.TouchInput.performed -= ctx => StartTouch(ctx);
        input.Disable();
    }


    void StartTouch(InputAction.CallbackContext context)
    {
        Vector2 pos = context.ReadValue<Vector2>();
        Ray ray = cam.ScreenPointToRay(pos);
        Debug.Log("touch");

        if (gameManager.IsMyTurn)
        {
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, Vector2.up);
            if (hit.collider != null)
            {
               Cell cell = hit.collider.gameObject.GetComponent<Cell>();
               if(cell != null) 
               {
                    cell.Fill(isX);
               }
            }
        }
    }

}
