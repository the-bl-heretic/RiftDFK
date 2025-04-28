using UnityEngine;
using UnityEditor;

namespace RiftDFK
{
    [CustomEditor(typeof(RiftKernel))]
    public class RiftKernelEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            if (GUILayout.Button("Force Update Bodies"))
            {
                RiftKernel kernel = (RiftKernel)target;
                kernel.SendMessage("FixedUpdate");
            }
        }
    }
}