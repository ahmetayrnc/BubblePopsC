using BubblePopsC.Scripts.Systems.Initialize;
using BubblePopsC.Scripts.Systems.Input;
using BubblePopsC.Scripts.Systems.View;
using UnityEngine;

namespace BubblePopsC.Scripts.Mono
{
    public class GameController : MonoBehaviour
    {
        private Entitas.Systems _systems;
        private Contexts _contexts;

        // Start is called before the first frame update
        void Start()
        {
            _contexts = Contexts.sharedInstance;
            _systems = CreateSystems(_contexts);
            InitWorld();
        }

        private void Update()
        {
            _systems.Execute();
            _systems.Cleanup();
        }

        private static Entitas.Systems CreateSystems(Contexts contexts)
        {
            return new Feature("Systems")
                    //initialize
                    .Add(new InitializeBoardSystem(contexts))
                    .Add(new InitializeTilesSystem(contexts))

                    //input
                    .Add(new ProcessTouchDownSystem(contexts))
                    .Add(new ProcessTouchUpSystem(contexts))
                    .Add(new CleanupInputSystem(contexts))

                    //view
                    .Add(new AddViewSystem(contexts))
                    .Add(new GameEventSystems(contexts))
                ;
        }

        private void InitWorld()
        {
            _systems.ActivateReactiveSystems();
            _systems.Initialize();
        }
    }
}