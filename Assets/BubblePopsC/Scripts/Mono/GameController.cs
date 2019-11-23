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
//                    .Add(new InitializeBoardSystem(contexts))
                ;
        }

        private void InitWorld()
        {
            _systems.ActivateReactiveSystems();
            _systems.Initialize();
        }
    }
}