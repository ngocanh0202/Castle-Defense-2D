## Why

The current gameplay requires direct character control (WASD movement + mouse aiming), which is designed for PC but not suitable for mobile Android deployment. Converting to an RTS-style tower defense with touch controls will enable Android deployment while maintaining strategic gameplay.

## What Changes

- **Remove** direct player character controller (WASD movement, mouse attack)
- **Add** RTS camera system with touch controls (drag to pan, pinch to zoom)
- **Add** tower selection and unit spawning system (tap tower → select unit → spawn)
- **Add** tower targeting system (drag from tower to enemy to prioritize target)
- **Add** formation algorithm for unit movement (organized squad structure)
- **Update** InputManager to handle touch events instead of keyboard/mouse
- **Update** README.md with new gameplay description

## Capabilities

### New Capabilities

- `rts-camera`: RTS-style camera with touch pan, pinch zoom, and map bounds
- `tower-interaction`: Tap-to-select towers and spawn units from inventory
- `tower-targeting`: Drag gesture to command towers to target specific enemies
- `unit-formation`: Formation algorithm for organizing units during movement

### Modified Capabilities

- None - this is a new feature set

## Impact

- **Code Changes**:
  - `Assets/_Scripts/Camera/CameraController.cs` - Implement RTS camera
  - `Assets/_Scripts/Managers/InputManager.cs` - Add touch input handling
  - `Assets/_Scripts/Player/PlayerController.cs` - Disable movement/attack
  - New: `TowerInteraction.cs` - Tower selection and spawning
  - New: `TowerTargetingSystem.cs` - Drag-to-target functionality
  - New: `UnitFormationSystem.cs` - Formation algorithms
- **Documentation**: Update README.md with new gameplay overview
