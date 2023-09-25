using UnityEngine;
using System.Collections.Generic;
using FiberUtils;

namespace CursorManager
{
    [CreateAssetMenu(menuName = "Fiber/CursorManager/CursorManagerSO", fileName = "CursorManagerSO")]
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
                _cursorManager.UpdateCursorDefinitions(_cursorDefinitions);
            }
        }
    }
}