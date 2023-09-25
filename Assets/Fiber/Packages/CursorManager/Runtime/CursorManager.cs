using System;
using UnityEngine;
using System.Collections.Generic;
using FiberUtils;
#if UNITY_WEBGL && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif


/* The CursorManager handles everything in regards to the cursor.
 * 
 * Building blocks in the CursorManager:
 * - Config / definition - define cursors and when they are available
 * - API - the API to interact with the CursorManager
 * - State - the state of the CursorManager, eg. which cursor should currently be displayed.
 * - Effect - the effect of state changes in the CursorManager, eg. change the actualy cursor being displayed.

- ICursorManagerAPI
    - WishCursor(int id, string name)
    - UnwishCursor(int id)

- CursorManager
    - SignalList<CursorDefinition> -> MASTER STATE
    - Implements ICursorManagerAPI
    - Implements Effect
    

- CursorManagerSO
    - List<CursorDefinition> -> Sets initial state in CursorManager
    - Holds CursorManager

- CursorManagerComponent
    - Prop: CursorManager -> Can be used if CursorManagerSO is used
    - Prop: SignalList<CursorDefinition> -> Updates state in CursorManager
    - Prop: EffectOverride - Is this needed? 
    - Provides CursorManager via context


------------------------------
- If global SO, no need to reimplement in component
- If component first -> need way to expose it in a none fiber way. Set SO value.

------------------------------
### CursorManager
    - WishCursor(int id, string name)
    - UnwishCursor(int id)
    - SetCursorDefinitions()
    - SignalList<CursorDefinition>
    - Handles setting the cursor internally

### CursorManagerSO
    - List<CursorDefinition>
    - Holds CursorManager

### CursorManagerComponent
    - Prop to set CursorManagerSO (from global scope)
    - SignalList<CursorDefinition> -> Updates cursor definitions in cursor manager
    - Exposes cursor manager via context

### InteractiveElement
    - Uses cursor manager


 */
namespace CursorManager
{
    [Serializable]
    public struct CursorDefinition
    {
        public string Name;
        public Texture2D Texture;
        public Vector2 Hotspot;

        public CursorDefinition(string name, Texture2D texture, Vector2 hotspot)
        {
            Name = name;
            Texture = texture;
            Hotspot = hotspot;
        }
    }


    [CreateAssetMenu(menuName = "Fiber/CursorManager", fileName = "CursorManager")]
    public class CursorManager : ScriptableObject
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern void SetBrowserCursor(string cursor);
#endif
        private const string DEFAULT_CURSOR = "auto";

        [SerializeField]
        private List<CursorDefinition> _cursorDefinitions = new();

        private IndexedDictionary<int, CursorWish> _cursorWishesById = new();
        private int _prio = 0;
        private int GetPrio()
        {
            _prio++;
            return _prio;
        }

        public struct CursorWish
        {
            public int Prio;
            public string Name;

            public CursorWish(int prio, string name)
            {
                Prio = prio;
                Name = name;
            }
        }

        public bool IsWishingCursor(int id)
        {
            return _cursorWishesById.ContainsKey(id);
        }

        public void WishCursor(int id, string name)
        {
            if (IsWishingCursor(id))
            {
                _cursorWishesById.SetByKey(id, new CursorWish(GetPrio(), name));
            }
            else
            {
                _cursorWishesById.Add(id, new CursorWish(GetPrio(), name));
            }
            UpdateCursor();
        }

        public void UnwishCursor(int id)
        {
            if (!_cursorWishesById.ContainsKey(id)) return;
            _cursorWishesById.Remove(id);
            UpdateCursor();
        }

        void UpdateCursor()
        {
            int pickedCursorWishKey = -1;
            int pickedCursorWishPrio = int.MaxValue;

            for (var i = 0; i < _cursorWishesById.Count; ++i)
            {
                var cursorWish = _cursorWishesById.GetKVPAt(i);
                if (cursorWish.Value.Prio < pickedCursorWishPrio)
                {
                    pickedCursorWishPrio = cursorWish.Value.Prio;
                    pickedCursorWishKey = cursorWish.Key;
                }
            }

            SetCursor(pickedCursorWishKey != -1 ? _cursorWishesById[pickedCursorWishKey].Name : DEFAULT_CURSOR);
        }

        void SetCursor(string name = DEFAULT_CURSOR)
        {
            for (var i = 0; i < _cursorDefinitions.Count; ++i)
            {
                var cursorDefinition = _cursorDefinitions[i];
                if (cursorDefinition.Name == name)
                {
                    var texture = cursorDefinition.Texture;
                    var hotspot = cursorDefinition.Hotspot;
                    Cursor.SetCursor(texture, hotspot, CursorMode.Auto);
                    return;
                }
            }

            // TODO: Set default cursor
        }
    }

}