using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Game.Services.LevelServiceMock
{
    public interface ILevelSelectorService : IService
    {
        LevelData SelectedLevelData { get; }
        IReadOnlyCollection<LevelData> LevelDatas { get; }
        void SetLevel(LevelData levelData);
        void SetLevel(int index);
    }
}
