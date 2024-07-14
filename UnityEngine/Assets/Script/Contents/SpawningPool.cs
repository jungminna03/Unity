using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class SpawningPool : MonoBehaviour
{
    [SerializeField]
    int _monsterCount = 0;

    [SerializeField]
    int _keepMonsterCount = 0;

    [SerializeField]
    Vector3 _spawnPos;

    [SerializeField]
    float _spawnRadius = 15.0f;

    [SerializeField]
    float _spawnTime = 5.0f;

    int _reserveCount = 0;

    private void Start()
    {
        Managers.Game.OnMonsterSpawnEvent -= AddMonsterCount;
        Managers.Game.OnMonsterSpawnEvent += AddMonsterCount;
    }

    private void Update()
    {
        while (_reserveCount + _monsterCount < _keepMonsterCount)
        {
            StartCoroutine("ReserveSpawn");
        }
    }

    void AddMonsterCount(int value) { _monsterCount += value; }
    public void SetKeepMonsterCount(int value) { _keepMonsterCount += value; }

    IEnumerator ReserveSpawn()
    {
        ++_reserveCount;
        yield return new WaitForSeconds(Random.Range(0, _spawnTime));
        GameObject obj = Managers.Game.Spawn(Define.WorldObject.Monster, "Knight");
        NavMeshAgent nma = obj.GetComponent<NavMeshAgent>();

        Vector3 randPos;

        while (true)
        {
            Vector3 randDir = Random.insideUnitSphere * _spawnRadius;
            randDir.y = 0;
            randPos = _spawnPos + randDir;

            NavMeshPath path = new NavMeshPath();
            if (nma.CalculatePath(randPos, path))
                break;
        }
        
        obj.transform.position = randPos;
        --_reserveCount;
    }
}
