using System;
using UnityEngine;

namespace Lines.Scripts.BallStuff.Behaviour
{
    public class Bounce : MonoBehaviour
    {
        public float BounceHeight { get; set; } = 4.0f;
        public bool IsBouncing { get; protected set; }

        protected void OnCollisionEnter(Collision collision)
        {
            if (!IsBouncing)
                return;

            BouncePerform();
        }

        public void BounceStart()
        {
            IsBouncing = true;
            BouncePerform();
        }

        public void BounceStop()
        {
            IsBouncing = false;
        }

        public void BounceToggle()
        {
            if (IsBouncing)
                BounceStop();
            else
                BounceStart();
        }

        protected void BouncePerform()
        {
            var rigidBody = GetComponent<Rigidbody>();
            if (Math.Abs(rigidBody.velocity.y) < 0.01f)
                rigidBody.velocity = new Vector3(0, BounceHeight, 0);
        }
    }
}
