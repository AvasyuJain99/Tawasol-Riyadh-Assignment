# Quick Setup Guide

## Current Setup Status
✅ Player cube created  
✅ Two cameras created  
✅ Platform created (scale: x=5, z=35)  
⚠️ Camera viewport needs verification  

---

## Step 1: Camera Setup ✅
✅ Cameras already configured in scene!

---

## Step 2: Setup Your Scene

### A. Player Setup
1. Select your **Player cube**
2. Add Component → `Rigidbody`
   - Set **Use Gravity**: ✅ Enabled
   - Set **Is Kinematic**: ❌ Disabled
3. Add Component → `PlayerController` script
4. Position player on the platform (e.g., Y = 0.5 or 1)
5. Make sure the player is on the **right side** of the platform

### B. Ghost Player Setup
1. **Duplicate** your Player cube (Ctrl+D)
2. Rename it to "GhostPlayer"
3. Position it on the **left side** of the platform (mirror position)
4. Add Component → `GhostPlayerController` script
5. Optionally: Change the material color to make it look different (ghost-like)

### C. Camera Setup
✅ Already configured!

### D. Game Manager Setup
1. Create empty GameObject → Name it "GameManager"
2. Add Component → `GameManager` script
3. This handles scoring and game state

### E. State Sync Manager Setup
1. Create empty GameObject → Name it "StateSyncManager"
2. Add Component → `StateSyncManager` script
3. This handles the synchronization between player and ghost

---

## Step 3: Platform Setup

1. Make sure your platform has a **Collider** (Box Collider should be auto-added)
2. Position it at Y = 0 (ground level)
3. Scale: X = 5, Z = 35 (as you have it)

---

## Step 4: Test the Setup

1. **Play the scene**
2. The player should automatically move forward
3. **Click/Tap** or press **Space** to jump
4. The ghost player should mirror the player's movements with a slight delay

---

## Troubleshooting

### Player doesn't move forward:
- Check that `PlayerController` is attached
- Check that Rigidbody is attached and not kinematic
- Check console for errors

### Ghost doesn't sync:
- Make sure `StateSyncManager` exists in scene
- Make sure `GhostPlayerController` is attached to ghost
- Check that both cameras can see their respective players

### Cameras not split-screen:
- Verify viewport rects are set correctly:
  - Left Camera: (0, 0, 0.5, 1)
  - Right Camera: (0.5, 0, 0.5, 1)
- Make sure both cameras are enabled

### Player falls through platform:
- Check platform has a Collider
- Check player's ground check distance in `PlayerController`
- Make sure platform is on correct layer

---

## Next Steps

After basic setup works:
1. Add obstacles (Phase 2.3)
2. Add collectibles (Phase 2.4)
3. Add UI (Phase 4)
4. Add shaders and effects (Phase 5)

