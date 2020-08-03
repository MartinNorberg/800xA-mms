namespace MMSComunication
{
    public enum DataTypes
    {
        Error = -1,
        Empty = 0,
        VariableName = 128,
        Array = 129,
        Struct = 130,
        Bool = 131,
        BitString = 132,
        Int = 133,
        Uint = 134,
        Float = 135,
        OctetStr = 137,
        VisibleStr = 138,
        Time = 140,
        Bcd = 141,
        BoolArray = 142,
        DateTime = 162,
        Double = 112,
    }

    public static class TypeCast
    {
        public static DataTypes CastIntToDataType(int type)
        {
            switch (type)
            {                
                case (int)DataTypes.Error:
                    return DataTypes.Error;
                case (int)DataTypes.Empty:
                    return DataTypes.Empty;
                case (int)DataTypes.Array:
                    return DataTypes.Array;
                case (int)DataTypes.Struct:
                    return DataTypes.Struct;
                case (int)DataTypes.Bool:
                    return DataTypes.Bool;
                case (int)DataTypes.BitString:
                    return DataTypes.BitString;
                case (int)DataTypes.Int:
                    return DataTypes.Int;
                case (int)DataTypes.Uint:
                    return DataTypes.Uint;
                case (int)DataTypes.Float:
                    return DataTypes.Float;
                case (int)DataTypes.OctetStr:
                    return DataTypes.OctetStr;
                case (int)DataTypes.VisibleStr:
                    return DataTypes.VisibleStr;
                case (int)DataTypes.Time:
                    return DataTypes.Time;
                case (int)DataTypes.Bcd:
                    return DataTypes.Bcd;
                case (int)DataTypes.BoolArray:
                    return DataTypes.BoolArray;
                case (int)DataTypes.DateTime:
                    return DataTypes.DateTime;
                case (int)DataTypes.Double:
                    return DataTypes.Double;
                default:
                    return DataTypes.Error;
            }
        }
    }
}
