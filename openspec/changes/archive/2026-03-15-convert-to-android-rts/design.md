## Context

The project is a Unity 2D tower defense game currently designed for PC with direct character control. The player uses WASD to move and mouse to aim/attack. The goal is to convert this to an Android-ready RTS-style tower defense game.

**Current State:**
- `PlayerController.cs` - Handles WASD movement, camera follows player
- `CameraController.cs` - Empty placeholder
- `InputManager.cs` - Keyboard/mouse input handling
- Towers auto-attack enemies in range
- No unit spawning from towers

**Constraints:**
- Target platform: Android (touch input)
- Unity 2022.3 LTS
- Must maintain existing tower defense mechanics

## Goals / Non-Goals

**Goals:**
- Implement RTS camera with touch controls (drag pan, pinch zoom, bounds)
- Add tower interaction system (tap to select, spawn units)
- Add tower targeting via drag gesture
- Implement unit formation algorithm
- Update documentation

**Non-Goals:**
- Adding online/multiplayer features
- Changing core tower defense economics
- Adding new enemy types
- Implementing AI-controlled allied units (they operate autonomously)

## Decisions

### 1. Camera Control: Touch-based RTS Camera

**Decision**: Use single-finger drag for panning and two-finger pinch for zoom.

**Rationale**: This is the standard pattern for mobile RTS games (Clash Royale, Age of Empires Mobile). Edge panning doesn't work on touch screens.

**Alternative Considered**:
- Tap buttons at screen edges - Less intuitive, blocks UI
- Minimap-only navigation - Too slow for combat

### 2. Tower Selection: Tap-based

**Decision**: Single tap on tower selects it and shows unit selection UI.

**Rationale**: Simple, familiar mobile pattern. Long press introduces delay.

**Alternative Considered**:
- Long press to select - Slower, less responsive feel
- Drag from tower to spawn - Conflicts with camera drag

### 3. Targeting: Drag Gesture

**Decision**: Drag from selected tower toward enemy to prioritize targeting.

**Rationale**: Direct manipulation feels natural on touch. Player visually "pulls" the target.

**Alternative Considered**:
- Tap tower then tap enemy - Two taps, less intuitive
- Priority buttons UI - Takes screen space

### 4. Unit Formation: Grid-based Formation

**Decision**: Units spawn in grid formation behind tower, move in formation toward target.

**Rationale**: Maintains organized appearance, easier to control. Simple to implement.

**Alternative Considered**:
- Flocking behavior - More complex, harder to predict
- Line formation - Looks less organized

### 5. Player Entity: Keep but Passive

**Decision**: Keep Player entity in scene but disable all input handling.

**Rationale**: May be needed for future features (spectator mode, stats display). Easier to re-enable than recreate.

## Risks / Trade-offs

- **[Risk] Touch input latency** → Mitigation: Use `Input.GetTouch()` with cached references, avoid allocations in Update
- **[Risk] Pinch zoom sensitivity** → Mitigation: Configurable zoom speed, smooth interpolation
- **[Risk] Drag conflicting with camera** → Mitigation: Raycast to detect if drag started on tower vs empty space
- **[Risk] Formation units overlapping** → Mitigation: Use physics collision avoidance in movement

## Migration Plan

1. Update README.md with new gameplay description
2. Implement CameraController with touch controls
3. Disable player movement/attack in InputManager
4. Create TowerInteraction component
5. Create TowerTargetingSystem
6. Create UnitFormationSystem
7. Test all interactions in Unity Editor
8. Build for Android to verify

## Open Questions

- Should player be able to see HP bars of distant towers? (Minimap feature)
- What happens if all towers are destroyed? (Game over condition)
- Should units have individual targeting or follow tower's target? (Default to following tower)
