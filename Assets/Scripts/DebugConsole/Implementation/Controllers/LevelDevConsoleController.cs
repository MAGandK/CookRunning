using Level;
using SRDebugger;
using UnityEngine;
using Zenject;

namespace DebugConsole.Controllers
{
    public class LevelDevConsoleController : IDevConsoleController, ITickable
    {
        private const string CategoryName = "Level";
        
        private readonly ILevelLoader _levelLoader;

        public int CroupPriority => 0;

        public LevelDevConsoleController(ILevelLoader levelLoader)
        {
            _levelLoader = levelLoader;
        }
        

        public void Tick()
        {
            LoadLevels();
        }

        private void LoadLevels()
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                _levelLoader.LoadNextLevel();
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                _levelLoader.LoadCurrentLevel();
            }
        }

        public void Init()
        {
            var optionDefinition = OptionDefinition.FromMethod("Next level", ()=> _levelLoader.LoadNextLevel(), CategoryName);
            SRDebug.Instance.AddOption(optionDefinition);
            
            var optionPrevDefinition = OptionDefinition.FromMethod("Reload level", ()=> _levelLoader.LoadCurrentLevel(), CategoryName);
            SRDebug.Instance.AddOption(optionPrevDefinition);
        }
    }
}