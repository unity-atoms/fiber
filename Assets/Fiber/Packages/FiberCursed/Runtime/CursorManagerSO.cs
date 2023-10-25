using UnityEngine;
using System.Collections.Generic;
using FiberUtils;

namespace Fiber.Cursed
{
    [CreateAssetMenu(menuName = "Fiber/Cursed/CursorManagerSO", fileName = "CursorManagerSO")]
    public class CursorManagerSO : ScriptableObject
    {
        [SerializeField]
        private List<CursorDefinition> _cursorDefinitions;

        [SerializeField]
        [FiberReadOnly]
        private CursorManager _cursorManager;
        public CursorManager Manager
        {
            get => _cursorManager;
            set => _cursorManager = value;
        }

        void OnEnable()
        {
            if (_cursorManager != null)
            {
                _cursorManager.Reset();
                _cursorManager.UpdateCursorDefinitions(_cursorDefinitions);
            }
        }
    }
}