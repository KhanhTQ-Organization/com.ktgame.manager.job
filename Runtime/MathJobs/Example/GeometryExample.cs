using Sirenix.OdinInspector;
using UnityEngine;

namespace com.ktgame.manager.job.math.geometry
{
    public class GeometryExample : MonoBehaviour
    {
        [SerializeField] private Transform target;

        [Button]
        public void Test()
        {
            var from = transform.position;
            var to = target.position;
            this.transform.rotation = Quaternion.AngleAxis(from.AngleTo(to), Vector3.forward);
        }
    }
}
