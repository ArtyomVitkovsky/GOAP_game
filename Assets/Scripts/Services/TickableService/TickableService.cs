using System.Collections.Generic;

namespace Services.TickableService
{
    public class TickableService : ITickableService
    {
        public List<TickableEntity> UpdateTickables { get; private set; }
        public List<TickableEntity> FixedUpdateTickables { get; private set; }

        public void Bootstrap()
        {
            UpdateTickables = new List<TickableEntity>();
            FixedUpdateTickables = new List<TickableEntity>();
        }

        public void AddUpdateTickable(TickableEntity tickableEntity)
        {
            UpdateTickables ??= new List<TickableEntity>();
            UpdateTickables.Add(tickableEntity);
        }

        public void RemoveUpdateTickable(TickableEntity tickableEntity)
        {
            if (UpdateTickables.Contains(tickableEntity))
            {
                UpdateTickables.Remove(tickableEntity);
            }
        }

        public void AddFixedUpdateTickable(TickableEntity tickableEntity)
        {
            FixedUpdateTickables ??= new List<TickableEntity>();
            FixedUpdateTickables.Add(tickableEntity);
        }

        public void RemoveFixedUpdateTickable(TickableEntity tickableEntity)
        {
            if (FixedUpdateTickables.Contains(tickableEntity))
            {
                FixedUpdateTickables.Remove(tickableEntity);
            }
        }

        public void Update()
        {
            for (var i = 0; i < UpdateTickables.Count; i++)
            {
                var tickable = UpdateTickables[i];
                tickable.Update();
            }
        }

        public void FixedUpdate()
        {
            for (var i = 0; i < FixedUpdateTickables.Count; i++)
            {
                var tickable = FixedUpdateTickables[i];
                tickable.Update();
            }
        }
    }
}