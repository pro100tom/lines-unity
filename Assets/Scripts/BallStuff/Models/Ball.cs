using Lines.Scripts.BallStuff.Styles;
using UnityEngine;

namespace Lines.Scripts.BallStuff.Models
{
    public class BallFactory
    {
        public GameObject Create(Vector3 position, Color32 color, BallStyle ballStyle)
        {
            var ball = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            ball.transform.localScale = Vector3.one * ballStyle.UniformScale;
            ball.transform.position = position;
            ballStyle.Material.color = color;
            ball.GetComponent<Renderer>().material = ballStyle.Material;

            return ball;
        }
    }
}
