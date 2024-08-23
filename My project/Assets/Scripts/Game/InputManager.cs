using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class InputManager : MonoBehaviour
{
    [Inject] GameManager gameManager;
    InputController input;
    Camera cam;


    void Awake()
    {
        input = new InputController();
        cam = Camera.main;
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

        if (gameManager.IsMyTurn && gameManager.CanPlay)
        {
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, Vector2.up);
            if (hit.collider != null)
            {
               SceneCell cell = hit.collider.gameObject.GetComponent<SceneCell>();
               if(cell != null) 
               {
                    if (!cell.IsFilled)
                    {
                        cell.SendMessageToServer();
                    }
               }
            }
        }
    }

}
