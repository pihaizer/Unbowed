﻿namespace Gameplay {
    public interface ISelectable {
        string GetName();

        bool CanBeSelected();
    }
}