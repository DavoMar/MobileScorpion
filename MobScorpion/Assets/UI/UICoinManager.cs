using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICoinManager : MonoBehaviour
{
    [SerializeField] private GameObject uiCoinPrefab;
    [SerializeField] float offset;

    private int numCoins = 0;
    private List<GameObject> uiCoins = new List<GameObject>();

    Vector3 lastP;
    Vector3 lastLP;

    /*
        void Start ()
        {
            lastP = transform.position;
            lastLP = transform.localPosition;
        }

        void Update ()
        {
            if (lastP != transform.position)
            {
                lastP = transform.position;
                print ("global: " + transform.position);
            }
            if (lastLP != transform.localPosition)
            {
                lastLP = transform.localPosition;
                print ("local: " + transform.localPosition);
            }
            print ();
        }
    */

    public void SetCoins(int num)
    {
        int numCoinsToDisplay = (int)Mathf.Ceil(((float)num) / 5f);
        int numCoinsCurrentlyDisplayed = (int)Mathf.Ceil(((float)numCoins) / 5f);

        if (numCoinsToDisplay > numCoinsCurrentlyDisplayed)
        {
            float y = transform.position.y;
            for (int i = 0; i < numCoinsToDisplay - numCoinsCurrentlyDisplayed; ++i)
            {
                GameObject uiCoin = Instantiate(uiCoinPrefab, transform.position, Quaternion.identity, transform);
                uiCoin.transform.localPosition += new Vector3(0f, ((float)numCoinsCurrentlyDisplayed + 0.7f) * offset, 0f);
                uiCoins.Add(uiCoin);
                ++numCoinsCurrentlyDisplayed;
            }
        }
        else
        {
            for (int i = numCoinsCurrentlyDisplayed - 1; i >= numCoinsToDisplay; --i)
            {
                if (i >= 0 && i < uiCoins.Count) // Ensure index is within bounds
                {
                    GameObject uiCoin = uiCoins[i];
                    uiCoins.RemoveAt(i);
                    Destroy(uiCoin);
                }
            }
        }

        numCoins = num;
    }

}