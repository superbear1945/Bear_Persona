# 附身机制与技能指示器实现文档

本文档详细说明了附身（Possession）模式、子弹时间与技能范围指示器的架构与实现细节。

## 核心架构

重构后的架构采用 **PlayerController** 作为核心指挥者，协调各个子系统的功能。

### 1. PlayerController (指挥者)

**位置**: `Assets/Script/PlayerController.cs`
**职责**:

- **状态管理**: 维护当前是否处于“附身准备状态”（即子弹时间）。
- **流程协调**: 当玩家按下 Switch 键时：
    1. 调用 `TimeManager` 开启/关闭慢动作。
    2. 控制 `RangeCircle` 显示/隐藏当前技能范围。
- **配置入口**:
  - `Range Circle Prefab`: 技能指示器的预制体。
  - `Skill Range`: 技能半径。

### 2. TimeManager (时间服务)

**位置**: `Assets/Script/System/TimeManager.cs`
**职责**:

- 纯粹的时间管理：`TimeScale` 和 `FixedDeltaTime` 的平滑切换。
- 全屏视觉增强（如变暗蒙版），这部分保留在这里作为全局特效是合适的。

### 3. RangeCircle (视觉表现)

**位置**: `Assets/Script/System/RangeCircle.cs`
**职责**:

- 使用 `LineRenderer` 绘制圆环。
- 跟随指定的目标（即当前被控制的单位）。

---

## 具体实现流程

### 进入附身模式 (TogglePossessionMode)

当 `ControlledState` 捕获到输入时，调用 `PlayerController.Instance.TogglePossessionMode()`：

1. **时间变速**:
   `PlayerController` 调用 `TimeManager.Instance.ToggleBulletTime()`，游戏变慢。

2. **显示指示器**:
    - `PlayerController` 实例化（或激活）`RangeCircle`。
    - 设置圆环半径 (`Setup`)。
    - 设置跟随目标 (`SetTarget`) 指向 `currentUnit`。

### 退出附身模式

再次调用 Toggle 或时间耗尽时：

1. `TimeManager` 恢复时间。
2. `PlayerController` 隐藏/销毁 `RangeCircle`。

---

## 为什么这样改？

- **单一职责**: `TimeManager` 不再关心圆形、射线检测或技能逻辑，只管时间。
- **逻辑内聚**: 技能范围是玩家操作的一部分，指示器跟随玩家当前单位，这些数据都在 `PlayerController` 中，调用链更短，逻辑更自然。
