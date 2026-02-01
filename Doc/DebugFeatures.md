# 调试功能文档

## BearUnit Inspector 开关

**位置**: `Assets/Script/BearUnit.cs`

**功能**:
在 `BearUnit` 组件的 Inspector 界面中，有一个 `Is Switched` 复选框。

- **勾选**: 强制将状态机切换到 `ControlledState`。此时可以使用键盘控制该单位。
- **取消勾选**: 强制将状态机切换到 `IdleState`。

**代码位置**:
逻辑位于 `BearUnit.UpdateDebugSwitchState()` 方法中。

**如何移除**:
删除 `UpdateDebugSwitchState()` 方法及其在 `Update()` 中的调用。
