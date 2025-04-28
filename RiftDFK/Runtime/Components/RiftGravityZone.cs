using UnityEngine;

namespace RiftDFK
{
    public class RiftGravityZone : MonoBehaviour
    {
        public Vector3 CustomGravity = new Vector3(0, -20f, 0);

        private void OnTriggerStay(Collider other)
        {
            RiftBody body = other.GetComponent<RiftBody>();
            if (body != null)
            {
                body.ApplyForce(CustomGravity * body.Mass * Time.fixedDeltaTime);
            }
        }
    }
}