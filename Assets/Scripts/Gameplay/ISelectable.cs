namespace Unbowed.Gameplay {
    public interface ISelectable {
        string GetName();

        bool CanBeSelected();

        bool HasTargetUI();
    }
}