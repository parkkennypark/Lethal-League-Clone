using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Juicer : MonoBehaviour
{
    public Volume volume;

    public float invertSpeedThreshold;

    void OnEnable()
    {
        Ball.OnHitWall += OnBallHitWall;
        Ball.OnBallHit += OnBallHit;
    }

    void OnDisable()
    {
        Ball.OnHitWall -= OnBallHitWall;
        Ball.OnBallHit -= OnBallHit;
    }

    void OnBallHitWall(float speed)
    {

    }

    void OnBallHit(int newSpeed, int maxSpeed, float hitRatio, float delay)
    {
        StartCoroutine(DoChromaticAberration(delay, hitRatio));
    }

    IEnumerator DoChromaticAberration(float delay, float intensity)
    {
        ChromaticAberration chr;
        volume.profile.TryGet<ChromaticAberration>(out chr);

        float time = 0;

        while (time < delay)
        {
            float amt = (time / delay) * intensity;
            chr.intensity.Override(amt);

            time += Time.deltaTime;
            yield return null;
        }

        FilmGrain grain;
        volume.profile.TryGet<FilmGrain>(out grain);
        grain.intensity.Override(intensity);

        ColorAdjustments adjustments;
        volume.profile.TryGet<ColorAdjustments>(out adjustments);
        adjustments.hueShift.Override(intensity * -50);

        if (intensity < invertSpeedThreshold)
            yield break;

        ColorCurves curves;
        volume.profile.TryGet<ColorCurves>(out curves);
        curves.active = !curves.active;
    }
}
