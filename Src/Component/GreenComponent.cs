using Verse;

namespace GreenFight.Component
{
    public class GreenComponent : GameComponent
    {
        public int UploadCount = 0;
        
        // В Главном меню при создании новой игры. После выбора сценария.
        public GreenComponent(Game game)
        {
            
        }

        public GreenComponent()
        {
            
        }

        // Постоянно
        public override void GameComponentOnGUI()
        {
            base.GameComponentOnGUI();
        }
        
        // Постоянно
        public override void GameComponentUpdate()
        {
            base.GameComponentUpdate();
        }
        
        // Стандартная обработка тика.
        public override void GameComponentTick()
        {
            base.GameComponentTick();
        }
        

        public override void FinalizeInit()
        {
            base.FinalizeInit();
        }

        // После восстановления при сохранении.
        public override void LoadedGame()
        {
            base.LoadedGame();
            
        }

        // При первом запуске игры. Уже на карте. После приземления.
        public override void StartedNewGame()
        {
            base.StartedNewGame();
        }

        public override void ExposeData()
        {
            Scribe_Values.Look(ref UploadCount, "Value");
        }
    }
}