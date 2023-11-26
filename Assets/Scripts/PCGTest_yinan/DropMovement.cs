using UnityEngine;

namespace Dropping2D
{
    public class DropMovement : MonoBehaviour
    {
        private Vector3 dampingVelocity;
        private Vector3 dampingScale;
        private Vector2 impact;
        private Vector2 scale;
        private Vector2 originalScale;

        public Vector2 Movement => impact;

        private void Update()
        {

            impact = Vector3.SmoothDamp(impact, Vector3.zero, ref dampingVelocity, 1f);
            scale = Vector3.SmoothDamp(scale, originalScale, ref dampingScale, 2f);
            if (impact.sqrMagnitude < 0.8f * 0.8f)
            {
                impact = Vector3.zero;
                scale = originalScale;
            }

            transform.Translate(Movement * Time.deltaTime);
            transform.localScale = scale;
        }

        public void AddForce(Vector2 force)
        {
            impact += force;
        }

        public void AddScaleEffect(Vector2 scale)
        {
            originalScale = transform.localScale;
            this.scale = originalScale + scale;
        }
    }
}