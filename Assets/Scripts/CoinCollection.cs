using TMPro;
using UnityEngine;

public class CoinCollection : MonoBehaviour
{
	int score = 0;
	[SerializeField] TextMeshProUGUI scoreText;

	const string scorePrefix = "Score: ";

	private void Awake()
	{
		scoreText.text = scorePrefix + score;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Coin"))
		{
			Destroy(other.gameObject);
			++score;

			scoreText.text = scorePrefix + score;
		}
	}
}
