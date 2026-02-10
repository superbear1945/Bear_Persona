# Project Guidelines

## Code Style
- **Language**: Write all code comments, documentation, and string literals in **Chinese (Simplified)**.
- **Naming**: 
  - `_camelCase` for private fields (e.g., `_moveAction`).
  - `PascalCase` for properties and methods.
  - `IPascalCase` for interfaces.
- **Attributes**: Use `[SerializeField]`, `[Header]`, and `[Tooltip]` to organize Inspector fields.
- **Example**: See `Assets/Script/Ability/ChargeAttack.cs`.

## Architecture
- **Patterns**: Singleton managers (`PlayerController`, `TimeManager`), FSM for AI logic (`Assets/Script/FSM`).
- **Input**: `UnityEngine.InputSystem` via `[SerializeField] private InputActionReference`.
- **Data**: `ScriptableObject` for game data configurations (`Assets/Data`).
- **Separation**: Visuals (Indicators) separated from Logic (Controllers).

## Build and Test
- **Engine**: Unity (URP 2D Pipeline).
- **Compilation**: Standard C# .NET solution.
- **Tools**: Reference `Antigravity` tools for editor extensions.

## Project Conventions
- **Docs**: Refer to `Doc/` for feature specs (e.g., `PossessionMechanic.md`).
- **Analysis**: Check `Assets/Script/Ability/` for ability implementation patterns.

## Integration Points
- **Unity Packages**: Input System, Universal RP.
- **External**: MCP Installer, custom Antigravity editor tools.
