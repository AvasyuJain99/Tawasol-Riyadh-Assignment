using UnityEngine;
using SyncDash.Networking;

namespace SyncDash.Core
{
    /// <summary>
    /// Controls the ghost player on the left side - mirrors player actions with network lag simulation
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class GhostPlayerController : MonoBehaviour
    {
        [Header("Visual Settings")]
        [SerializeField] private Color ghostColor = new Color(1f, 1f, 1f, 0.7f);
        [SerializeField] private Material ghostMaterial;
        
        [Header("Interpolation Settings")]
        [SerializeField] private float interpolationSpeed = 10f;
        
        private Rigidbody rb;
        private Renderer renderer;
        private PlayerState targetState;
        private PlayerState currentState;
        private bool hasReceivedState = false;
        
        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            renderer = GetComponent<Renderer>();
            
            // Set ghost material
            if (ghostMaterial != null)
            {
                renderer.material = ghostMaterial;
            }
            else
            {
                // Create ghost material if not assigned
                Material mat = new Material(Shader.Find("Standard"));
                mat.color = ghostColor;
                renderer.material = mat;
            }
            
            // Make ghost kinematic or use different physics
            rb.isKinematic = false;
            rb.useGravity = true;
        }
        
        private void Start()
        {
            // Register with StateSyncManager to receive state updates
            if (StateSyncManager.Instance != null)
            {
                StateSyncManager.Instance.RegisterStateReceiver(OnStateReceived);
            }
            else
            {
                Debug.LogError("GhostPlayerController: StateSyncManager not found!");
            }
        }
        
        private void FixedUpdate()
        {
            if (hasReceivedState && targetState != null)
            {
                // Interpolate towards target state
                if (useInterpolation)
                {
                    InterpolateToState();
                }
                else
                {
                    ApplyStateDirectly();
                }
            }
        }
        
        private void OnStateReceived(PlayerState state)
        {
            if (!hasReceivedState)
            {
                // First state - set directly
                currentState = state.Clone();
                ApplyStateDirectly();
                hasReceivedState = true;
            }
            
            targetState = state;
        }
        
        private void InterpolateToState()
        {
            if (currentState == null || targetState == null) return;
            
            // Interpolate position
            Vector3 targetPos = targetState.position;
            Vector3 currentPos = rb.position;
            
            // Use interpolation speed for smooth movement
            Vector3 newPos = Vector3.Lerp(currentPos, targetPos, interpolationSpeed * Time.fixedDeltaTime);
            rb.MovePosition(newPos);
            
            // Update rotation
            rb.MoveRotation(targetState.rotation);
            
            // Apply velocity for jump simulation
            if (targetState.isJumping && !currentState.isJumping)
            {
                // Simulate jump
                rb.velocity = new Vector3(rb.velocity.x, targetState.velocity.y, rb.velocity.z);
            }
            
            // Update current state
            currentState = targetState.Clone();
        }
        
        private void ApplyStateDirectly()
        {
            if (targetState == null) return;
            
            rb.MovePosition(targetState.position);
            rb.MoveRotation(targetState.rotation);
            
            if (targetState.isJumping)
            {
                rb.velocity = new Vector3(rb.velocity.x, targetState.velocity.y, rb.velocity.z);
            }
            
            currentState = targetState.Clone();
        }
        
        private bool useInterpolation => StateSyncManager.Instance != null && 
                                         StateSyncManager.Instance.GetNetworkDelay() > 0;
    }
}

