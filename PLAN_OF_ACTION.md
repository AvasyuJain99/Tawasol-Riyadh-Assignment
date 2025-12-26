# Sync Dash - Unity Assignment Plan of Action

## Project Overview
Create a hyper-casual game where the screen is divided into two halves:
- **Right Side**: Player-controlled character
- **Left Side**: Ghost player with real-time synced actions (simulating network multiplayer locally)

---

## Phase 1: Project Setup & Core Architecture

### 1.1 Folder Structure Setup
- [ ] Create folder structure:
  - `Scripts/` - All C# scripts
    - `Core/` - Core gameplay systems
    - `Networking/` - State synchronization system
    - `UI/` - UI controllers
    - `Managers/` - Game managers
    - `Pooling/` - Object pooling system
    - `Effects/` - Visual effects scripts
  - `Shaders/` - Custom shaders
  - `Materials/` - Material assets
  - `Prefabs/` - Prefab assets
    - `Player/`
    - `Obstacles/`
    - `Collectibles/`
    - `Effects/`
  - `Scenes/` - Scene files
  - `Particles/` - Particle system assets
  - `UI/` - UI prefabs and assets

### 1.2 Core Scripts Architecture
- [ ] Create base scripts:
  - `GameManager.cs` - Main game state manager
  - `PlayerController.cs` - Player movement and input
  - `GhostPlayerController.cs` - Synced ghost player
  - `StateSyncManager.cs` - Real-time state synchronization system
  - `ScoreManager.cs` - Score tracking
  - `ObjectPool.cs` - Generic object pooling system
  - `ObstacleSpawner.cs` - Obstacle generation
  - `CollectibleSpawner.cs` - Collectible generation

---

## Phase 2: Core Gameplay Implementation

### 2.1 Player Movement System
- [ ] **PlayerController.cs**
  - [ ] Auto-forward movement (constant speed)
  - [ ] Tap/click detection for jumping
  - [ ] Jump physics (rigidbody or character controller)
  - [ ] Ground detection
  - [ ] Collision detection with obstacles
  - [ ] Collectible collection detection

### 2.2 Screen Division Setup
- [ ] **Single Scene Architecture**: Both player and ghost exist in the SAME scene (GameScene.unity)
- [ ] Create camera setup:
  - [ ] Split screen camera system (two cameras with different viewport rects)
  - [ ] Left camera: Viewport rect (0, 0, 0.5, 1) - Shows ghost player
  - [ ] Right camera: Viewport rect (0.5, 0, 0.5, 1) - Shows player
  - [ ] Both cameras render the same scene/world
  - [ ] Alternative: Single camera with UI overlay divider (less recommended)

### 2.3 Obstacle System
- [ ] **Obstacle.cs**
  - [ ] Obstacle prefab creation
  - [ ] Movement towards player
  - [ ] Collision handling
  - [ ] Dissolve shader integration point

- [ ] **ObstacleSpawner.cs**
  - [ ] Spawn obstacles at intervals
  - [ ] Random positioning
  - [ ] Object pooling integration
  - [ ] Difficulty scaling (spawn rate increases)

### 2.4 Collectible System
- [ ] **Collectible.cs**
  - [ ] Orb prefab creation
  - [ ] Glowing effect setup
  - [ ] Collection detection
  - [ ] Particle effect trigger point

- [ ] **CollectibleSpawner.cs**
  - [ ] Spawn collectibles at intervals
  - [ ] Random positioning
  - [ ] Object pooling integration

### 2.5 Score System
- [ ] **ScoreManager.cs**
  - [ ] Distance-based scoring (continuous)
  - [ ] Collectible-based scoring (on collection)
  - [ ] Score display update
  - [ ] High score persistence (PlayerPrefs)

### 2.6 Game Speed Progression
- [ ] Implement speed increase over time:
  - [ ] Base speed variable
  - [ ] Speed multiplier that increases with time/distance
  - [ ] Apply to player movement, obstacle movement, spawn rates

---

## Phase 3: Real-Time State Synchronization (Network Simulation)

