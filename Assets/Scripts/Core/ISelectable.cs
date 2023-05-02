using Barista.Core;

namespace Barista
{
    public interface ISelectable
    {
        public void Select();
        public void Deselect();
        public void DoAction();
        public void Refil();
        public ItemState State();
    }
}
