using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WhiteScreenController : MonoBehaviour
{
    Image image;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    public void SetOpacity(float k) {
        image.color = new Color(255, 255, 255, k);
    }
}
