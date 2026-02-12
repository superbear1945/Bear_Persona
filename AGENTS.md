# Repository Guidelines

## Project Structure & Module Organization

- `Assets/Script/` holds all gameplay and system C# code (e.g., FSM, abilities, controllers).
- `Assets/Data/` contains ScriptableObject configs such as `UnitData`.
- `Assets/Prefab/` stores reusable prefabs; `Assets/Scenes/` stores scene files.
- `Assets/Input/` keeps Input System action maps; `Assets/Settings/` and `ProjectSettings/` track Unity project configuration.
- `Doc/` contains feature specs (e.g., possession mechanic notes).

## Build, Test, and Development Commands

- Open the project in Unity Hub, then use `File > Build Settings` to build. No custom build scripts are defined.
- Play and iterate in the Unity Editor; the project uses URP 2D and the new Input System.
- Tests are not currently committed. If you add tests, use Unity Test Runner (EditMode/PlayMode).

## Coding Style & Naming Conventions

- **All comments, documentation, and string literals must be in Simplified Chinese.**
- Private fields: `_camelCase` (e.g., `_moveAction`). Public members: `PascalCase`. Interfaces: `IPascalCase`.
- Avoid abbreviations in names (`distance` instead of `dist`). Prefer guard clauses to reduce deep `if/else` nesting.
- Use `[SerializeField]`, `[Header]`, and `[Tooltip]` to keep Inspector fields organized.

## Testing Guidelines

- When adding tests, place them under `Assets/Tests/EditMode` or `Assets/Tests/PlayMode`.
- Name tests by behavior, e.g., `ChargeAttack_DoesDamage_WhenFullyCharged`.
- Run tests via Unity’s Test Runner window.

## Commit & Pull Request Guidelines

- Commit messages are typically short, descriptive Chinese summaries; `feat:` is occasionally used. Match that style (one-line, action-focused).
- PRs should describe what changed and why, link relevant issues, and include screenshots/GIFs for gameplay or UI changes.
- Note any new assets, editor settings changes, or required migrations.

## Agent-Specific Instructions

- This is a Unity 2D project with a possession mechanic; core code lives in `Assets/Script/`.
- Follow existing architecture (FSM, ScriptableObject-driven configs) and keep gameplay logic data-driven.
