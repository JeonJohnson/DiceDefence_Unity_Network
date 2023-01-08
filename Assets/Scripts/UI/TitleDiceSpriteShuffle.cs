using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleDiceSpriteShuffle : MonoBehaviour
{

    public List<Transform> sprites;



    // Start is called before the first frame update
    void Start()
    {
        sprites = Funcs.Shuffle(sprites);

        for (int i = 0; i < sprites.Count; ++i)
        {
            sprites[i].SetSiblingIndex(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
