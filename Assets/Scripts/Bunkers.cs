using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bunkers : MonoBehaviour
{
    private AudioManager audioManager;
    public BunkerPart[] prefabs;
    public int rows = 5;
    public int columns = 11;

    private void Awake() {
        audioManager = FindObjectOfType<AudioManager>();

        for (int row = 0; row < this.rows; row++)
        {
            float width = 2.0f * (this.columns - 1);
            float hight = 2.0f * (this.rows - 1);

            Vector2 centering = new Vector2(-width * 2, -hight * 1.8f);
            Vector3 rowPosition = new Vector3(centering.x, centering.y, 0.0f);
            rowPosition.y += row * 8.0f;

            for (int col = 0; col < this.columns; col++)
            {
                BunkerPart bunkerPart = Instantiate(this.prefabs[row], this.transform);
                Vector3 position = new Vector3(centering.x, centering.y + (row / 2.05f), 0.0f);;
                position.x += col * 8.0f;
                bunkerPart.transform.localPosition = position;
            }
        }
    }
    public void ResetBunkers()
    {
        audioManager.PlaySfx("Bunkers");
        foreach (Transform bunkerPart in transform) {
            bunkerPart.gameObject.SetActive(true);
        }
    }
}
