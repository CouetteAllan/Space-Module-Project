using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum StatType
{
    Health,
    Damage,
    Resist,
    ReloadSpeed,
    NbProjectile,
    Weight
}

public class SingleStat
{
    private float _value;
    private StatType _type;
    private StatClass _statClass;

    public SingleStat(float value, StatType type, StatClass statClass)
    {
        _value = value;
        _type = type;
        _statClass = statClass;
    }



    public float Value => _value;
    public StatType Type => _type;
    public void SetValue(float value)
    {
        if (value < 0)
            throw new System.ArgumentException("value less than 0");

        _value = value;
    }

    public float ChangeValue(float value)
    {
        _value += value;
        return _value;
    }

    public void MultiplyValue(float value)
    {
        _value *= value;
    }

    public static SingleStat operator +(SingleStat a, SingleStat b){
        if (a.Type != b.Type)
            throw new System.ArgumentException($"The stat {a} and {b} are not the same type ");

        b.ChangeValue(a.Value);
        return b;
    }

}
