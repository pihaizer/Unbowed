namespace Unbowed.Utility.Modifiers {
    public abstract class BaseModifier {
        public abstract int Priority { get; }

        public abstract void Apply(BaseModifiable baseModifiable);
    }
}