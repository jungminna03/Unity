using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    Define.CameraMode _mode;

    [SerializeField]
    Vector3 _delta;

    [SerializeField]
    GameObject _player;

    public void SetPlayer(GameObject player) { _player = player; }

    void LateUpdate()
    {
        if (!_player.IsValid())
            return;

        if (_mode == Define.CameraMode.QurterView)
        {
            RaycastHit hit;
            if (Physics.Raycast(_player.transform.position, _delta, out hit, _delta.magnitude, 1 << (int)Define.Layer.Block))
            {
                float dist = (hit.point - _player.transform.position).magnitude + 0.8f;
                transform.position = _player.transform.position + _delta.normalized * dist;
            }
            else
            {
                transform.position = _player.transform.position + _delta;
                transform.LookAt(_player.transform);
            }
        }
    }

    void SetQuterView(Vector3 m_position)
    {
        _player.transform.position = m_position;
        _mode = Define.CameraMode.QurterView;
    }


}
