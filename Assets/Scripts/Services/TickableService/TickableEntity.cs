using System;

namespace Services.TickableService
{
    public class TickableEntity
    {
        private Action action;
    
        public TickableEntity(Action action)
        {
            this.action = action;
        }
    
        public void Update()
        {
            action?.Invoke();
        }
    }
}