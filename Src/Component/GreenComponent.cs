using System.Linq;
using GreenFight.Mod;
using RimWorld;
using Verse;

namespace GreenFight.Component
{
    public class GreenComponent : GameComponent
    {
        
        /** Количество загрузок в фабрику. */
        public int UploadCount = 0;
        
        // В Главном меню при создании новой игры. После выбора сценария.
        public GreenComponent(Game game)
        {
            Log.Message("Выбран сценарий для новой игры.");
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

            if (Find.TickManager.TicksGame % 500 == 0)
            {
                for (int i = 0; i < GreenMod.Settings.lightningCount; i++)
                {
                    var position = Find.CurrentMap.AllCells
                        .Where(x => x.Walkable(Find.CurrentMap) && !x.Roofed(Find.CurrentMap))
                        .RandomElement();
                    var lightningEvent = new WeatherEvent_LightningStrike(Find.CurrentMap, position);
                    Find.CurrentMap.weatherManager.eventHandler.AddEvent(lightningEvent);
                }
            }
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
            Log.Message("Приземление");
        }

        public override void ExposeData()
        {
            Scribe_Values.Look(ref UploadCount, "Value");
        }
    }
}