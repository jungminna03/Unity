using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace Data
{
    #region Stat

    [Serializable]
    public class Stat
    {
        public int level;
        public int maxHp;
        public int attack;
        public int totalExp;
    }

    [Serializable]
    public class StatData : ILoder<int, Stat>
    {
        public List<Stat> stats = new List<Stat>();

        // 딕셔너리를 만들어주는 함수
        public Dictionary<int, Stat> MakeDict()
        {
            Dictionary<int, Stat> dict = new Dictionary<int, Stat>();

            foreach (Stat stat in stats)
            {
                dict.Add(stat.level, stat);
            }
            return dict;
        }
    }


    #endregion
}