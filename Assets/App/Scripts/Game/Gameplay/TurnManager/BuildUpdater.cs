using System.Collections;
using System.Collections.Generic;
using App.System.Utils;

namespace App.Game.Gameplay
{
    public class BuildUpdater
    {
        readonly Dictionary<string, IBuild> playerBuilds = new Dictionary<string, IBuild>();

        public void Dispose()
        {

        }

        public void AddBuild(IBuild build)
        {
            playerBuilds.Add(build.GUID, build);
            build.OnDestroy += RemoveBuild;
        }

        public void RemoveBuild(IEntity entity)
        {
            playerBuilds.Remove(entity.GUID);
        }

        public void DoTurn()
        {
            foreach (KeyValuePair<string, IBuild> valuePair in playerBuilds)
            {
                if (valuePair.Value is IBuild build)
                {
                    build.Update();
                }
            }
        }
    }
}