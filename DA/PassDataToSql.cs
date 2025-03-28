using System;
using System.Data;

namespace PegasusDataAccess
{
    
    public class PassDataToSql
    {
        public string ParaName;
        public string ParaValue;
        public ParaTypeList ParaType;
        public ParaDirectonList ParaDirection;
        public byte[] ParaValue1;

        public PassDataToSql(String ParaName1, String ParaValue1, ParaTypeList ParaType1, ParaDirectonList ParaDirecton1)
        {
            this.ParaName = ParaName1;
            this.ParaValue = ParaValue1;
            this.ParaDirection = ParaDirecton1;
            this.ParaType = ParaType1;
        }
        public PassDataToSql(String ParaName1, byte[] ParaValue1, ParaTypeList ParaType1, ParaDirectonList ParaDirecton1)
        {
            this.ParaName = ParaName1;
            this.ParaValue1 = ParaValue1;
            this.ParaDirection = ParaDirecton1;
            this.ParaType = ParaType1;
        }
        public PassDataToSql()
        {
        }

        public SqlDbType  GetParaType(ParaTypeList D)
        {
            switch (D)
            {
                case ParaTypeList.Nvarchar:
                    return SqlDbType.NVarChar ;
                case ParaTypeList.Float:
                    return SqlDbType.Float ;
                case ParaTypeList.Int:
                    return SqlDbType.Int ;
                case ParaTypeList.Date:
                    return SqlDbType.Date;
                case ParaTypeList.Bit:
                    return SqlDbType.Bit ;
                case ParaTypeList.Text:
                    return SqlDbType.Text;
                case ParaTypeList.Image:
                    return  SqlDbType .Image ;
                case ParaTypeList.DateTime :
                    return SqlDbType.DateTime ;
                default:
                    return SqlDbType .Text;
            }
        }

        public ParameterDirection GetParaDirection(ParaDirectonList D)
        {
            switch (D)
            {
                case ParaDirectonList.Input:
                    return ParameterDirection.Input;
                case ParaDirectonList.Output:
                    return ParameterDirection.Output;
                case ParaDirectonList.Both:
                    return ParameterDirection.InputOutput;
                case ParaDirectonList.Return:
                    return ParameterDirection.ReturnValue;
                default:
                    return ParameterDirection.Input;
            }
        }

        public enum ParaTypeList
        {
            Nvarchar = 0,
            Float = 1,
            Int = 2,
            Date = 3,
            Bit = 4,
            Text = 5,
            Image = 6,
            DateTime = 7
        }
        public enum ParaDirectonList
        {
            Input = 0,
            Output = 1,
            Both = 2,
            Return = 3
        }
    }
}