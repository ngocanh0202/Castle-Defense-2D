## Why

The current codebase was designed for an action RPG-style game where the player directly controls a character that moves, shoots, and levels up. However, this project is a tower defense game where the player does not control any units directly. Instead, the player manages towers (building/placing them), spawns units manually, and controls the camera via touch/drag. The existing player controller components are unnecessary and create confusion.

## What Changes

### Remove Unnecessary Components
- Delete `PlayerController.cs` - handles player movement, shooting, weapon rotation (not needed)
- Delete `PlayerReceiveDamageBehaviour.cs` - player character damage system (not needed)
- Remove player-related event subscriptions from `InputManager.cs` (OnMove, OnStop, OnAttack, OnSetBulletStrategy, OnReloadAmmo, OnOpenInventory, OnSetWeaponRotation)

### Rename for Clarity
- Rename `TowerReceiveDamageBehaviour.cs` to `MainTowerBehaviour.cs` to clarify it represents the player's main base tower

### Modify Existing Components
- `InputManager.cs`: Keep only tower interaction events (OnTowerTap, OnTowerDrag, OnToggleBuildMode)
- `EnemyReceiveDamageBehaviour.cs`: Remove reference to `PlayerController.Instance.IncreaseEXP` (causes compile error)

### Create New Tower Types
- **UnitTowerBehaviour**: Player-built tower that spawns units on demand when player taps a "Spawn Unit" button
- **EnemyTowerBehaviour**: Enemy tower that automatically spawns enemies on a timer (wave-based)
- **MainEnemyTowerBehaviour**: Enemy base tower - game over (WIN) when destroyed

### Create Unit Combat System
- **UnitCombatBehaviour**: Attached to both player and enemy units. Finds nearest valid target (unit or tower), moves into attack range, and deals damage

### Create Wave Management
- **WaveManager**: Controls enemy spawn waves, timing, difficulty scaling

### Create UI Components
- **BuildingModeUI**: Toggle button to enable/disable building mode
- **UnitSpawnButtonUI**: "Spawn Unit" button that appears when a Unit Tower is selected

### Game Flow Changes
- Main Tower destroyed → GAME OVER
- Main Enemy Tower destroyed → WIN
- Enemy units path toward Main Tower
- Player units defend against enemy units

## Capabilities

### New Capabilities
- `tower-building`: Player can place towers on grid in building mode
- `unit-spawning`: Player can manually spawn units from Unit Towers
- `unit-combat`: Units automatically find and attack nearest enemies
- `wave-spawning`: Enemy towers spawn enemies automatically on timers
- `camera-control`: Touch/drag to move camera, pinch to zoom (already exists)
- `building-mode-ui`: Toggle building mode via UI button
- `unit-selection-ui`: Spawn unit button when Unit Tower selected
- `win-lose-conditions`: Main Tower death = lose, Main Enemy Tower death = win

### Modified Capabilities
- None - these are all new capabilities for the tower defense game

## Impact

### Code Changes
- Delete 2 files in `Assets/_Scripts/Player/`
- Modify 2 files: `InputManager.cs`, `EnemyReceiveDamageBehaviour.cs`
- Rename 1 file: `TowerReceiveDamageBehaviour.cs` → `MainTowerBehaviour.cs`
- Create 7 new component files in `Assets/_Scripts/`

### Dependencies
- No new external dependencies required
- Uses existing Unity components (Rigidbody2D, Collider2D, etc.)

### Removed Functionality
- Player character movement and direct control
- Player weapon switching and reloading
- Player inventory system trigger
- Experience/leveling system for player
