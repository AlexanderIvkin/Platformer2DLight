using UnityEngine;

public class RotatorToDirection : MonoBehaviour
{
    [SerializeField] private float _scaleFactor;
    
    public void Rotate(float direction)
    {
        float rotateBound = 0.05f;

        if (Mathf.Abs(direction) >= rotateBound)
        {
            transform.localScale = new Vector3(Mathf.Sign(_scaleFactor * direction), transform.localScale.y, transform.localScale.z);
        }
    }
}
