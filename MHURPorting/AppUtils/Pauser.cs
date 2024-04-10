using System.Threading.Tasks;

namespace GGSTPorting.AppUtils;

public class Pauser
{
    public bool IsPaused;

    public void Pause()
    {
        IsPaused = true;
    }

    public void Unpause()
    {
        IsPaused = false;
    }

    public async Task WaitIfPaused()
    {
        while (IsPaused)
        {
            await Task.Delay(1);
        }
    }
}