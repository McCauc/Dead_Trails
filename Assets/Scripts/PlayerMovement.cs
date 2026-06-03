using System.Transactions; 
using System.Collections; 
using System.Collections.Generic; 
using UnityEngine; 
 
public class PlayerMovement : MonoBehaviour 
{ 
 
    [SerializeField] private float speed = 5f; 
    [SerializeField] private float turnSpeed = 360f; 
    [SerializeField] private SpriteRenderer sprteRnderer; 
    // [SerializeField] private Animator animator; 
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
            // Sprite faces right, no offset needed.
            transform.rotation = Quaternion.Euler(0f, 0f, newAngle);
            
            // Play walk animation
            if (animator != null)
            {
                animator.SetBool("IsMoving", true);
            }
        }
        else
        {
            // Stop walk animation
            if (animator != null)
            {
                animator.SetBool("IsMoving", false);
            }
        }

        float moveAmount = speed * inputMagnitude * Time.deltaTime;
        Vector2 moveDirection = moveInput.normalized;
        transform.position += (Vector3)(moveDirection * moveAmount); 
    }
}