// --~~~~======# Author : Lupon Dylan #======~~~~~~--- //
// --~~~~======# Date   : 03 / 11 / 2025 #======~~~~-- //

using UnBocal.TweeningSystem.Interfaces;

namespace UnBocal.TweeningSystem.Interpolations
{
    public static class InterpolationFactory
    {
        public static IInterpolator GetNew<InterpolatorType>() where InterpolatorType : IInterpolator
        {
            switch (typeof(InterpolatorType).Name)
            {
                case nameof(Position): return new Position();
                case nameof(Scale): return new Scale();
                case nameof(RotationFast): return new RotationFast();
                default: return default;
            }
        }
    }
}