using UnityEngine;

public class Card : MonoBehaviour
{
    // The material of front face quad in child object.
    // The front face object has tag "FrontFace".
    private Material frontfaceMaterial;

    void Awake()
    {
        // Get the Material component from child object with tag "FrontFace"
        for(int i=0;i<transform.childCount; ++i)
        {
            GameObject child = transform.GetChild(i).gameObject;
            if (child.tag.Equals("FrontFace"))
            {
                frontfaceMaterial = child.GetComponent<MeshRenderer>().material;
            }
        }
    }

    // Change the front face image of this card
    public void SetFrontFaceTexture(Texture2D texture)
    {
        Debug.Log(texture);
        frontfaceMaterial.mainTexture = texture;
    }
}
