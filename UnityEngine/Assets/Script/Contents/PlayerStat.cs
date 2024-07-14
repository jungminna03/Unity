using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerStat : Stat
{
    [SerializeField]
    int _gold;
    [SerializeField]
    int _exp;

    public int Gold { get { return _gold; } set { _gold = value; } }    

    public int Exp 
    { 
        get { return _exp; } 

        set 
        { 
            _exp = value;

            int level = 1;
            while (true)
            {
                Data.Stat _stat;
                if (!Managers.Data.StatDict.TryGetValue(level + 1, out _stat))
                    break;
                if (_exp < _stat.totalExp)
                    break;
                level++;
            }

            if (_level != level)
            {
                _level = level;
                SetStat(_level);
            }
        }
    }

    private void Start()
    {
        _level = 1;
        
        _def = 5;
        _moveSpeed = 5.0f;
        _gold = 0;
        _exp = 0;

        SetStat(_level);
    }

    public void SetStat(int level)
    {
        Dictionary<int, Data.Stat> dict = Managers.Data.StatDict;
        Data.Stat stat = dict[level];

        _hp = stat.maxHp;
        _maxHp = stat.maxHp;
        _atk = stat.attack;
    }

    protected override void OnDead(Stat attacker)
    {
        Debug.Log("Player Dead");
    }
}
