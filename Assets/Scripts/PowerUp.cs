using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//導入到要觸發的物體 e.g.鑽石
public enum PowerUpType { None, Strong, Bullet, Smash}    //定義一組不同的能力或效果類型

public class PowerUp : MonoBehaviour
{
    public PowerUpType powerUpType;
}
