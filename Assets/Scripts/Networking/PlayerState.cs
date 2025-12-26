using UnityEngine;

namespace SyncDash.Networking
{
    /// <summary>
    /// Serializable state data structure for player synchronization
    /// Represents the data that would be sent over network in real multiplayer
    /// </summary>
    [System.Serializable]
    public class PlayerState
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 velocity;
        public bool isJumping;
        public float timestamp;
        public int score;
        
        public PlayerState(Vector3 pos, Quaternion rot, Vector3 vel, bool jumping, float time, int scr)
        {
            position = pos;
            rotation = rot;
            velocity = vel;
            isJumping = jumping;
            timestamp = time;
            score = scr;
        }
        
        public PlayerState Clone()
        {
            return new PlayerState(position, rotation, velocity, isJumping, timestamp, score);
        }
    }
}

