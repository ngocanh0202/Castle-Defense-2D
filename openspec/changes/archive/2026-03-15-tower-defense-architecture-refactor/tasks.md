## 1. Cleanup - Remove Unnecessary Files

- [x] 1.1 Delete `Assets/_Scripts/Player/PlayerController.cs`
- [x] 1.2 Delete `Assets/_Scripts/Player/PlayerReceiveDamageBehaviour.cs`
- [x] 1.3 Delete `Assets/_Scripts/Player/` directory if empty

## 2. Modify InputManager

- [x] 2.1 Remove `OnMove` event and handler
- [x] 2.2 Remove `OnStop` event and handler
- [x] 2.3 Remove `OnAttack` event and handler
- [x] 2.4 Remove `OnSetBulletStrategy` event and handler
- [x] 2.5 Remove `OnReloadAmmo` event and handler
- [x] 2.6 Remove `OnOpenInventory` event and handler
- [x] 2.7 Remove `OnSetWeaponRotation` event and handler
- [x] 2.8 Add `OnToggleBuildMode` event with Action<bool> delegate

## 3. Fix EnemyReceiveDamageBehaviour

- [x] 3.1 Remove `PlayerController.Instance.IncreaseEXP` reference from OnDie handler

## 4. Rename TowerReceiveDamageBehaviour

- [x] 4.1 Rename file `TowerReceiveDamageBehaviour.cs` to `MainTowerBehaviour.cs`
- [x] 4.2 Update class name from `TowerReceiveDamageBehaviour` to `MainTowerBehaviour`

## 5. Create UnitTowerBehaviour

- [x] 5.1 Create new file `Assets/_Scripts/Towers/UnitTowerBehaviour.cs`
- [x] 5.2 Inherit from `ReceiveDamageBehaviour`
- [x] 5.3 Add unit spawn functionality (spawns player units on button press)
- [x] 5.4 Add tower attack functionality (attacks enemies in range)
- [x] 5.5 Override Die() to disable tower when destroyed

## 6. Create EnemyTowerBehaviour

- [x] 6.1 Create new file `Assets/_Scripts/Towers/EnemyTowerBehaviour.cs`
- [x] 6.2 Inherit from `ReceiveDamageBehaviour`
- [x] 6.3 Add auto-spawn functionality (timer-based)
- [x] 6.4 Add tower attack functionality (attacks player units in range)
- [x] 6.5 Override Die() to disable tower when destroyed

## 7. Create MainEnemyTowerBehaviour

- [x] 7.1 Create new file `Assets/_Scripts/Towers/MainEnemyTowerBehaviour.cs`
- [x] 7.2 Inherit from `EnemyTowerBehaviour`
- [x] 7.3 Override Die() to trigger VICTORY game state

## 8. Create UnitCombatBehaviour

- [x] 8.1 Create new file `Assets/_Scripts/Units/UnitCombatBehaviour.cs`
- [x] 8.2 Add target finding logic (find nearest enemy unit or tower)
- [x] 8.3 Add movement logic (move toward target)
- [x] 8.4 Add attack logic (deal damage when in range)
- [x] 8.5 Handle target death (retarget to next nearest)
- [x] 8.6 Add unit death handling

## 9. Create WaveManager

- [x] 9.1 Create new file `Assets/_Scripts/Managers/WaveManager.cs`
- [x] 9.2 Implement singleton pattern
- [x] 9.3 Add wave timer functionality
- [x] 9.4 Add StartWaves() and StopWaves() methods
- [x] 9.5 Handle GameOver state (stop spawning)
- [x] 9.6 Make spawn interval configurable

## 10. Create BuildingModeUI

- [x] 10.1 Create new file `Assets/_Scripts/UI/BuildingModeUI.cs`
- [x] 10.2 Add UI button to toggle building mode
- [x] 10.3 Connect button to TowerGridManager
- [x] 10.4 Show/hide building mode indicators

## 11. Create UnitSpawnButtonUI

- [x] 11.1 Create new file `Assets/_Scripts/UI/UnitSpawnButtonUI.cs`
- [x] 11.2 Show spawn button when Unit Tower is selected
- [x] 11.3 Hide button when different tower or empty area selected
- [x] 11.4 Connect button to spawn unit at selected tower

## 12. Create Units Directory and Refactor

- [x] 12.1 Create `Assets/_Scripts/Units/` directory
- [x] 12.2 Move UnitCombatBehaviour to Units directory
- [x] 12.3 Create placeholder for player units (uses UnitCombatBehaviour)
- [x] 12.4 Create placeholder for enemy units (uses UnitCombatBehaviour)

## 13. Testing and Integration

- [ ] 13.1 Test placing Main Tower
- [ ] 13.2 Test placing Unit Tower
- [ ] 13.3 Test building mode toggle
- [ ] 13.4 Test unit spawning from Unit Tower
- [ ] 13.5 Test enemy wave spawning
- [ ] 13.6 Test unit combat (movement and attacking)
- [ ] 13.7 Test Main Tower destruction (Game Over)
- [ ] 13.8 Test Main Enemy Tower destruction (Victory)
