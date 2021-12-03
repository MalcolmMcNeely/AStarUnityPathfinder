using Grid;
using UnityEngine.EventSystems;

namespace World
{
    public interface IWorldMessageTarget : IEventSystemHandler
    {
        void OnGridUnitClicked(GridUnit gridUnit);
    }
}