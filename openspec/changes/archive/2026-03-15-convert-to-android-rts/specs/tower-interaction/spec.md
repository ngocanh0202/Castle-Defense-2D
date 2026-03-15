## ADDED Requirements

### Requirement: Tower Selection
Towers SHALL be selectable via tap gesture.

#### Scenario: Tap on tower
- **WHEN** user taps on a tower collider
- **THEN** tower becomes selected and displays selection indicator

#### Scenario: Tap on empty space
- **WHEN** user taps on area with no tower
- **THEN** any previously selected tower becomes deselected

#### Scenario: Tap on different tower
- **WHEN** user taps on a different tower while one is selected
- **THEN** new tower becomes selected, previous tower deselected

### Requirement: Unit Spawn Panel
Selecting a tower SHALL display a unit selection UI panel.

#### Scenario: Tower selected shows panel
- **WHEN** tower is selected
- **THEN** unit selection panel appears near the tower with available unit options

#### Scenario: Panel closes on deselect
- **WHEN** tower is deselected
- **THEN** unit selection panel closes

### Requirement: Unit Spawning
Users SHALL be able to spawn units from selected towers.

#### Scenario: Spawn unit from tower
- **WHEN** user selects a tower, then taps a unit option in the panel
- **THEN** selected unit spawns at tower location
- **AND** unit begins behaving according to its AI (march toward enemy)

#### Scenario: Unit spawn cooldown
- **WHEN** user attempts to spawn unit while cooldown is active
- **THEN** spawn is rejected and cooldown indicator is shown

### Requirement: Multiple Tower Selection
The system SHALL support selecting multiple towers for batch commands.

#### Scenario: Multi-select towers
- **WHEN** user taps on multiple towers while holding shift or uses selection rectangle
- **THEN** all tapped towers become selected
- **AND** unit spawn panel shows options that apply to all selected towers
