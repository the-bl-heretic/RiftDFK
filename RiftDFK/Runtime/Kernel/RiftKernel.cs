using System.Collections.Generic;
using UnityEngine;

namespace RiftDFK
{
    public class RiftKernel : MonoBehaviour
    {
        private static List<RiftBody> bodies = new List<RiftBody>();

        public static void Register(RiftBody body)
        {
            if (!bodies.Contains(body))
                bodies.Add(body);
        }

        public static void Unregister(RiftBody body)
        {
            if (bodies.Contains(body))
                bodies.Remove(body);
        }

        private void FixedUpdate()
        {
            foreach (var body in bodies)
            {
                body.ApplyForces();
            }
        }
    }
}