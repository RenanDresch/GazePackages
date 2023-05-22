using System;
using Gaze.Utilities;
using UnityEngine;

namespace Gaze.MCS
{
    public class Test : MonoBehaviour, IDestroyable
    {
        readonly IReactiveDictionary<Guid, IReactiveProperty<Vector3>>
            transformMap
                = new ReactiveDictionary<Guid,
                        IReactiveProperty<Vector3>>(16)
                    .WithCustomDefaultValueGetter(GetDefault);

        static IReactiveProperty<Vector3> GetDefault()
        {
            var defaultProperty = new ReactiveProperty<Vector3>(
                Vector3.one);
            defaultProperty.WithCustomComparer(
                GarbageFreeComparers.Vector3Comparer);
            return defaultProperty;
        }

        void Awake()
        {
            transformMap.SafeBindOnAddAction(this, OnAdd);
            var guid = Guid.NewGuid();
            Debug.Log(transformMap[guid].Value);
            transformMap.Add(Guid.NewGuid(), GetDefault());
        }

        void OnAdd(Guid id, IReactiveProperty<Vector3> reactivePosition)
        {
           Debug.Log(reactivePosition.Value);
        }

        void Update()
        {
            if (Input.anyKeyDown)
            {
                var guid = Guid.NewGuid();
                transformMap[guid].Value = Vector3.down;
            }
        }

        public event Action OnDestroyEvent;
        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
    
    public static class GarbageFreeComparers
    {
        public static bool SameAs(this Vector2 vectorA, Vector2 vectorB)
        {
            return Vector2Comparer(vectorA, vectorB);
        }
        
        public static bool SameAs(this Vector3 vectorA, Vector3 vectorB)
        {
            return Vector3Comparer(vectorA, vectorB);
        }
        
        public static bool Vector2Comparer(Vector2 vectorA, Vector2 vectorB)
        {
            return Mathf.Approximately(vectorA.x, vectorB.x) &&
                   Mathf.Approximately(vectorA.y, vectorB.y);
        }
        
        public static bool Vector3Comparer(Vector3 vectorA, Vector3 vectorB)
        {
            return Mathf.Approximately(vectorA.x, vectorB.x) &&
                   Mathf.Approximately(vectorA.y, vectorB.y) &&
                   Mathf.Approximately(vectorA.z, vectorB.z);
        }
    }
}
