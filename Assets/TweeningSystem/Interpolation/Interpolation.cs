// --~~~~======# Author : Lupon Dylan #======~~~~~~--- //
// --~~~~======# Date   : 03 / 08 / 2025 #======~~~~-- //

namespace UnBocal.TweeningSystem.Interpolations
{
    public struct Interpolation<ValueType>
    {
        // -------~~~~~~~~~~================# // Value
        public ValueType StartValue;
        public ValueType EndValue;

        // -------~~~~~~~~~~================# // Time
        public float Duration;
        public float Delay;

        // -------~~~~~~~~~~================# // Ease
        public System.Func<float, float> EaseFunction;
    }
}