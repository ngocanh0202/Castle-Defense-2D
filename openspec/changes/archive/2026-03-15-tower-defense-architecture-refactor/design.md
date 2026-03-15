## Context

The Castle Defense 2D project is transitioning from an action RPG-style architecture to a tower defense architecture. The current codebase contains `PlayerController` and related components designed for a game where the player directly controls a character that moves, shoots, and levels up. However, the intended gameplay is tower defense where:

- Player does not control any character directly
- Player manages towers by building/placing them on a grid
- Player spawns units manually from Unit Towers
- Player controls camera via touch/drag gestures
- Enemies spawn from Enemy Towers on timers (waves)
- Enemy units path toward the Main Tower (player base)
- Player units defend against enemy units

## Goals / Non-Goals

**Goals:**
1. Remove unnecessary player character components (PlayerController, PlayerReceiveDamageBehaviour)
2. Create distinct tower types: MainTower, UnitTower, EnemyTower, MainEnemyTower
3. Implement manual unit spawning from Unit Towers
4. Implement unit combat AI (find nearest target, move, attack)
5. Implement wave-based enemy spawning system
6. Add UI for building mode toggle and unit spawning
7. Implement win/lose conditions

**Non-Goals:**
- Multiplayer functionality
- Save/load system
- Advanced enemy pathfinding (simple direct pathing is sufficient)
- Tower upgrade system (out of scope for initial implementation)
- Unit formation movement (already exists in UnitFormationSystem but not primary focus)

## Decisions

### Decision 1: Reuse Existing Damage System

**Choice:** Use existing `ReceiveDamageBehaviour` as base class for all damageable objects.

**Rationale:** The existing damage system is already implemented with health bars, damage intervals, and death handling. Rather than creating parallel systems, extend this for towers and units.

**Alternatives Considered:**
- Create separate damage interfaces for towers vs units → Rejected, unnecessary complexity
- Use Unity's damage system → Rejected, existing system works well

### Decision 2: Tower Component Architecture

**Choice:** Each tower type (MainTower, UnitTower, EnemyTower, MainEnemyTower) is a separate MonoBehaviour, all extending from a base `TowerBehaviour`.

**Rationale:** 
- Clear separation of concerns
- Different towers have different behaviors (some spawn, some attack, some are bases)
- Easier to test and maintain

**Alternatives Considered:**
- Single TowerBehaviour with enum for type → Rejected, leads to switch statements and unclear code
- Component-based (attach different components) → Rejected, too many dependencies to manage

### Decision 3: Unit Targeting Priority

**Choice:** Units target the nearest valid target (enemy unit or enemy tower), regardless of type.

**Rationale:**
- Simple and predictable behavior
- Units act as defenders - they engage whatever is closest
- Works well for both player units defending and enemy units attacking

**Alternatives Considered:**
- Priority system (always target towers first) → Rejected, units should engage threats equally
- Player manual targeting → Rejected, too much micro-management

### Decision 4: Wave Management

**Choice:** Centralized WaveManager controls all enemy spawning across all Enemy Towers.

**Rationale:**
- Single source of truth for wave timing
- Easy to implement difficulty scaling
- Centralized game state

**Alternatives Considered:**
- Each EnemyTower manages its own spawn timer → Rejected, harder to coordinate waves
- Spawn points defined in EnemySpawnerManager → Rejected, need tower-specific spawning

### Decision 5: Building Mode

**Choice:** Building mode is toggled via UI button, not keyboard.

**Rationale:**
- Mobile/touch-first design
- More accessible than keyboard shortcuts
- Clear visual feedback when active

## Risks / Trade-offs

**[Risk]** Game balance may be difficult to tune initially
→ **Mitigation:** Start with simple wave timings, allow easy adjustment via serialized fields

**[Risk]** Unit pathing may get blocked by obstacles
→ **Mitigation:** Keep terrain simple, use direct movement (no pathfinding) initially

**[Risk]** Many units may cause performance issues
→ **Mitigation:** Use object pooling for units (existing pattern in codebase)

**[Risk]** Camera bounds may not match gameplay area
→ **Mitigation:** Make bounds configurable, auto-detect from terrain

## Migration Plan

1. **Phase 1 - Cleanup:** Delete PlayerController, PlayerReceiveDamageBehaviour
2. **Phase 2 - Modify:** Update InputManager, EnemyReceiveDamageBehaviour
3. **Phase 3 - Rename:** TowerReceiveDamageBehaviour → MainTowerBehaviour
4. **Phase 4 - New Components:** Create UnitTowerBehaviour, EnemyTowerBehaviour, MainEnemyTowerBehaviour, UnitCombatBehaviour, WaveManager
5. **Phase 5 - UI:** Create BuildingModeUI, UnitSpawnButtonUI
6. **Phase 6 - Testing:** Test full game loop (build tower → spawn unit → wave spawns → combat → win/lose)

No rollback needed as we're removing unused code and adding new functionality.

## Open Questions

1. **Unit movement speed**: What should be the default speed for units? Should it vary by unit type?
2. **Wave timing**: How long between waves? Should it decrease over time (difficulty scaling)?
3. **Spawn positions**: Where do units spawn relative to their tower? Should there be a spawn offset?
4. **Unit capacity**: Should Unit Towers have a limit on how many units they can have active?
5. **Win condition**: Is destroying MainEnemyTower the only win condition, or are there other criteria?
