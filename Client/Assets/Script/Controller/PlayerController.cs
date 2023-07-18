using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class PlayerController : BaseController
{
    PlayerStat _stat;
    bool _stopSkill = false;


    int _mask = (1 << (int)Define.Layer.Monster) | (1 << (int)Define.Layer.Ground);



    public override void Init()
    {
        anime = GetComponent<Animator>();

        Managers.Input.MouseEvent -= OnMouseEvent;
        Managers.Input.MouseEvent += OnMouseEvent;

        if (GetComponentInChildren<UI_HPBar>() == null)
        {
            Managers.UI.MakeWorldSpaceUI<UI_HPBar>(transform);
        }

        _stat = gameObject.GetComponent<PlayerStat>();

        WorldObjectType = Define.WorldObject.Player;
    }

    override protected void UpdateMoving()
    {
        // 공격
        if (_target != null)
        {
            float distance = (_destPos - transform.position).magnitude;
            if (distance < 1)
            {
                State = Define.State.Skill;
                return;
            }
        }

        // 이동
        Vector3 dir = _destPos - transform.position;
        dir.y = 0;

        if (dir.magnitude <= 0.1f)
        {
            State = Define.State.Idle;
        }
        else
        {
            Debug.DrawRay(transform.position + Vector3.up * 0.5f, new Vector3(dir.x, 0, dir.z).normalized, Color.green); 
            if (Physics.Raycast(transform.position + Vector3.up * 0.5f, new Vector3(dir.x, 0, dir.z), 1, LayerMask.GetMask("Block")))
            {
                if (Input.GetMouseButton(0) == false)
                {
                    State = Define.State.Idle;
                }
                return;
            }

            float moveDist = Mathf.Clamp(_stat.MoveSpead * Time.deltaTime, 0, dir.magnitude);
            transform.position += dir.normalized * moveDist;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
        }
    }

    override protected void UpdateSkill()
    {
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
    }

    void OnHitEndEvent()
    {
        if (_stopSkill)
        {
            State = Define.State.Idle;
        }
        else
        {
            State = Define.State.Skill;
        }
    }

    

    
    void OnMouseEvent(Define.MouseEvent evt)
    {
        switch (State)
        {
            case Define.State.Idle:
                OnMouseEvent_RunIdle(evt);
                break;
            case Define.State.Moving:
                OnMouseEvent_RunIdle(evt);
                break;
            case Define.State.Skill:
                {
                    if (evt == Define.MouseEvent.PointerUp)
                    {
                        _stopSkill = true;
                    }
                }
                break;
        }
    }

    void OnMouseEvent_RunIdle(Define.MouseEvent evt)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool rayCastHit = Physics.Raycast(ray, out hit, 100f, _mask);
        //Debug.DrawRay(Camera.main.transform.position, ray.direction * 100f, Color.red, 1.0f);

        switch (evt)
        {
            case Define.MouseEvent.PointerDown:
                {
                    if (rayCastHit)
                    {
                        _destPos = hit.transform.position;
                        State = Define.State.Moving;

                        _stopSkill = false;

                        if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
                        {
                            _target = hit.collider.gameObject;
                        }
                        else
                        {
                            _target = null;
                        }
                    }
                }
                break;
            case Define.MouseEvent.Press:
                {
                    if (_target == null && rayCastHit)
                        _destPos = hit.point;
                }
                break;
            case Define.MouseEvent.PointerUp:
                _stopSkill = true;
                break;
        }
    }
}
