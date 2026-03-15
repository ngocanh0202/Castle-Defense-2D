## ADDED Requirements

### Requirement: UI button toggles building mode
A visible button in the game UI SHALL toggle building mode ON/OFF.

#### Scenario: Tap toggle button enables building mode
- **WHEN** player taps the building mode toggle button while it's OFF
- **THEN** building mode is enabled and visual indicator shows it's active

#### Scenario: Tap toggle button disables building mode
- **WHEN** player taps the building mode toggle button while it's ON
- **THEN** building mode is disabled and visual indicator shows it's inactive

#### Scenario: Button shows current state
- **WHEN** building mode is ON or OFF
- **THEN** the toggle button visually reflects the current state (e.g., different color, icon)

### Requirement: Building mode controls are visible when active
When building mode is enabled, the UI SHALL show relevant controls.

#### Scenario: Tower selection panel appears in building mode
- **WHEN** building mode is enabled
- **THEN** a panel shows available tower types to place (Unit Tower, Main Tower, Enemy Tower)

#### Scenario: Grid becomes visible in building mode
- **WHEN** building mode is enabled
- **THEN** the grid visualization appears, showing buildable tiles

### Requirement: Building mode disables when game is over
Building mode SHALL automatically turn off when the game ends.

#### Scenario: Building mode off on game over
- **WHEN** game state changes to GameOver
- **THEN** building mode is disabled automatically
