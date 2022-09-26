using Gaze.Utilities;

namespace Gaze.MVVM.Services
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
