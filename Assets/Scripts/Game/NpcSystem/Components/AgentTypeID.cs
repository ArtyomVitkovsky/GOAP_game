using UnityEngine.AI;

namespace Game.NpcSystem.Components
{
    public static class AgentTypeID
    {
        public static int GetAgentTypeIDByName(string agentTypeName)
        {
            int count = NavMesh.GetSettingsCount();
            string[] agentTypeNames = new string[count + 2];
            for (var i = 0; i < count; i++)
            {
                int id = NavMesh.GetSettingsByIndex(i).agentTypeID;
                string name = NavMesh.GetSettingsNameFromID(id);
                if(name == agentTypeName)
                {
                    return id;
                }
            }
            return -1;
        }
    }
}