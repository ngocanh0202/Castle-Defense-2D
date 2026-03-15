## ADDED Requirements

### Requirement: Player can place Unit Tower on valid grid tiles
When building mode is enabled, player SHALL be able to tap on a valid (buildable) grid tile to place a Unit Tower.

#### Scenario: Place Unit Tower on empty buildable tile
- **WHEN** building mode is ON and player taps on an empty, buildable grid tile
- **THEN** a Unit Tower is instantiated at that tile position

#### Scenario: Cannot place Unit Tower on occupied tile
- **WHEN** building mode is ON and player taps on a tile that already has a tower
- **THEN** no tower is placed and existing tower remains unchanged

#### Scenario: Cannot place Unit Tower on non-buildable tile
- **WHEN** building mode is ON and player taps on a non-buildable grid tile
- **THEN** no tower is placed

#### Scenario: Cannot place Unit Tower before Main Tower exists
- **WHEN** building mode is ON and there is no Main Tower placed yet
- **THEN** player receives notification "Please place the Main Tower first!"

### Requirement: Player can place Main Tower once at game start
The Main Tower (player base) SHALL be placed exactly once at the start of the game.

#### Scenario: Place Main Tower on first buildable tile
- **WHEN** building mode is ON and player presses hotkey/toggle to place Main Tower and taps a buildable tile
- **THEN** Main Tower is placed at that position and tagged as "Player"

#### Scenario: Cannot place second Main Tower
- **WHEN** a Main Tower already exists and player attempts to place another
- **THEN** player receives notification "Main Tower already placed!"

### Requirement: Player can place Enemy Tower in building mode
For testing purposes, player SHALL be able to manually place enemy towers in building mode.

#### Scenario: Place Enemy Tower on buildable tile
- **WHEN** building mode is ON and player uses enemy tower placement and taps a buildable tile
- **THEN** an Enemy Tower is instantiated at that position

#### Scenario: Cannot place Enemy Tower before Main Tower exists
- **WHEN** building mode is ON and no Main Tower exists
- **THEN** player receives notification "Please place the Main Tower first!"

### Requirement: Grid visualization appears in building mode
The grid SHALL be visually rendered when building mode is active.

#### Scenario: Grid lines visible when building mode ON
- **WHEN** building mode is enabled
- **THEN** green grid lines are rendered showing buildable tiles

#### Scenario: Non-buildable tiles shown in red
- **WHEN** building mode is enabled and a tile is marked as non-buildable
- **THEN** that tile is rendered with red diagonal lines

#### Scenario: Grid hidden when building mode OFF
- **WHEN** building mode is disabled
- **THEN** grid visualization is hidden
