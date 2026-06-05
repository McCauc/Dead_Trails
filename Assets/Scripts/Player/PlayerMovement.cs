using System.Transactions; 
using System.Collections; 
using System.Collections.Generic; 
using UnityEngine; 
 
public class PlayerMovement : MonoBehaviour 
{ 
    [SerializeField] private float speed = 5f; 
    [SerializeField] private float turnSpeed = 360f; 
    [SerializeField] private SpriteRenderer sprteRnderer; 
    private float playerHalfWidth; 
    private Animator animator;
    
    void Start() 
    { 
        playerHalfWidth = sprteRnderer.bounds.extents.x;
        animator = GetComponent<Animator>();
    } 
    
    void Update() 
    { 
        HandleMovement(); 
    } 
 
    private void HandleMovement() 
    { 
        float inputx = Input.GetAxis("Horizontal"); 
        float inputy = Input.GetAxis("Vertical");

        Vector2 moveInput = new Vector2(inputx, inputy);
        float inputMagnitude = Mathf.Clamp01(moveInput.magnitude);

        if (inputMagnitude > 0f)
        {
            Vector2 inputDir = moveInput.normalized;
            float targetAngle = Mathf.Atan2(inputDir.y, inputDir.x) * Mathf.Rad2Deg;
            float currentAngle = transform.eulerAngles.z;
            float newAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, turnSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0f, 0f, newAngle);
            
            if (animator != null)
            {
                animator.SetBool("IsMoving", true);
            }
        }
        else
        {
            if (animator != null)
            {
                animator.SetBool("IsMoving", false);
            }
        }

        float moveAmount = speed * inputMagnitude * Time.deltaTime;
        Vector2 moveDirection = moveInput.normalized;
        transform.position += (Vector3)(moveDirection * moveAmount); 

        // FIX: Keep player strictly inside the camera screen bounds
        ClampPlayerToScreen();
    }

    private void ClampPlayerToScreen()
    {
        if (Camera.main == null) return;

        // Convert screen corners (bottom-left and top-right) into world coordinates
        Vector3 bottomComponents = Camera.main.ScreenToWorldPoint(new Vector3(0f, 0f, 0f));
        Vector3 topComponents = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0f));

        // Use the player sprite padding bounds to stop overshoot
        float minX = bottomComponents.x + playerHalfWidth;
        float maxX = topComponents.x - playerHalfWidth;
        float minY = bottomComponents.y + playerHalfWidth;
        float maxY = topComponents.y - playerHalfWidth;

        // Force position clamp math calculation changes
        float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);
        float clampedY = Mathf.Clamp(transform.position.y, minY, maxY);

        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }
}