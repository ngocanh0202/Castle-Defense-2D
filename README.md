# Castle Defense 2D

## Overview
Castle Defense 2D is a strategic top-down tower defense game built with Unity for Android. Instead of directly controlling a single player character, the player acts as a commander overseeing the battlefield. Build towers, spawn units, and defend your base against waves of enemies.

## Core Features

### 🏰 Tower Defense System
- **Tower Placement**: Build various types of defensive towers to stop enemy waves
- **Tower Upgrades**: Improve damage, range, and attack speed of your towers
- **Resource Management**: Collect gold and resources to build and upgrade defenses
- **Strategic Positioning**: Plan optimal tower placement for maximum effectiveness
- **Unit Spawning**: Towers act as barracks, spawning allied soldiers to defend and counter-attack

### 🎥 RTS Camera Controls
- **Drag to Pan**: Single finger drag to move camera across the battlefield
- **Pinch to Zoom**: Two-finger pinch gesture to zoom in/out
- **Map Bounds**: Camera stays within map boundaries
- **No Direct Avatar**: Focus entirely on macro-strategy and base management

### ⚔️ Weapon System
- **Diverse Arsenal**: Swords, bows, magic staffs, and ranged weapons
- **Weapon Switching**: Change weapons instantly during combat
- **Weapon Upgrades**: Enhance damage, effects, and special properties
- **Combo System**: Chain attacks for increased effectiveness

### 🏗️ Building System
- **Base Construction**: Build houses and production facilities
- **Resource Production**: Automated gold and resource generation
- **Base Expansion**: Grow and fortify your defensive perimeter
- **Infrastructure Management**: Balance between offense and economy

## Installation & Setup

### System
- Unity 2022.3 LTS 
- Android SDK
- Visual Studio Code
- Git

## Gameplay Guide

### Controls (Touch)
- **Single Finger Drag**: Move camera (pan)
- **Two Finger Pinch**: Zoom in/out
- **Tap Tower**: Select tower to spawn units
- **Drag Tower to Enemy**: Command tower to prioritize target

### How to Play
1. **Preparation Phase**: Build towers and production buildings before enemy waves
2. **Command Phase**: Tap towers and spawn units to defend
3. **Combat Phase**: Towers auto-attack enemies; drag to set priority targets
4. **Development**: Use collected resources to upgrade and expand your defenses
5. **Strategy**: Balance between tower placement and unit spawning

## Technical Features

### 2D Systems
- **Sprite Rendering**: Optimized 2D sprite rendering pipeline
- **2D Physics**: Collision detection and physics interactions
- **Tilemap System**: Efficient terrain and level design
- **2D Animation**: Smooth character and enemy animations

### AI Systems
- **Pathfinding**: A* algorithm for enemy and allied unit movement
- **Behavior Trees**: Smart AI for different enemy types
- **Formation System**: Coordinated enemy group movement in structured formations
- **Dynamic Difficulty**: Adaptive challenge scaling

### Optimization
- **Object Pooling**: Reuse objects to reduce garbage collection
- **Sprite Batching**: Optimize rendering performance
- **Culling**: Only render visible objects
- **Memory Management**: Efficient resource loading and unloading

### Audio
- **2D Spatial Audio**: Positional audio effects
- **Dynamic Music**: Adaptive soundtrack based on game state
- **Sound Effects**: Rich audio feedback for all interactions

## Game Mechanics

### Tower Mechanics
- **Active Defense**: Towers automatically target and fire at enemies within range
- **Priority Targeting**: Drag from tower to enemy to prioritize specific targets
- **Unit Spawning**: Select towers to spawn allied soldiers for defense and counter-attacks
- **Tower Types**: Archer, Cannon, Magic, and Barracks towers

### Unit System
- **Allied Soldiers**: Spawned from towers, use A* pathfinding to hunt enemies
- **Formation Movement**: Units move in organized grid formations
- **Autonomous Behavior**: Units operate without direct player control
- **Enemy AI**: Enemies seek nearest tower, progressing to Main Tower

### Building Types
- **Gold Mine**: Generates gold over time
- **Barracks**: Trains defensive units
- **Workshop**: Upgrades weapons and towers
- **Wall Segments**: Defensive barriers

## Credits

- **Lead Developer**: [Bui Huynh Ngoc Anh]
- **Art**: [Bui Huynh Ngoc Anh]
- **Audio**: [Random]

## Contact

- **Email**: buihuynhngocanh@gmail.com
- **GitHub**: https://github.com/ngocanh0202

## Acknowledgments

- Unity Technologies for the game engine
- Unity 2D community for tutorials and resources
- Open source contributors and asset creators
- Playtesters and community feedback

---

**Target Platform**: Android (Touch Controls)
**Note**: This game is currently in development. Some features may be incomplete or subject to change in future updates.
