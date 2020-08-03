using System;
using System.Collections.Generic;
using System.Linq;

namespace MMSComunication
{
    public class MMSVariable
    {
        private readonly string name;
        private DataTypes type;
        private string value;

        public MMSVariable(string name, DataTypes type, string value = "")
        {
            this.name = name;
            this.value = value;
            this.type = type;
        }
                
        public string Name => this.name;

        public string Value { get => this.value; set => this.value = value; }

        public DataTypes Type { get => this.type; set => this.type = value; }

        public override string ToString()
        {
            return "Name: " + this.Name + " Value: " + this.Value + "Type: " + this.type;
        }

        public static bool TryGetMmsVariables(byte[] data, out IReadOnlyList<MMSVariable>mmsVariables )
        {
            var result = new List<MMSVariable>();
            var variableName = false;
            var variablesFound = 0;

            if (data.Length > 20 && data[0] == 3)
            {
                var i = 0;

                byte[] newArray = new byte[data.Length - 20];
                Buffer.BlockCopy(data, 20, newArray, 0, newArray.Length);
                data = newArray;

                while (data != null)
                {
                    try
                    {
                        if (i > data.Length-1)
                        {
                            break;
                        }
                        if (data.Length <= 0)
                        {
                            break;
                        }

                        if (data[i] == (int)DataTypes.VariableName)
                        {
                            variableName = true;
                            i++;
                            continue;
                        }

                        if (variableName)
                        {
                            result.Add(new MMSVariable(System.Text.Encoding.ASCII.GetString(data, i+1, data[i]), DataTypes.Empty));
                            newArray = new byte[data.Length - (i+1 + data[i])];
                            Buffer.BlockCopy(data, i+1 + data[i], newArray, 0, newArray.Length);
                            data = newArray;
                            variablesFound++;
                            i = 0;
                            variableName = false;
                            continue;
                        }

                        i++;
                    }
                    catch (Exception)
                    {
                        break;                    
                    }

                }

                foreach (var mmsVariable in result)
                {
                    i = 0;
                    //var split = data.

                    foreach (var @byte in data)
                    {
                        if (TypeCast.CastIntToDataType(@byte) != DataTypes.Error && TypeCast.CastIntToDataType(@byte) != DataTypes.Empty)
                        {
                            newArray = new byte[data.Length - i];
                            Buffer.BlockCopy(data, i, newArray, 0, newArray.Length);
                            data = newArray;

                            mmsVariable.Type = TypeCast.CastIntToDataType(@byte);

                            newArray = new byte[data[1]];
                            Buffer.BlockCopy(data, 2, newArray, 0, newArray.Length);
                            
                            if (TryGetValueFromBytes(mmsVariable.Type, newArray, out var resultValue))
                            {
                                mmsVariable.Value = resultValue;
                                if (data.Length > 2+data[1])
                                {
                                    newArray = new byte[data.Length - (2+data[1])];
                                    Buffer.BlockCopy(data, 2 + data[1], newArray, 0, newArray.Length);
                                    data = newArray;
                                }
 
                                break;
                            }
                        }

                        i++;
                    }
                }

                mmsVariables = result;
                return true;
            }

            mmsVariables = null;
            return false;

            
        }

        public static bool TryGetValueFromBytes(DataTypes dataTypes, byte[] bytes, out string value)
        {
            value = null;
            try
            {
                switch (dataTypes)
                {
                    case DataTypes.Error:
                        value = null;
                        return false;
                    case DataTypes.Empty:
                        value = null;
                        return false;
                    case DataTypes.Array:
                        value = null;
                        return false;
                    case DataTypes.Struct:
                        value = null;
                        return false;
                    case DataTypes.Bool:                                               
                        value = BitConverter.ToBoolean(bytes, 0).ToString();
                        return true;
                    case DataTypes.BitString:
                        value = null;
                        return false;
                    case DataTypes.Int:
                        if (BitConverter.IsLittleEndian)
                        {
                            bytes = bytes.Reverse().ToArray();
                        }

                        while (bytes.Length != 4)
                        {
                            if (bytes.Length == 4)
                            {
                                break;
                            }

                            var newArray = new byte[bytes.Length + 1];
                            Buffer.BlockCopy(bytes, 0, newArray, 0, bytes.Length);
                            bytes = newArray;
                        }

                        value = BitConverter.ToInt16(bytes,0).ToString();
                        return true;
                    case DataTypes.Uint:
                        value = null;
                        return false;
                    case DataTypes.Float:
                        if (BitConverter.IsLittleEndian)
                        {
                            bytes = bytes.Reverse().ToArray();
                        }

                        value = BitConverter.ToSingle(bytes, 0).ToString();
                        return true;
                    case DataTypes.OctetStr:
                        value = null;
                        return false;
                    case DataTypes.VisibleStr:
                        value = null;
                        return false;
                    case DataTypes.Time:
                        value = null;
                        return false;
                    case DataTypes.Bcd:
                        value = null;
                        return false;
                    case DataTypes.BoolArray:
                        value = null;
                        return false;
                    case DataTypes.DateTime:
                        value = null;
                        return false;
                    case DataTypes.Double:
                        value = null;
                        return false;
                    default:
                        break;
                }
            }
            catch (Exception e)
            {

                throw;
            }
           
            return true;
        }
    }
}
