using UnityEngine;
namespace Dropping2D
{
    public class Drop : MonoBehaviour
    {
        public GameObject dropObject;
        [Tooltip("Negative values does not change the behaviour")]
        public Vector2 DropForce;
        [Tooltip("This is inclusive")]
        public int RandomMin;
        [Tooltip("This is exclusive")]
        public int RandomMax;

        public Vector2 ScaleForce;

        public void MakeDrop()
        {
            int randomNo = Random.Range(RandomMin, RandomMax);
            for (int i = 0; i < randomNo; i++)
            {
                var dropObjectInScene = Instantiate(dropObject, transform.position, Quaternion.identity);
                float dropX = Random.Range(DropForce.x, -DropForce.x);
                float dropY = Random.Range(DropForce.y, -DropForce.y);

                dropObjectInScene.GetComponent<DropMovement>().AddForce(new Vector2(dropX, dropY));
                dropObjectInScene.GetComponent<DropMovement>().AddScaleEffect(ScaleForce);
            }
        }
    }

}
