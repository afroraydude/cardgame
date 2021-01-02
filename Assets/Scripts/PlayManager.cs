using UnityEngine;

namespace DefaultNamespace
{
    public class PlayManager : MonoBehaviour
    {
        public enum Actions
        {
            Heavy = 0,
            Shield = 1,
            Sword = 2,
        }
        
        [SerializeField]
        public int[] actionSet = {-1,-1,-1,-1,-1};

        void Start()
        {
        }
        
        public int energyPoints = 7;

        public bool InsertAction(int spot, int action)
        {
            if (spot == 4 && action == 0)
            {
                Debug.Log($"err1 {spot} {action}");
                return false;
            }

            if (spot - 1 >= 0 && (actionSet[spot - 1]) == 0)
            {
                Debug.Log($"err2 {spot} {action}");
                if (action == 0)
                {
                    actionSet[spot + 1] = -1;
                }
                return false;
            }

            if (!CheckEnergy(spot, action))
            {
                if (action == 0)
                {
                    actionSet[spot + 1] = -1;
                }
                return false;
            }
            
            actionSet[spot] = action;
            return true;
        }

        public bool CheckEnergy(int spot, int action)
        {
            int energy = energyPoints;
            // if spot was previously filled in
            if (actionSet[spot] != -1)
            {
                // restore points
                switch (actionSet[spot])
                {
                    case 0:
                        energy += 0;
                        break;
                    case 1:
                        energy += 1;
                        break;
                    case 2:
                        energy += 2;
                        break;
                }
            }
            
            switch (action)
            {
                case 0:
                    energy -= 0;
                    break;
                case 1:
                    energy -= 1;
                    break;
                case 2:
                    energy -= 2;
                    break;
            }
            if (energy < 0) return false;
            return true;
        }
        
        public void CalculateEnergy()
        {
            int energy = 7;
            for (int i = 0; i <= 4; i++)
            {
                int actionType = actionSet[i];
                switch (actionType)
                {
                    case 0:
                        energy -= 0;
                        break;
                    case 1:
                        energy -= 1;
                        break;
                    case 2:
                        energy -= 2;
                        break;
                }
            }
            this.energyPoints = energy;
        }
    }
}