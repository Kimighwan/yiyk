using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Tpye
{
    lever,
    pressureStep
}

public interface IPuzzle
{
    public void Open();
    public void On();
}
