using UnityEngine;

namespace RiftDFK
{
    [RequireComponent(typeof(RiftBody))]
    public class GravityForce : MonoBehaviour
    {
        [Tooltip("Gravity force applied to the object every frame.")]
        public Vector3 Gravity = new Vector3(0, -9.81f, 0);

        private RiftBody body;

        private void Awake()
        {
            body = GetComponent<RiftBody>();
            if (body == null)
            {
                Debug.LogError($"❌ GravityForce on '{gameObject.name}' could not find a RiftBody component!");
            }
        }

        private void FixedUpdate()
{
    if (body != null && !body.IsKinematic)
    {
        if (Gravity == Vector3.zero)
        {
            // Optional: stop the object completely if no gravity is set
            body.Editor_SetVelocity(Vector3.zero);
        }
        else
        {
            Vector3 gravityForce = Gravity * body.Mass;
            body.ApplyForce(gravityForce);
        }
    }
}

    }
}
