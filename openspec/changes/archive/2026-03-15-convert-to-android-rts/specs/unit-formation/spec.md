## ADDED Requirements

### Requirement: Grid Formation Spawn
Units SHALL spawn in organized grid formation behind the tower.

#### Scenario: Single unit spawn
- **WHEN** one unit spawns at tower
- **THEN** unit positions itself directly behind tower

#### Scenario: Multiple units spawn
- **WHEN** multiple units spawn from same tower in quick succession
- **THEN** units arrange in grid formation (e.g., 2x2, 3x2)
- **AND** each unit maintains spacing from neighbors

### Requirement: Formation Movement
Units SHALL move in formation toward targets.

#### Scenario: Formation moves to target
- **WHEN** units receive move command to target position
- **THEN** all units move together maintaining formation structure
- **AND** units adjust speed to stay synchronized

### Requirement: Formation Collision Avoidance
Units SHALL avoid colliding with each other during movement.

#### Scenario: Units approaching obstacle
- **WHEN** formation encounters obstacle
- **THEN** units adjust path individually while trying to maintain formation
- **AND** units do not overlap or stack on same position

### Requirement: Formation Reorganization
Units SHALL reorganize when formation is disrupted.

#### Scenario: Unit blocked
- **WHEN** a unit in formation is blocked
- **THEN** that unit finds alternative path
- **AND** other units continue toward target
- **AND** formation reorganizes when possible

### Requirement: Attack Formation
Units SHALL maintain formation while engaging enemies.

#### Scenario: Formation engages enemy
- **WHEN** formation units engage enemy unit
- **THEN** front-line units attack while rear units maintain formation
- **AND** units rotate to face target direction
