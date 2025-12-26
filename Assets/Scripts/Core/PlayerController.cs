using UnityEngine;
using SyncDash.Managers;
using SyncDash.Networking;

namespace SyncDash.Core
{
    /// <summary>
    /// Controls the player cube movement - auto-forward movement and tap-to-jump
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float forwardSpeed = 5f;
        [SerializeField] private float jumpForce = 8f;
        [SerializeField] private float speedIncreaseRate = 0.1f; // Speed increase per second
        
        [Header("Ground Detection")]
        [SerializeField] private float groundCheckDistance = 0.1f;
        [SerializeField] private LayerMask groundLayer = 1; // Default layer
        
        private Rigidbody rb;
        private bool isGrounded;
        private float currentSpeed;
        private Vector3 lastPosition;
        private Vector3 lastVelocity;
        
        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            currentSpeed = forwardSpeed;
        }
        
        private void Start()
        {
            lastPosition = transform.position;
        }
        
        private void Update()
        {
            // Increase speed over time
            currentSpeed = forwardSpeed + (Time.time * speedIncreaseRate);
            
            // Check for input (tap/click to jump)
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
            
            // Check ground status
            CheckGrounded();
        }
        
        private void FixedUpdate()
        {
            // Auto-forward movement
            Vector3 forwardMovement = Vector3.forward * currentSpeed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + forwardMovement);
            
            // Calculate velocity
            Vector3 velocity = (transform.position - lastPosition) / Time.fixedDeltaTime;
            lastVelocity = velocity;
            
            // Update distance for scoring
            float distanceTraveled = Vector3.Distance(transform.position, lastPosition);
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddDistance(distanceTraveled);
            }
            
            // Send state update for synchronization (simulate network send)
            if (StateSyncManager.Instance != null)
            {
                int score = GameManager.Instance != null ? GameManager.Instance.GetScore() : 0;
                StateSyncManager.Instance.SendPlayerState(
                    transform.position,
                    velocity,
                    !isGrounded,
                    score
                );
            }
            
            lastPosition = transform.position;
        }
        
        private void Jump()
        {
            if (isGrounded)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
        
        private void CheckGrounded()
        {
            // Raycast downward to check if grounded
            isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance + 0.1f, groundLayer);
        }
        
        public float GetCurrentSpeed()
        {
            return currentSpeed;
        }
        
        public bool IsGrounded()
        {
            return isGrounded;
        }
        
        private void OnDrawGizmosSelected()
        {
            // Visualize ground check ray
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Gizmos.DrawRay(transform.position, Vector3.down * (groundCheckDistance + 0.1f));
        }
    }
}

