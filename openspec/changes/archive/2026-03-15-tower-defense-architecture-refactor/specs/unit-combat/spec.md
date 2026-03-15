## ADDED Requirements

### Requirement: Units automatically target nearest enemy
Both player units and enemy units SHALL find and target the nearest valid enemy (unit or tower of opposite team).

#### Scenario: Player unit targets nearest enemy unit
- **WHEN** a player unit exists and there are enemy units in the game
- **THEN** the player unit targets the enemy unit that is geographically closest

#### Scenario: Player unit targets nearest enemy tower
- **WHEN** a player unit exists and there are no enemy units but enemy towers exist
- **THEN** the player unit targets the enemy tower that is geographically closest

#### Scenario: Enemy unit targets nearest player unit or tower
- **WHEN** an enemy unit exists
- **THEN** it targets the nearest player unit, or if none exist, the Main Tower

#### Scenario: Unit retargets when current target dies
- **WHEN** a unit's current target is destroyed
- **THEN** the unit immediately searches for the next nearest valid target

### Requirement: Units move toward their target
Units SHALL move toward their target's position.

#### Scenario: Unit moves toward target
- **WHEN** a unit has a valid target
- **THEN** the unit moves toward the target's position at its configured speed

#### Scenario: Unit stops at attack range
- **WHEN** a unit with a target enters attack range of that target
- **THEN** the unit stops moving and begins attacking

### Requirement: Units attack targets within range
When within attack range of their target, units SHALL deal damage to the target.

#### Scenario: Unit attacks target in range
- **WHEN** a unit is within attack range of its target
- **THEN** the unit deals damage to the target at configured attack rate

#### Scenario: Unit stops attacking when target out of range
- **WHEN** a unit's target moves out of attack range
- **THEN** the unit stops attacking and resumes moving toward the target

### Requirement: Units die when health reaches zero
Units SHALL be destroyed when their health is reduced to zero.

#### Scenario: Unit dies at zero health
- **WHEN** a unit's health reaches zero or below
- **THEN** the unit is deactivated/destroyed

#### Scenario: Dead unit triggers death event
- **WHEN** a unit dies
- **THEN** appropriate death handlers are notified (for enemy units: may trigger rewards)
