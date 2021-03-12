using System;
using UnityEngine;
using System.Linq;
using Lines.Scripts.General.EventArgs;

namespace Lines.Scripts.General.Notifiers
{
    public class MouseEventsNotifier : MonoBehaviour
    {
        protected EventHandler<MouseEventArgs> mouseEnter, mouseDown, mouseUp, mouseExit;

        public event EventHandler<MouseEventArgs> MouseEnter
        {
            add {
                if (mouseEnter == null || !mouseEnter.GetInvocationList().Contains(value))
                    mouseEnter += value;
            }
            remove => mouseEnter -= value;
        }

        public event EventHandler<MouseEventArgs> MouseDown
        {
            add {
                if (mouseDown == null || !mouseDown.GetInvocationList().Contains(value))
                    mouseDown += value;
            }
            remove => mouseDown -= value;
        }

        public event EventHandler<MouseEventArgs> MouseUp
        {
            add {
                if (mouseUp == null || !mouseUp.GetInvocationList().Contains(value))
                    mouseUp += value;
            }
            remove => mouseUp -= value;
        }

        public event EventHandler<MouseEventArgs> MouseExit
        {
            add {
                if (mouseExit == null || !mouseExit.GetInvocationList().Contains(value))
                    mouseExit += value;
            }
            remove => mouseExit -= value;
        }

        protected void OnMouseEnter()
        {
            mouseEnter?.Invoke(this, new MouseEventArgs(gameObject));
        }

        protected void OnMouseDown()
        {
            mouseDown?.Invoke(this, new MouseEventArgs(gameObject));
        }

        protected void OnMouseUp()
        {
            mouseUp?.Invoke(this, new MouseEventArgs(gameObject));
        }

        protected void OnMouseExit()
        {
            mouseExit?.Invoke(this, new MouseEventArgs(gameObject));
        }
    }
}
