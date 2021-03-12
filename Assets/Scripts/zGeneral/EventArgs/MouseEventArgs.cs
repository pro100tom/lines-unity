using UnityEngine;

namespace Lines.Scripts.General.EventArgs
{
    public class MouseEventArgs
    {
        public GameObject GameObject { get; }

        public MouseEventArgs(GameObject gameObject)
        {
            GameObject = gameObject;
        }
    }
}
