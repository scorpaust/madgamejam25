using UnityEngine;


public class MenuSpotlight : MonoBehaviour
{
	[Header("Movement Settings")]
	[SerializeField] private float speed = 2f;
	[SerializeField] private float amplitude = 3f;
	[SerializeField] private float verticalOffset = 2f;

	[Header("Light Settings")]
	[SerializeField] private UnityEngine.Rendering.Universal.Light2D spotLight;
	[SerializeField] private float minIntensity = 2f;
	[SerializeField] private float maxIntensity = 5f;
	[SerializeField] private float intensitySpeed = 1.5f;

	private float startX;
	private float timeOffset;

	private void Start()
	{
		if (spotLight == null)
		{
			spotLight = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
		}

		startX = transform.position.x;
		timeOffset = Random.Range(0f, 100f); // Para variar entre luzes diferentes
	}

	private void Update()
	{
		// Movimento suave horizontal usando Sin()
		float x = startX + Mathf.Sin(Time.time * speed) * amplitude;
		float y = verticalOffset + Mathf.Cos(Time.time * speed * 0.5f) * (amplitude / 2);

		transform.position = new Vector3(x, y, transform.position.z);

		// Varia��o suave na intensidade da luz
		float intensity = Mathf.Lerp(minIntensity, maxIntensity, Mathf.PingPong(Time.time * intensitySpeed, 1));
		spotLight.intensity = intensity;
	}
}
