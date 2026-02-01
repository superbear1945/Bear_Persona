using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISwitchable
{
    // 标记当前单位是否被附身
    bool IsSwitched { get; set; }

    public void OnEnterSwitch();
    public void OnExitSwitch();

    // 行为接口
    public void OnMove(Vector2 direction);
    public void OnAttack();
    public void OnSpecialAttack();
    public void OnPossess();
}
