using System;
using UnityEngine;

public enum StatType
{
    Health,
    Damage,
    Resist,
    ReloadSpeed,
    NbProjectile,
    Weight,
    MaxWeight
}

[Serializable]
public class SingleStat
{
    [SerializeField] private string _name;
    [SerializeField] private float _baseValue;
    [SerializeField] private StatType _type;
    private float _value;
    private StatClass _statClass;

    public SingleStat(float value, StatType type, StatClass statClass)
    {
        _baseValue = value;
        _value = _baseValue;
        _type = type;
        _statClass = statClass;
    }
    public SingleStat(float value, StatType type)
    {
        _baseValue = value;
        _value = _baseValue;
        _type = type;
    }



    public float Value => _value;
    public float BaseValue => _baseValue;
    public StatType Type => _type;
    public void SetValue(float value)
    {
        if (value < 0)
            throw new ArgumentException("value less than 0");

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
    public void MultiplyPercentValue(float percent)
    {
        float newValue = _baseValue * percent;
        _value += newValue;
    }

    public static SingleStat operator +(SingleStat a, SingleStat b){
        if (a.Type != b.Type)
            throw new System.ArgumentException($"The stat {a} and {b} are not the same type ");

        b.ChangeValue(a.Value);
        return b;
    }

    public override string ToString()
    {
        return _name + ": " + Value;
    }

}
