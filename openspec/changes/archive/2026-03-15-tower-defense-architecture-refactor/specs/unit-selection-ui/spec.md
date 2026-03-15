## ADDED Requirements

### Requirement: Tapping a Unit Tower shows spawn button
When a Unit Tower is tapped/selected, a "Spawn Unit" button SHALL appear in the UI.

#### Scenario: Spawn button appears for Unit Tower
- **WHEN** player taps on a Unit Tower
- **THEN** a "Spawn Unit" button appears in the UI

#### Scenario: Spawn button shows unit cost (if applicable)
- **WHEN** a Unit Tower is selected and spawn button is visible
- **THEN** the button displays any resource cost to spawn a unit

### Requirement: Spawn button hidden when deselected
The spawn button SHALL be hidden when no valid tower is selected.

#### Scenario: Button hidden on empty area tap
- **WHEN** player taps on empty ground (not on a tower)
- **THEN** the spawn button is hidden

#### Scenario: Button hidden when non-Unit Tower selected
- **WHEN** player taps on a tower that is NOT a Unit Tower (e.g., Enemy Tower, Main Tower)
- **THEN** the spawn button is hidden

### Requirement: Spawn button triggers unit spawn
Tapping the spawn button SHALL spawn a unit at the selected Unit Tower.

#### Scenario: Tap spawn button creates unit
- **WHEN** player taps the "Spawn Unit" button while a Unit Tower is selected
- **THEN** a player unit spawns at the Unit Tower's position
