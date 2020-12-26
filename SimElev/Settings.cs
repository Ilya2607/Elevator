using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimElev
{
    public class Settings
    {
        private int LEVEL;
        private int NUM_OF_ELEVATOR;

        public Settings()
        {
            LEVEL = 20;
            NUM_OF_ELEVATOR = 5;
        }

        public Settings(int lvl, int num)
        {
            LEVEL = lvl;
            NUM_OF_ELEVATOR = num;
        }

        public int GetSettingLevel()
        {
            return LEVEL;
        }
        public int GetSettingNumOfElev()
        {
            return NUM_OF_ELEVATOR;
        }
    }
}
