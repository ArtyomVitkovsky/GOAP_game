using UnityEngine;
using Zenject;

namespace Services.TickableService
{
    public class TickProvider : MonoBehaviour
    {
        [Inject] private ITickableService tickableService;

        private void Update()
        {
            tickableService.Update();
        }

        private void FixedUpdate()
        {
            tickableService.FixedUpdate();
        }
    }
}