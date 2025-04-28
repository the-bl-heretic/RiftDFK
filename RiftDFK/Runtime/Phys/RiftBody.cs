#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace RiftDFK
{
    [RequireComponent(typeof(Collider))]
    public class RiftBody : MonoBehaviour
    {
        public Vector3 Velocity { get; private set; }
        public Vector3 InitialVelocity { get; private set; }
        public float Mass = 1f;
        public Vector3 AccumulatedForce { get; private set; }
        public bool IsKinematic = false;
        public Vector3 Drag = new Vector3(0.1f, 0.1f, 0.1f);
        public Vector3 MaxVelocity = new Vector3(50f, 50f, 50f);

        private Vector3 _position;

       private void OnEnable()
{
    RiftKernel.Register(this);
    _position = transform.position;

    // 🛠 Disable built-in Unity Rigidbody physics if it exists
    Rigidbody rb = GetComponent<Rigidbody>();
    if (rb != null)
    {
        rb.isKinematic = true;
        rb.useGravity = false;
    }
}


        private void OnDisable()
        {
            RiftKernel.Unregister(this);
        }

        public void ApplyForce(Vector3 force)
{
    if (IsKinematic) return;

    if (float.IsNaN(force.x) || float.IsNaN(force.y) || float.IsNaN(force.z))
    {
        Debug.LogWarning($"⚠️ Tried to apply NaN force to {gameObject.name}!");
        return;
    }

    AccumulatedForce += force;
}


        public void ApplyTorque(Vector3 torque)
        {
            if (!IsKinematic)
            {
                transform.Rotate(torque * Time.fixedDeltaTime, Space.World);
            }
        }

        public void ApplyForces()
{
    if (IsKinematic) return;

    float deltaTime = Time.fixedDeltaTime;
    Vector3 acceleration = AccumulatedForce / Mass;

    // 🚀 Use RiftMath to calculate per-axis displacement
    float dx = RiftMath.SolveS(Velocity.x, acceleration.x, deltaTime);
    float dy = RiftMath.SolveS(Velocity.y, acceleration.y, deltaTime);
    float dz = RiftMath.SolveS(Velocity.z, acceleration.z, deltaTime);

    Vector3 displacement = new Vector3(dx, dy, dz);

    _position += displacement;
    transform.position = _position;

    // 🚀 Use RiftMath to update velocity
    float vx = RiftMath.SolveV(Velocity.x, acceleration.x, deltaTime);
    float vy = RiftMath.SolveV(Velocity.y, acceleration.y, deltaTime);
    float vz = RiftMath.SolveV(Velocity.z, acceleration.z, deltaTime);

    Velocity = new Vector3(vx, vy, vz);

    // Apply manual drag
    Velocity = Vector3.Scale(Velocity, Vector3.one - Drag * deltaTime);

    // Clamp max velocity
    Velocity = Vector3.ClampMagnitude(Velocity, MaxVelocity.magnitude);

    // Reset accumulated forces
    AccumulatedForce = Vector3.zero;
}




        public void ResetPhysics()
        {
            Velocity = Vector3.zero;
            InitialVelocity = Vector3.zero;
            AccumulatedForce = Vector3.zero;
            _position = transform.position;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + Velocity);
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + AccumulatedForce / Mass);
        }

        // 🛠 Editor-only methods grouped together cleanly:
        #if UNITY_EDITOR
        public void Editor_SetVelocity(Vector3 velocity)
        {
            Velocity = velocity;
        }

        public void Editor_SetInitialVelocity(Vector3 velocity)
        {
            InitialVelocity = velocity;
        }

        public void Editor_UpdateRigidbody()
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb == null)
            {
                Debug.LogWarning("⚠️ No Rigidbody found to update from RiftBody!");
                return;
            }

            Undo.RecordObject(rb, "Update Rigidbody from RiftBody");

            rb.mass = Mass;
            rb.drag = (Drag.x + Drag.y + Drag.z) / 3f;
            rb.angularDrag = 0.05f;
            rb.velocity = Velocity;
            rb.maxAngularVelocity = MaxVelocity.magnitude;

            Debug.Log("✅ Rigidbody updated from RiftBody!");
        }
        #endif
    }
}
