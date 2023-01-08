using System.Collections.Generic;

namespace App.Game.Services
{
    public interface ILevelSelectorService : IService
    {
        LevelData SelectedLevelData { get; }
        IReadOnlyCollection<LevelData> LevelDatas { get; }
        void SetLevel(LevelData levelData);
        void SetLevel(int index);
    }
}
