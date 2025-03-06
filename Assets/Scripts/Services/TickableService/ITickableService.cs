using System.Collections.Generic;

namespace Services.TickableService
{
    public interface ITickableService
    {
        public List<TickableEntity> UpdateTickables { get; }
    
        public List<TickableEntity> FixedUpdateTickables { get; }

        public void Bootstrap();

        public void AddUpdateTickable(TickableEntity tickableEntity);
        public void RemoveUpdateTickable(TickableEntity tickableEntity);
    
        public void AddFixedUpdateTickable(TickableEntity tickableEntity);
        public void RemoveFixedUpdateTickable(TickableEntity tickableEntity);

        public void Update();
    
        public void FixedUpdate();
    }
}