using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Script.Save
{
    [Serializable]
    public class SaveGameData
    {
        public List<int> UnlockedLevels = new List<int>();

        public bool AddCompletedLevel(int level)
        {
            if (IsLevelCompleted(level))
            {
                return false;
            }
            UnlockedLevels.Add(level);
            return true;
        }

        public bool IsLevelCompleted(int level)
        {
            return UnlockedLevels.Contains(level);
        }
    }
}
