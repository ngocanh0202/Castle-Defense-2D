## ADDED Requirements

### Requirement: Game over when Main Tower is destroyed
When the Main Tower's health reaches zero, the game SHALL end in a loss state.

#### Scenario: Game over triggers on Main Tower death
- **WHEN** Main Tower receives fatal damage
- **THEN** the game state changes to GameOver and victory screen shows "DEFEAT"

#### Scenario: Towers stop attacking on game over
- **WHEN** game state changes to GameOver
- **THEN** all towers set isStopAttack = true and stop attacking

### Requirement: Victory when Main Enemy Tower is destroyed
When the Main Enemy Tower (enemy base) is destroyed, the game SHALL end in a victory state.

#### Scenario: Victory triggers on Main Enemy Tower death
- **WHEN** Main Enemy Tower receives fatal damage
- **THEN** the game state changes to GameOver and victory screen shows "VICTORY"

### Requirement: Game over stops enemy spawning
When the game ends, enemy spawning SHALL stop.

#### Scenario: WaveManager stops on game over
- **WHEN** game state changes to GameOver
- **THEN** WaveManager stops its spawning timer

### Requirement: Enemy units continue moving after game over (optional)
Enemy units that are already spawned MAY continue their behavior after game over, or may stop.

#### Scenario: Enemy units stop on game over
- **WHEN** game state changes to GameOver
- **THEN** all enemy units stop their movement and attacking

### Requirement: Player can restart after game over
After game over, player SHALL be able to restart the game.

#### Scenario: Restart option available
- **WHEN** game is in GameOver state
- **THEN** a restart button is available to reload the scene
