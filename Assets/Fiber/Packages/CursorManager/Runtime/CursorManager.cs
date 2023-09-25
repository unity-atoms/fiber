using System;
using UnityEngine;
using System.Collections.Generic;
using FiberUtils;
#if UNITY_WEBGL && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif

namespace CursorManager
{
    // Enum based on https://developer.mozilla.org/en-US/docs/Web/CSS/cursor
    public enum CursorType
    {
        Auto = 0,
        Default = 1,
        None = 2,
        ContextMenu = 3,
        Help = 4,
        Pointer = 5,
        Progress = 6,
        Wait = 7,
        Cell = 8,
        CrossHair = 9,
        Text = 10,
        VerticalText = 11,
        Alias = 12,
        Copy = 13,
        Move = 14,
        NoDrop = 15,
        NotAllowed = 16,
        Grab = 17,
        Grabbing = 18,
        AllScroll = 19,
        ColResize = 20,
        RowResize = 21,
        NResize = 22,
        EResize = 23,
        SResize = 24,
        WResize = 25,
        NeResize = 26,
        NwResize = 27,
        SeResize = 28,
        SwResize = 29,
        EwResize = 30,
        NsResize = 31,
        NeswResize = 32,
        NwseResize = 33,
        ZoomIn = 34,
        ZoomOut = 35,
    }

    [Serializable]
    public struct CursorDefinition
    {
        public CursorType Cursor;
        public Texture2D Texture;
        public Vector2 Hotspot;

        public CursorDefinition(CursorType cursor, Texture2D texture, Vector2 hotspot)
        {
            Cursor = cursor;
            Texture = texture;
            Hotspot = hotspot;
        }
    }

    public static class CursorManagerUtils
    {
        private static Dictionary<CursorType, string> _cursorTypeToStringMap = new()
        {
            { CursorType.Auto, "auto" },
            { CursorType.Default, "default" },
            { CursorType.None, "none" },
            { CursorType.ContextMenu, "context-menu" },
            { CursorType.Help, "help" },
            { CursorType.Pointer, "pointer" },
            { CursorType.Progress, "progress" },
            { CursorType.Wait, "wait" },
            { CursorType.Cell, "cell" },
            { CursorType.CrossHair, "crosshair" },
            { CursorType.Text, "text" },
            { CursorType.VerticalText, "vertical-text" },
            { CursorType.Alias, "alias" },
            { CursorType.Copy, "copy" },
            { CursorType.Move, "move" },
            { CursorType.NoDrop, "no-drop" },
            { CursorType.NotAllowed, "not-allowed" },
            { CursorType.Grab, "grab" },
            { CursorType.Grabbing, "grabbing" },
            { CursorType.AllScroll, "all-scroll" },
            { CursorType.ColResize, "col-resize" },
            { CursorType.RowResize, "row-resize" },
            { CursorType.NResize, "n-resize" },
            { CursorType.EResize, "e-resize" },
            { CursorType.SResize, "s-resize" },
            { CursorType.WResize, "w-resize" },
            { CursorType.NeResize, "ne-resize" },
            { CursorType.NwResize, "nw-resize" },
            { CursorType.SeResize, "se-resize" },
            { CursorType.SwResize, "sw-resize" },
            { CursorType.EwResize, "ew-resize" },
            { CursorType.NsResize, "ns-resize" },
            { CursorType.NeswResize, "nesw-resize" },
            { CursorType.NwseResize, "nwse-resize" },
            { CursorType.ZoomIn, "zoom-in" },
            { CursorType.ZoomOut, "zoom-out" },
        };

        public static string CursorTypeToString(CursorType cursorType)
        {
            if (_cursorTypeToStringMap.ContainsKey(cursorType) == false)
            {
                return null;
            }

            return _cursorTypeToStringMap[cursorType];
        }
    }

    public class CursorManager
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern void Fiber_SetBrowserCursor(string cursor);
#endif

        [SerializeField]
        [FiberReadOnly]
        private List<CursorDefinition> _cursorDefinitions = new();
        [SerializeField]
        [FiberReadOnly]
        private IndexedDictionary<int, CursorWish> _cursorWishesById = new();
        private int _prio = 0;
        private int GetPrio()
        {
            _prio++;
            return _prio;
        }


        private static IntIdGenerator _idGenerator = new IntIdGenerator();
        public int GetUniqueID()
        {
            return _idGenerator.NextId();
        }

        private struct CursorWish
        {
            public int Prio;
            public CursorType Cursor;

            public CursorWish(int prio, CursorType cursor)
            {
                Prio = prio;
                Cursor = cursor;
            }
        }

        public void UpdateCursorDefinitions(IList<CursorDefinition> cursorDefinitions)
        {
            _cursorDefinitions.Clear();

            for (var i = 0; cursorDefinitions != null && i < cursorDefinitions.Count; ++i)
            {
                _cursorDefinitions.Add(cursorDefinitions[i]);
            }

            UpdateCursor();
        }

        public void WishCursor(int id, CursorType cursor)
        {
            if (IsWishingCursor(id))
            {
                _cursorWishesById.SetByKey(id, new CursorWish(GetPrio(), cursor));
            }
            else
            {
                _cursorWishesById.Add(id, new CursorWish(GetPrio(), cursor));
            }
            UpdateCursor();
        }

        public void UnwishCursor(int id)
        {
            if (!_cursorWishesById.ContainsKey(id)) return;
            _cursorWishesById.Remove(id);
            UpdateCursor();
        }

        bool IsWishingCursor(int id)
        {
            return _cursorWishesById.ContainsKey(id);
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

            SetCursor(pickedCursorWishKey != -1 ? _cursorWishesById.GetByKey(pickedCursorWishKey).Cursor : CursorType.Default);
        }

        void SetCursor(CursorType cursor)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            var cursorString = CursorManagerUtils.CursorTypeToString(cursor);
            Fiber_SetBrowserCursor(cursorString);
#else
            for (var i = 0; i < _cursorDefinitions.Count; ++i)
            {
                var cursorDefinition = _cursorDefinitions[i];
                if (cursorDefinition.Cursor == cursor)
                {
                    var texture = cursorDefinition.Texture;
                    var hotspot = cursorDefinition.Hotspot;
                    Cursor.SetCursor(texture, hotspot, CursorMode.Auto);
                    return;
                }
            }

            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
#endif
        }
    }

}