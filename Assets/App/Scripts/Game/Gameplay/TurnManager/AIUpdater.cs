using App.Game.Gameplay.AI;
using System.Collections.Generic;

namespace App.Game.Gameplay
{
    public class AIUpdater
    {
        readonly Dictionary<string, IAICharacter> aiCharacters = new Dictionary<string, IAICharacter>();

        public Dictionary<string, IAICharacter> AiCharacters => aiCharacters;

        public void Dispose()
        {

        }

        public void AddAICharacter(IAICharacter aiCharacter)
        {
            aiCharacters.Add(aiCharacter.GUID, aiCharacter);
            aiCharacter.OnDestroy += RemoveCharacter;
        }

        public void RemoveCharacter(IEntity entity)
        {
            var character = (IAICharacter)entity;
            aiCharacters.Remove(character.GUID);
        }

        public void DoTurn()
        {
            var pairs = new Dictionary<string, IAICharacter>(aiCharacters); 
            foreach (KeyValuePair<string, IAICharacter> valuePair in pairs)
            {
                var character = valuePair.Value;
                character.Update();
            }
        }
    }
}