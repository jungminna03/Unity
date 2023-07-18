using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : BaseController
{
    Stat _stat;

    [SerializeField]
    float _scanRange = 10.0f;

    [SerializeField]
    float _attackRange = 2f;

    public override void Init()
    {
        _stat = GetComponent<Stat>();
        anime = GetComponent<Animator>();

        if (GetComponentInChildren<UI_HPBar>() == null)
        {
            Managers.UI.MakeWorldSpaceUI<UI_HPBar>(transform);
        }

        WorldObjectType = Define.WorldObject.Monster;
    }

    protected override void UpdateIdle()
    {
        base.UpdateIdle();

        GameObject _player = Managers.Game.GetPlayer();
        if (_player == null)
        {
            return;
        }

        float distance = (_player.transform.position - transform.position).magnitude;

        if (distance <= _scanRange)
        {
            _target = _player;
            State = Define.State.Moving;
            return;
        }
    }

    protected override void UpdateMoving()
    {
        base.UpdateMoving();

        // 공격
        if (_target != null)
        {
            _destPos = _target.transform.position;
            float distance = (_destPos - transform.position).magnitude;
            if (distance <= _attackRange)
            {
                NavMeshAgent nma = gameObject.GetComponent<NavMeshAgent>();
                nma.SetDestination(transform.position);
                State = Define.State.Skill;
                return;
            }
        }

        // 이동
        Vector3 dir = _destPos - transform.position;

        if (dir.magnitude < 0.1f)
        {
            State = Define.State.Idle;
        }
        else
        {
            NavMeshAgent nma = gameObject.GetComponent<NavMeshAgent>();
            nma.SetDestination(_destPos);
            nma.speed = _stat.MoveSpead;

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);
        }
    }

    protected override void UpdateSkill()
    {
        base.UpdateSkill();

        if (_target != null)
        {
            Vector3 _dir = _target.transform.position - transform.position;

            Quaternion quat = Quaternion.LookRotation(_dir);

            transform.rotation = Quaternion.Lerp(transform.rotation, quat, 20 * Time.deltaTime);
        }
    }

    void OnHitEvent()
    {
        if (_target != null)
        {
            Stat targetStat = _target.GetComponent<Stat>();
            targetStat.OnAttacked(_stat);
        }
        else
            State = Define.State.Idle;
    }

    void OnHitEndEvent()
    {
        Stat targetStat = _target.GetComponent<Stat>();
        if (targetStat.HP > 0)
        {
            float distance = (_target.transform.position - transform.position).magnitude;

            if (distance <= _attackRange)
                State = Define.State.Skill;
            else
                State = Define.State.Moving;
        }
        else
            State = Define.State.Idle;
    }
}
