public class SoundFragment
{
    public int Frequency { get; set; }
    public float Duration { get; set; }

    public SoundFragment(int frequency, float duration)
    {
        Frequency = frequency;
        Duration = duration;
    }
}
