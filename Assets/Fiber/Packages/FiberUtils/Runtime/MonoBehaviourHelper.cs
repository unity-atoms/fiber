using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

namespace FiberUtils
{
    public class MonoBehaviourHelper : MonoBehaviour
    {
        private static MonoBehaviourHelper _instance;
        private static IntIdGenerator _idGenerator;

        public static MonoBehaviourHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject go = new GameObject("MonoBehaviourHelper");
                    _instance = go.AddComponent<MonoBehaviourHelper>();
                    _idGenerator = new IntIdGenerator();
                }

                return _instance;
            }
        }

        private List<ValueTuple<int, Action<float>>> _onUpdateHandlers = new();
        private List<ValueTuple<int, Action<float>>> _onLateUpdateHandlers = new();
        private List<ValueTuple<int, Action>> _onFixedUpdateHandlers = new();

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            var deltaTime = Time.deltaTime;
            for (var i = 0; i < _onUpdateHandlers.Count; ++i)
            {
                _onUpdateHandlers[i].Item2.Invoke(deltaTime);
            }
        }

        private void LateUpdate()
        {
            var deltaTime = Time.deltaTime;
            for (var i = 0; i < _onLateUpdateHandlers.Count; ++i)
            {
                _onLateUpdateHandlers[i].Item2.Invoke(deltaTime);
            }
        }

        private void FixedUpdate()
        {
            for (var i = 0; i < _onFixedUpdateHandlers.Count; ++i)
            {
                _onFixedUpdateHandlers[i].Item2.Invoke();
            }
        }

        void OnDestroy()
        {
            foreach (var _corutine in _runningCoroutines)
            {
                StopCoroutine(_corutine.Value);
            }
        }

        public static int AddOnUpdateHandler(Action<float> handler)
        {
            var id = _idGenerator.NextId();
            if (Application.isPlaying)
            {
                Instance._onUpdateHandlers.Add(new ValueTuple<int, Action<float>>(id, handler));
            }
            else
            {
                void AddHandler(Scene scene, LoadSceneMode mode)
                {
                    SceneManager.sceneLoaded -= AddHandler;
                    Instance._onUpdateHandlers.Add(new ValueTuple<int, Action<float>>(id, handler));
                }

                SceneManager.sceneLoaded += AddHandler;
            }

            return id;
        }

        public static int AddOnLateUpdateHandler(Action<float> handler)
        {
            var id = _idGenerator.NextId();
            if (Application.isPlaying)
            {
                Instance._onLateUpdateHandlers.Add(new ValueTuple<int, Action<float>>(id, handler));
            }
            else
            {
                void AddHandler(Scene scene, LoadSceneMode mode)
                {
                    SceneManager.sceneLoaded -= AddHandler;
                    Instance._onLateUpdateHandlers.Add(new ValueTuple<int, Action<float>>(id, handler));
                }

                SceneManager.sceneLoaded += AddHandler;
            }

            return id;
        }

        public static int AddOnFixedUpdateHandler(Action handler)
        {
            var id = _idGenerator.NextId();
            if (Application.isPlaying)
            {
                Instance._onFixedUpdateHandlers.Add(new ValueTuple<int, Action>(id, handler));
            }
            else
            {
                void AddHandler(Scene scene, LoadSceneMode mode)
                {
                    SceneManager.sceneLoaded -= AddHandler;
                    Instance._onFixedUpdateHandlers.Add(new ValueTuple<int, Action>(id, handler));
                }

                SceneManager.sceneLoaded += AddHandler;
            }

            return id;
        }

        public static void RemoveOnUpdateHandler(int id)
        {
            if (_instance != null)
            {
                for (var i = _instance._onUpdateHandlers.Count - 1; i >= 0; --i)
                {
                    if (_instance._onUpdateHandlers[i].Item1 == id)
                    {
                        _instance._onUpdateHandlers.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        public static void RemoveOnLateUpdateHandler(int id)
        {
            if (_instance != null)
            {
                for (var i = _instance._onLateUpdateHandlers.Count - 1; i >= 0; --i)
                {
                    if (_instance._onLateUpdateHandlers[i].Item1 == id)
                    {
                        _instance._onLateUpdateHandlers.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        public static void RemoveOnFixedUpdateHandler(int id)
        {
            if (_instance != null)
            {
                for (var i = _instance._onFixedUpdateHandlers.Count - 1; i >= 0; --i)
                {
                    if (_instance._onFixedUpdateHandlers[i].Item1 == id)
                    {
                        _instance._onFixedUpdateHandlers.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        private Dictionary<int, Coroutine> _runningCoroutines = new Dictionary<int, Coroutine>();
        public int SetTimeout(Action callback, float delay)
        {
            var coroutineId = _idGenerator.NextId();
            var coroutine = StartCoroutine(SetTimeoutCoroutine(callback, delay, coroutineId));
            _runningCoroutines.Add(coroutineId, coroutine);
            return coroutineId;
        }

        public bool ClearTimeout(int coroutineId)
        {
            if (_runningCoroutines.ContainsKey(coroutineId))
            {
                StopCoroutine(_runningCoroutines[coroutineId]);
                _runningCoroutines.Remove(coroutineId);
                return true;
            }

            return false;
        }

        private IEnumerator SetTimeoutCoroutine(Action callback, float delay, int coroutineId)
        {
            yield return new WaitForSeconds(delay);
            callback();
            _runningCoroutines.Remove(coroutineId);
        }
    }
}