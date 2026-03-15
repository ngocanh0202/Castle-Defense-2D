# AGENTS.md - Castle Defense 2D

## Project Overview

This is a Unity 2D tower defense game project using C#. The project uses Unity 2022.3 LTS with the Unity Test Framework for testing.

---

## Build, Test, and Development Commands

### Unity Editor
- **Open Project**: Open in Unity Hub or double-click the project folder
- **Build Player**: `File > Build Settings` or use Unity CLI: `Unity -buildTarget <platform> -quit -batchmode -executeMethod BuildScript.Build`
- **Run Tests**: `Window > General > Test Runner` or run via command line:
  ```bash
  Unity -runTests -projectPath <path> -testResults <results.xml> -testFilter <filter>
  ```
- **Run Single Test**: Use `[UnityTest]` attribute with specific test name filter:
  ```bash
  Unity -runTests -projectPath . -testFilter "TestNamespace.TestClass.TestMethod"
  ```

### Editor Shortcuts
- **Play**: Ctrl+P
- **Pause**: Ctrl+Shift+P
- **Step**: Ctrl+Alt+P
- **Build and Run**: Ctrl+B

---

## Code Style Guidelines

### File Organization

```
Assets/
├── _Scripts/          # Main game scripts (custom namespace)
│   ├── Enemy/
│   ├── Weapons/
│   └── Interfaces/
├── Common Scripts/     # Reusable utilities
│   ├── Singleton/
│   ├── Utilities/
│   └── Testing/
├── Plugins/           # Third-party assets (DOTween, etc.)
└── Resources/         # Loadable resources
```

### Naming Conventions

| Element | Convention | Example |
|---------|------------|---------|
| Classes | PascalCase | `EnemyBehaviour`, `EnemyStateMachine` |
| Interfaces | PascalCase with I prefix | `IState`, `IEnemyState` |
| Methods | PascalCase | `InitializeComponents()`, `SwitchState()` |
| Properties | PascalCase | `Instance`, `CurrentState` |
| Fields/Variables | camelCase | `enemyStat`, `mainTowerTransform` |
| Private Fields | camelCase with optional `_` prefix | `instance`, `_instance` |
| Constants | PascalCase | `MaxHealth`, `DefaultSpeed` |
| Enums | PascalCase | `KeyOfObjPooler`, `KeyGuns` |
| Namespaces | PascalCase | `Common2D`, `Utilities` |

### Class Structure Order

1. Using statements (alphabetical, Unity first)
2. Namespace declaration
3. Class declaration with attributes
4. Public fields with `[Header]` attributes
5. Private fields
6. Properties
7. Unity lifecycle methods (Awake, Start, OnEnable, Update, FixedUpdate, etc.)
8. Public methods
9. Private methods

### Using Statements

```csharp
using System;
using System.Collections;
using Common2D;
using UnityEngine;
using UnityEngine.UI;
```

Order: System > Unity > Third-party > Project-specific

### Attributes Usage

```csharp
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(EnemyStat))]
public class EnemyStateMachine : MonoBehaviour
{
    [Header("Components")]
    public Animator animator;
    
    [Header("Enemy States")]
    public EnemyBaseState CurrentState;
    
    [SerializeField] private string currentStateName;
}
```

### Property and Field Guidelines

- Use `[SerializeField]` for private fields that need to be editable in Inspector
- Use public fields directly for simple data (no validation needed)
- Use properties for computed values or when validation is needed
- Use expression-bodied members when simple: `public string PoolName => KeyOfObjPooler.Enemy.ToString();`

### State Machine Pattern

Follow the existing state machine pattern using interfaces:

```csharp
public interface IState
{
    void EnterState();
    void UpdateState();
    void FixUpdateState();
    void OnCollision2D(Collision2D collision);
    void OnTriggerEnter2D(Collider2D collision);
}

public abstract class EnemyBaseState : IEnemyState, IState, IDrawGizmos
{
    public EnemyStateMachine StateMachine { get; set; }
    public Animator Animator { get; set; }
    
    public virtual void EnterState() { }
    // ... other methods
}
```

### Singleton Pattern

Use the provided generic Singleton<T> base class:

```csharp
public class InputManager : Singleton<InputManager>
{
    // Instance is automatically available via InputManager.Instance
}
```

### Null Checking

- Use null-conditional operators: `CurrentState?.EnterState()`
- Use null-coalescing: `mainTowerTransform = GameObject.Find("MainTower")?.transform;`
- Check before using: `if (CurrentState == null) return;`

### Error Handling

- Use defensive null checks in public APIs
- Log errors using `Debug.LogError()` for critical failures
- Use `Debug.LogWarning()` for non-critical issues
- Avoid exceptions for flow control

### Accessibility

- Default to private; make public only when needed
- Use `[SerializeField]` to expose private fields to Inspector
- Use protected for class members that may be overridden

### Comments and Documentation

- Avoid unnecessary comments; code should be self-documenting
- Use `[Header("...")]` attributes to group Inspector fields
- Use TODO comments for incomplete code: `// TODO: Implement X`

### Performance Considerations

- Use object pooling for frequently created/destroyed objects
- Cache component references in Awake/Start
- Use `[SerializeField]` and GetComponent in InitializeComponents()
- Avoid LINQ in Update/FixedUpdate loops

### Unity-Specific Guidelines

- Use Awake for initialization, Start for post-Awake setup
- Use Update for logic, FixedUpdate for physics
- Use OnDrawGizmos for debugging visualization
- Use Coroutines for delayed operations
- Reference Transforms, not GameObjects when possible

---

## Testing

### Test Framework
The project uses Unity Test Framework (`com.unity.test-framework`).

### Writing Tests
- Place tests in a `Tests` folder within `Assets`
- Use `[UnityTest]` for tests that need coroutine support
- Use `[Test]` for synchronous tests
- Use `[SetUp]` and `[TearDown]` for test fixtures

### Example Test Structure
```csharp
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class EnemyTests
{
    [SetUp]
    public void Setup()
    {
        // Setup test environment
    }
    
    [Test]
    public void EnemyTakesDamage()
    {
        // Test implementation
    }
    
    [UnityTest]
    public IEnumerator EnemyDiesAfterTimeout()
    {
        // Test with yield return
    }
}
```

---

## Common Patterns

### Object Pooling
```csharp
public class CommonPoolObject : MonoBehaviour
{
    public virtual string PoolName => "";
}
```

### Event System
```csharp
gun.OnSetBulletStrategy += OnChangeTextBulletStrategy;
gun.OnAmmoChanged += OnAmountChanged;
```

### Factory/Creation Pattern
```csharp
CreateGameObject.CreateCountdown(5, Vector3.zero, ...);
CreateGameObject.CreateTextPopup("Up", position, Color.yellow);
```

---

## Additional Resources

- Unity Documentation: https://docs.unity3d.com/
- C# Coding Conventions: https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions
