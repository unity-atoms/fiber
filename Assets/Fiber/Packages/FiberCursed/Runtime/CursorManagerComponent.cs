using System.Collections.Generic;
using Signals;

namespace Fiber.Cursed
{
    public static partial class BaseComponentExtensions
    {
        public static CursorManagerComponent CursorManager(
            this BaseComponent component,
            Store<ShallowSignalList<CursorDefinition>> cursorDefinitionsStore,
            VirtualBody children
        )
        {
            return new CursorManagerComponent(
                cursorDefinitionsStore: cursorDefinitionsStore,
                children: children
            );
        }
    }

    public class CursorManagerComponent : BaseComponent
    {
        private readonly Store<ShallowSignalList<CursorDefinition>> _cursorDefinitionsStore;

        public CursorManagerComponent(
            Store<ShallowSignalList<CursorDefinition>> cursorDefinitionsStore,
            VirtualBody children
        ) : base(children)
        {
            _cursorDefinitionsStore = cursorDefinitionsStore;
        }

        public override VirtualBody Render()
        {
            CursorManager cursorManager = null;

            // Set value of CursorManagerSO if provided by global scope
            var cursorManagerSO = G<CursorManagerSO>(throwIfNotFound: false);
            var globalCursorManager = G<CursorManager>(throwIfNotFound: false);
            if (cursorManagerSO != null || globalCursorManager != null)
            {
                cursorManager = cursorManagerSO != null && cursorManagerSO.Manager != null ? cursorManagerSO.Manager : globalCursorManager;

                F.CreateEffect(() =>
                {
                    cursorManager.UpdateCursor();
                    return null;
                });
            }
            else
            {
                cursorManager = new CursorManager();
                // Update cursor definitions
                F.CreateEffect((cursorDefinitions) =>
                {
                    cursorManager.UpdateCursorDefinitions(cursorDefinitions);
                    return null;
                }, _cursorDefinitionsStore);
            }

            return F.ContextProvider(
                value: cursorManager,
                children: Children
            );
        }
    }
}