### 3.1 State Data Structure
- [ ] **PlayerState.cs** (Serializable data class)
  - [ ] Position (Vector3)
  - [ ] Rotation (Quaternion)
  - [ ] Velocity (Vector3)
  - [ ] IsJumping (bool)
  - [ ] Timestamp (float)
  - [ ] Score (int)
  - [ ] CollectedOrbs (int)

### 3.2 State Sync Manager
- [ ] **StateSyncManager.cs**
  - [ ] Ring buffer/Queue for state history
  - [ ] Configurable delay (network lag simulation)
  - [ ] State interpolation system
  - [ ] Smooth movement between states
  - [ ] State compression (optional optimization)

### 3.3 Ghost Player Controller
- [ ] **GhostPlayerController.cs**
  - [ ] Receive state updates from StateSyncManager
  - [ ] Apply interpolated movement
  - [ ] Mirror player actions (jump, collect, collide)
  - [ ] Visual distinction (ghost effect, different color)

### 3.4 Integration
- [ ] Connect PlayerController → StateSyncManager → GhostPlayerController
- [ ] Test synchronization accuracy
- [ ] Optimize update frequency
- [ ] Add configurable lag slider (for testing)

---

## Phase 4: UI & Game Flow

### 4.1 Main Menu
- [ ] **MainMenuUI.cs**
  - [ ] "Start" button → Load game scene
  - [ ] "Exit" button → Quit application
  - [ ] Title/Logo display
  - [ ] Background/visuals

### 4.2 In-Game UI
- [ ] **GameplayUI.cs**
  - [ ] Score display (top of screen)
  - [ ] Distance display
  - [ ] Pause button (optional)

### 4.3 Game Over Screen
- [ ] **GameOverUI.cs**
  - [ ] Final score display
  - [ ] High score display
  - [ ] "Restart" button → Reload game scene
  - [ ] "Main Menu" button → Load main menu scene
  - [ ] Crash effect integration point

### 4.4 Scene Management
- [ ] **Scene Architecture**:
  - [ ] **MainMenu.unity** - Main menu scene
  - [ ] **GameScene.unity** - Single gameplay scene containing:
    - Player (right side)
    - Ghost player (left side)
    - Obstacles and collectibles (shared world)
    - Both split-screen cameras
