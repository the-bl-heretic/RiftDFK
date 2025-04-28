#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace RiftDFK.EditorTools
{
    [CustomEditor(typeof(RiftDFK.RiftBody))]
    public class RiftBodyEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector(); // Draw normal RiftBody fields

            EditorGUILayout.Space(10);

            RiftDFK.RiftBody riftBody = (RiftDFK.RiftBody)target;

            if (GUILayout.Button("🔄 Update Rigidbody From RiftBody"))
            {
                riftBody.Editor_UpdateRigidbody();
            }
        }
    }
}
#endif
