//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using NHibernate.SqlTypes;
//using NHibernate;
//using System.Data;
//using NHibernate.UserTypes;

//namespace DustInTheWind.ActiveTime.PersistenceModule
//{
//    class SQLiteDateTime : IUserType
//    {
//        #region IUserType Members

//        public object Assemble(object cached, object owner)
//        {
//            return cached;
//        }

//        public object DeepCopy(object value)
//        {
//            var dt = (DateTime)value;
//            return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);
//        }

//        public object Disassemble(object value)
//        {
//            return String.Format("{0:yyyy'-'MM'-'dd' 'HH':'mm':'ss.fff}", value);
//        }

//        public new bool Equals(object x, object y)
//        {
//            return x.Equals(y);
//        }

//        public int GetHashCode(object x)
//        {
//            return x.GetHashCode();
//        }

//        public bool IsMutable
//        {
//            get { return false; }
//        }

//        public object NullSafeGet(IDataReader rs, string[] names, object owner)
//        {
//            string dateString = (string)NHibernateUtil.Date.NullSafeGet(rs, names[0]);
//            DateTime result = DateTime.ParseExact(dateString, "yyyy'-'MM'-'dd' 'HH':'mm':'ss.fff", CultureInfo.InvariantCulture.DateTimeFormat);

//            return result;
//        }

//        public void NullSafeSet(IDbCommand cmd, object value, int index)
//        {
//            if (value == null)
//            {
//                NHibernateUtil.String.NullSafeSet(cmd, null, index);
//                return;
//            }
//            value = Disassemble(value);
//            NHibernateUtil.String.NullSafeSet(cmd, value, index);
//        }

//        public object Replace(object original, object target, object owner)
//        {
//            return original;
//        }

//        /// <summary>
//        /// The type returned by NullSafeGet()
//        /// </summary>
//        public Type ReturnedType
//        {
//            get { return typeof(DateTime); }
//        }

//        /// <summary>
//        /// The SQL types for the columns mapped by this type.
//        /// </summary>
//        public SqlType[] SqlTypes
//        {
//            get
//            {
//                var types = new SqlType[1];
//                types[0] = new SqlType(DbType.Date);
//                return types;

//                //return new[] { NHibernateUtil.Date.SqlType }; 
//            }
//        }

//        #endregion
//    }
//}
