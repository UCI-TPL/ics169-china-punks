using UnityEngine;

using UnityEngine.UI;

public class UI_text : MonoBehaviour
{

    public Text UIText;

    private float timer;

    // Use this for initialization

    void Start()
    {

        timer = 0.0f;

    }

    // Update is called once per frame

    void Update()
    {

        timer += Time.deltaTime * 2;

        if (timer % 2 > 1.0f)

        {

            UIText.text = "";

        }

        else
        {

            UIText.text = "A HUGE WAVE OF ENEMIES IS APPROACHING!!!";

        }

    }

}