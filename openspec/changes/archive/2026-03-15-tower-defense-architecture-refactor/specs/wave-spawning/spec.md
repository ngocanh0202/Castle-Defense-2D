## ADDED Requirements

### Requirement: Enemy towers spawn enemies automatically on a timer
Enemy Towers SHALL spawn enemy units at configured intervals (wave timing).

#### Scenario: Enemy Tower spawns enemy on timer
- **WHEN** the wave timer elapses and an Enemy Tower exists
- **THEN** an enemy unit spawns at the Enemy Tower's position

#### Scenario: Multiple Enemy Towers each spawn
- **WHEN** the wave timer elapses and multiple Enemy Towers exist
- **THEN** each Enemy Tower spawns one enemy unit (simultaneous waves)

#### Scenario: Wave timer is configurable
- **WHEN** WaveManager is configured with a specific interval
- **THEN** waves spawn at that exact interval

### Requirement: WaveManager controls spawn timing
A centralized WaveManager SHALL coordinate all enemy spawning.

#### Scenario: WaveManager starts on game begin
- **WHEN** the game scene loads
- **THEN** WaveManager begins its timer for the first wave

#### Scenario: WaveManager stops on game over
- **WHEN** the game state changes to GameOver
- **THEN** WaveManager stops spawning waves

#### Scenario: WaveManager can pause/resume
- **WHEN** WaveManager receives pause/resume commands
- **THEN** the wave timer pauses or resumes accordingly

### Requirement: Spawned enemies are tagged as enemies
Enemy units spawned from Enemy Towers SHALL be properly tagged.

#### Scenario: Enemy unit tagged correctly
- **WHEN** an enemy unit spawns from an Enemy Tower
- **THEN** the unit is tagged as "Enemy" or appropriate enemy tag

### Requirement: Enemy units spawn with combat capability
Newly spawned enemy units SHALL immediately path toward the Main Tower.

#### Scenario: Enemy unit paths toward Main Tower
- **WHEN** an enemy unit spawns
- **THEN** it immediately targets the Main Tower (or nearest player unit if available)
