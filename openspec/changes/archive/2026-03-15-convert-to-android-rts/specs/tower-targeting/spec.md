## ADDED Requirements

### Requirement: Drag Targeting Gesture
Users SHALL be able to prioritize tower targets via drag gesture.

#### Scenario: Drag from tower to enemy
- **WHEN** user starts drag on selected tower and releases on enemy unit
- **THEN** tower prioritizes that enemy as target until destroyed or retargeted

#### Scenario: Drag from tower to empty space
- **WHEN** user starts drag on selected tower and releases on empty space
- **THEN** tower returns to default targeting behavior (nearest enemy)

### Requirement: Visual Targeting Feedback
The system SHALL provide visual feedback during targeting drag.

#### Scenario: Targeting line visible
- **WHEN** user starts drag from selected tower
- **THEN** a visual line appears from tower to finger position
- **AND** line changes color when hovering over valid target

#### Scenario: Targeting cancelled
- **WHEN** user releases drag outside any valid target
- **THEN** targeting line disappears
- **AND** tower maintains current target

### Requirement: Priority Target Override
Prioritized targets SHALL take precedence over default targeting.

#### Scenario: Priority target in range
- **WHEN** tower has a priority target within range
- **THEN** tower always attacks priority target first

#### Scenario: Priority target destroyed
- **WHEN** priority target is destroyed
- **THEN** tower returns to default targeting (nearest enemy)