- [ ] **SceneManager.cs** (or use Unity's SceneManager)
  - [ ] Scene transition handling (MainMenu ↔ GameScene)
  - [ ] State preservation (scores, high scores)
  - [ ] Note: Split-screen is handled within GameScene, not separate scenes

---

## Phase 5: Shaders & Visual Effects

### 5.1 Player Glowing Shader
- [ ] **PlayerGlow.shader**
  - [ ] Emission property
  - [ ] Glow intensity parameter
  - [ ] Color tinting
  - [ ] Apply to player cube material

### 5.2 Obstacle Dissolve Shader
- [ ] **Dissolve.shader**
  - [ ] Dissolve effect on hit
  - [ ] Edge glow during dissolve
  - [ ] Noise texture for dissolve pattern
  - [ ] Integration with Obstacle.cs collision

### 5.3 Motion Blur Effect
- [ ] Implement motion blur:
  - [ ] Use Unity's built-in motion blur (Post-processing)
  - [ ] Or custom shader-based solution
  - [ ] Intensity increases with speed

### 5.4 Crash Screen Effects
- [ ] **CrashEffects.cs**
  - [ ] Chromatic aberration (post-processing)
  - [ ] Screen shake (camera shake)
  - [ ] Ripple effect (shader or post-processing)
  - [ ] Trigger on obstacle collision

### 5.5 Particle Effects
- [ ] **OrbCollectionParticle.cs**
  - [ ] Particle burst on orb collection
  - [ ] Glowing particles
  - [ ] Color matching orb color
  - [ ] Object pooling for particles

---

## Phase 6: Performance Optimization

### 6.1 Object Pooling System
- [ ] **ObjectPool.cs** (Generic)
  - [ ] Pool creation and initialization
  - [ ] Get/Return object methods
  - [ ] Pre-warm pools
  - [ ] Pool size management

- [ ] **ObstaclePool.cs**
  - [ ] Pool for obstacles
  - [ ] Integration with ObstacleSpawner

- [ ] **CollectiblePool.cs**
  - [ ] Pool for collectibles
  - [ ] Integration with CollectibleSpawner

- [ ] **ParticlePool.cs**
  - [ ] Pool for particle effects
  - [ ] Integration with effects system

### 6.2 Sync Optimization
- [ ] Optimize StateSyncManager:
  - [ ] Limit state history size
  - [ ] Reduce update frequency if needed
  - [ ] Use fixed timestep for consistency
  - [ ] Profile and optimize hot paths

### 6.3 Rendering Optimization
- [ ] Optimize rendering:
  - [ ] Use simple materials where possible
  - [ ] Limit particle count
  - [ ] Optimize shader complexity
  - [ ] Use LOD if needed (for complex objects)

### 6.4 Build Size Optimization
- [ ] Keep build under 50MB:
  - [ ] Compress textures
  - [ ] Remove unused assets
  - [ ] Optimize audio (if any)
  - [ ] Use efficient asset formats

---

## Phase 7: Testing & Polish

### 7.1 Gameplay Testing
- [ ] Test player movement smoothness
- [ ] Test jump mechanics
- [ ] Test obstacle collision
- [ ] Test collectible collection
- [ ] Test score system accuracy
- [ ] Test speed progression

### 7.2 Synchronization Testing
- [ ] Test state sync accuracy
- [ ] Test with different lag values
- [ ] Test interpolation smoothness
- [ ] Test edge cases (rapid inputs)

### 7.3 Visual Testing
- [ ] Test all shader effects
- [ ] Test particle effects
- [ ] Test crash effects
- [ ] Test motion blur intensity

### 7.4 Performance Testing
- [ ] Profile frame rate
- [ ] Test on target devices (if mobile)
- [ ] Optimize bottlenecks
- [ ] Verify build size

### 7.5 Polish
- [ ] Add sound effects (optional)
- [ ] Add background music (optional)
- [ ] Polish UI animations
- [ ] Add visual feedback for all actions
- [ ] Final balance tuning

---

## Implementation Order (Recommended)

1. **Week 1: Foundation**
   - Phase 1: Project Setup
   - Phase 2: Core Gameplay (Player, Obstacles, Collectibles)
   - Phase 4: Basic UI (Main Menu, Game Over)

2. **Week 2: Synchronization**
   - Phase 3: State Synchronization System
   - Integration and testing

3. **Week 3: Visual Effects**
   - Phase 5: Shaders & Effects
   - Integration with gameplay

4. **Week 4: Optimization & Polish**
   - Phase 6: Performance Optimization
   - Phase 7: Testing & Polish

---

## Technical Notes

### Key Design Decisions:
1. **State Sync**: Use a ring buffer with configurable delay (e.g., 50-200ms) to simulate network lag
2. **Interpolation**: Use Lerp/Slerp for smooth movement between states
3. **Object Pooling**: Pre-warm pools with reasonable sizes (e.g., 20 obstacles, 15 collectibles)
4. **Shaders**: Use Unity Shader Graph or HLSL shaders depending on Unity version
5. **Post-Processing**: Use Unity Post-Processing Stack v2 for screen effects

### Dependencies:
- Unity Post-Processing Stack v2 (for screen effects)
- Unity Input System (optional, can use legacy Input)
- TextMeshPro (for UI text)

---

## Success Criteria Checklist

- [ ] Player cube moves forward automatically
- [ ] Tap to jump works smoothly
- [ ] Obstacles spawn and move correctly
- [ ] Collectibles spawn and can be collected
- [ ] Left side mirrors right side in real-time
- [ ] State sync has configurable lag
- [ ] Smooth interpolation on ghost player
- [ ] Score system works (distance + collectibles)
- [ ] Game speed increases over time
- [ ] Main menu with Start/Exit buttons
- [ ] Game over screen with Restart/Main Menu
- [ ] Score displayed during gameplay
- [ ] Player has glowing shader
- [ ] Obstacles use dissolve shader on hit
- [ ] Orb collection triggers particle burst
- [ ] Crash shows screen distortion effects
- [ ] Object pooling implemented
- [ ] Performance optimized (60 FPS target)
- [ ] Build size under 50MB

---

## Notes
- Focus on clean, maintainable code
- Comment complex logic (especially sync system)
- Use Unity events/delegates for loose coupling
- Keep scripts modular and reusable
- Test frequently during development

