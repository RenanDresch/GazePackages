using Gaze.Utilities;

namespace Gaze.MCS
{
    public class Service
    {
        protected readonly IDestroyable Destroyable;

        public Service(IDestroyable destroyable)
        {
            Destroyable = destroyable;
        }
    }
}
