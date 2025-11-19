using UnityEngine;

public class DeathSplatterHandler : MonoBehaviour
{
    private void OnEnable()
    {
        Health.OnDeath += SpawnDeathVFX;
        Health.OnDeath += SpawnDeathSplatterPrefab;
    }

    private void OnDisable()
    {
        Health.OnDeath -= SpawnDeathVFX;
        Health.OnDeath -= SpawnDeathSplatterPrefab;
    }
    
    private void SpawnDeathSplatterPrefab(Health senderHealth)
    {
        var splatterInstance = Instantiate(senderHealth.SplatterPrefab, senderHealth.transform.position, Quaternion.identity);
        var colorChanger = senderHealth.gameObject.GetComponent<ColorChanger>();
        splatterInstance.GetComponent<SpriteRenderer>().color = colorChanger.DefaultColor;
        splatterInstance.transform.SetParent(this.transform);
    }

    private void SpawnDeathVFX(Health senderHealth)
    {
        var particlesInstance = Instantiate(senderHealth.DeathParticlesPrefab, senderHealth.transform.position, Quaternion.identity);
        var particles = particlesInstance.GetComponent<ParticleSystem>().main;
        var colorChanger = senderHealth.gameObject.GetComponent<ColorChanger>();
        particles.startColor = colorChanger.DefaultColor;
        particlesInstance.transform.SetParent(this.transform);
    }
}
