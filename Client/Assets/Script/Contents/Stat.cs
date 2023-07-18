using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat : MonoBehaviour
{
    [SerializeField]
    protected int _level;
    [SerializeField]
    protected int _hp;
    [SerializeField]
    protected int _maxHp;
    [SerializeField]
    protected int _atk;
    [SerializeField]
    protected int _def;
    [SerializeField]
    protected float _moveSpeed;

    public int Level { get { return _level; } set { _level = value; } }
    public int HP { get { return _hp; } set { _hp = value; } }
    public int MaxHp { get { return _maxHp; } set { _maxHp = value; } }
    public int Atk { get { return _atk; } set { _atk = value; } }
    public int Def { get { return _def; } set { _def = value; } }
    public float MoveSpead { get { return _moveSpeed; } set { _moveSpeed = value; } }

    private void Start()
    {
        _level = 1;
        _hp = 100;
        _maxHp = 100;
        _atk = 10;
        _def = 5;
        _moveSpeed = 5.0f;
    }

    public virtual void OnAttacked(Stat attacker)
    {
        int _damage = Mathf.Max(0, (attacker.Atk - Def));
        HP -= _damage;

        if (HP <= 0)
        {
            HP = 0;
            OnDead(attacker);
        }
    }

    protected virtual void OnDead(Stat attacker)
    {
        PlayerStat player = attacker as PlayerStat;
        if (player != null)
        {
            player.Exp += 15;
        }

        Managers.Game.Despawn(gameObject);
    }
}
