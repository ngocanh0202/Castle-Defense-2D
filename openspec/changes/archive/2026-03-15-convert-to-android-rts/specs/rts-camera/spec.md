## ADDED Requirements

### Requirement: RTS Camera Pan
The camera SHALL support panning by single-finger drag gesture on touch screens.

#### Scenario: Drag to pan camera
- **WHEN** user places one finger on screen and drags
- **THEN** camera moves in the opposite direction of the drag, following the finger movement

#### Scenario: Drag stops
- **WHEN** user lifts finger from screen
- **THEN** camera stops panning immediately

### Requirement: RTS Camera Zoom
The camera SHALL support zooming via two-finger pinch gesture.

#### Scenario: Pinch to zoom in
- **WHEN** user places two fingers on screen and moves them apart
- **THEN** camera zooms in (decreases orthographic size)

#### Scenario: Pinch to zoom out
- **WHEN** user places two fingers on screen and moves them together
- **THEN** camera zooms out (increases orthographic size)

#### Scenario: Zoom limits
- **WHEN** pinch zoom would exceed minimum or maximum zoom bounds
- **THEN** camera stops at the bound limit

### Requirement: Camera Bounds
The camera SHALL be constrained within map boundaries.

#### Scenario: Camera at boundary
- **WHEN** camera panning would move view outside map bounds
- **THEN** camera stops at the nearest boundary edge

### Requirement: Smooth Camera Movement
Camera movement SHALL be smooth with interpolation.

#### Scenario: Camera interpolation
- **WHEN** camera receives pan or zoom input
- **THEN** camera smoothly interpolates to new position over time (not instant snap)
