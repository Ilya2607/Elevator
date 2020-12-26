using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimElev
{
    class Elevator
    {
        private int level;
        private int nextLevel;
        private int coordinate;
        private int capacity;
        private int speed;
        private int acceleration;
        private readonly int[] arrayLevel;
        private int[] listLevel; 

        public Elevator(int spd, int cap, int lvl)
        {
            level = 1;
            nextLevel = 1;
            speed = spd;
            capacity = cap;
            acceleration = 1;
            arrayLevel = new int[lvl];
            for (int i = 0; i < lvl; i++)
            {
                arrayLevel[i] = 620 - 30 * i;
            }
            coordinate = arrayLevel[level - 1];
            listLevel = new int[capacity];
            for (int i = 0; i < capacity; i++)
            {
                listLevel[i] = 0;
            }
        }

        public int GetCoordinate()      {   return coordinate;      }
        public int GetLevel()           {   return level;           }
        public int GetNextLevel()       {   return nextLevel;       }
        public int GetSpeed()           {   return speed;           }
        public int GetAcceleration()    {   return acceleration;    }
        public void SetCoordinate(int coor)
        {
            coordinate = coor;
            for (int i = 0; i < arrayLevel.Length; i++)
            {
                if (Math.Abs(arrayLevel[i] - coordinate) / speed < 1.0)
                {
                    level = i + 1;
                }
            }
        }
        public void SetAcceleration(int acl)
        {
            acceleration = acl;
        }
        public void SetNextLevel(int lvl)
        {
            nextLevel = lvl;
        }
        public void AddLevel(int lvl)
        {
            for (int i = 0; i < capacity; i++)
            {
                if (listLevel[i] == 0)
                {
                    listLevel[i] = lvl;
                }
            }
        }
        public bool FreeSpace()
        {
            bool res = false;
            foreach(int unit in listLevel)
            {
                if (unit == 0)
                {
                    res = true;
                    break;
                }
            }
            return res;
        }        
    }
}
