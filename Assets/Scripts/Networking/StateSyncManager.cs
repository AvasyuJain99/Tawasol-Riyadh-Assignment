using System.Collections.Generic;
using UnityEngine;

namespace SyncDash.Networking
{
    /// <summary>
    /// Manages real-time state synchronization between player and ghost
    /// Simulates network multiplayer synchronization locally using a ring buffer
    /// </summary>
    public class StateSyncManager : MonoBehaviour
    {
        public static StateSyncManager Instance { get; private set; }
        
        [Header("Sync Settings")]
        [SerializeField] private float networkDelay = 0.1f; // Simulated network lag (100ms)
        [SerializeField] private int bufferSize = 60; // Ring buffer size (1 second at 60fps)
        [SerializeField] private bool useInterpolation = true;
        
        private Queue<PlayerState> stateBuffer = new Queue<PlayerState>();
        private System.Action<PlayerState> onStateReceived;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        /// <summary>
        /// Register callback for when state is received (for ghost player)
        /// </summary>
        public void RegisterStateReceiver(System.Action<PlayerState> callback)
        {
            onStateReceived = callback;
        }
        
        /// <summary>
        /// Send player state (called by PlayerController)
        /// Simulates sending data over network
        /// </summary>
        public void SendPlayerState(Vector3 position, Vector3 velocity, bool isJumping, int score)
        {
            PlayerState state = new PlayerState(
                position,
                Quaternion.identity,
                velocity,
                isJumping,
                Time.time,
                score
            );
            
            // Add to buffer
            stateBuffer.Enqueue(state);
            
            // Limit buffer size
            if (stateBuffer.Count > bufferSize)
            {
                stateBuffer.Dequeue();
            }
            
            // Schedule delayed delivery (simulate network lag)
            StartCoroutine(DelayedStateDelivery(state));
        }
        
        private System.Collections.IEnumerator DelayedStateDelivery(PlayerState state)
        {
            yield return new WaitForSeconds(networkDelay);
            
            // Deliver state to ghost player
            if (onStateReceived != null)
            {
                onStateReceived.Invoke(state);
            }
        }
        
        /// <summary>
        /// Get interpolated state between two states
        /// </summary>
        public PlayerState InterpolateState(PlayerState from, PlayerState to, float t)
        {
            if (!useInterpolation)
            {
                return to;
            }
            
            return new PlayerState(
                Vector3.Lerp(from.position, to.position, t),
                Quaternion.Slerp(from.rotation, to.rotation, t),
                Vector3.Lerp(from.velocity, to.velocity, t),
                to.isJumping, // Use latest jump state
                Mathf.Lerp(from.timestamp, to.timestamp, t),
                to.score
            );
        }
        
        public float GetNetworkDelay()
        {
            return networkDelay;
        }
        
        public void SetNetworkDelay(float delay)
        {
            networkDelay = Mathf.Clamp(delay, 0f, 1f);
        }
    }
}

