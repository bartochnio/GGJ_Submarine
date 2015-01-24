using UnityEngine;
using System.Collections;

public static class Extensions  {

    public static void SetColor(this GameObject o, string matName)
    {
        Material m = Resources.Load("Mats/" + matName, typeof(Material)) as Material;
        o.renderer.material = m;
    }
	
}
