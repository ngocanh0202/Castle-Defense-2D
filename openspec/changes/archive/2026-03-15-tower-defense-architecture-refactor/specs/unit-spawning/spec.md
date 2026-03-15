## ADDED Requirements

### Requirement: Player can manually spawn units from Unit Tower
When a Unit Tower is selected, player SHALL be able to tap a "Spawn Unit" button to spawn a player unit at the tower's position.

#### Scenario: Spawn unit from Unit Tower
- **WHEN** player selects a Unit Tower and taps the "Spawn Unit" button
- **THEN** a player unit spawns at the Unit Tower's position

#### Scenario: Spawn button appears when Unit Tower is selected
- **WHEN** player taps on a Unit Tower
- **THEN** a "Spawn Unit" button appears in the UI

#### Scenario: Spawn button hidden when different tower selected
- **WHEN** player taps on a tower that is NOT a Unit Tower
- **THEN** the "Spawn Unit" button is hidden

#### Scenario: Spawn button hidden when no tower selected
- **WHEN** player taps on empty ground (no tower)
- **THEN** the "Spawn Unit" button is hidden

### Requirement: Units spawn with correct team affiliation
Spawned units SHALL be tagged as "Player" to identify them as friendly.

#### Scenario: Player unit tagged correctly
- **WHEN** a unit spawns from a Unit Tower
- **THEN** the unit is tagged as "Player"

### Requirement: Units have combat capability upon spawn
Newly spawned units SHALL immediately be able to engage in combat.

#### Scenario: Unit immediately targets nearest enemy
- **WHEN** a unit spawns
- **THEN** the unit immediately searches for the nearest enemy (unit or tower) and begins moving toward it if one exists
