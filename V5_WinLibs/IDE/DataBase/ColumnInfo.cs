using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V5_IDE._Class.DataBase {
    /// <summary>
    /// 字段信息
    /// </summary>
    public class ColumnInfo {
        private string _colorder;
        private string _columnName;
        private string _typeName = "";
        private string _length = "";
        private string _preci = "";
        private string _scale = "";
        private bool _isIdentity;
        private bool _ispk;
        private bool _cisNull;
        private string _defaultVal = "";
        private string _deText = "";

        /// <summary>
        /// 序号
        /// </summary>
        public string Colorder {
            set { _colorder = value; }
            get { return _colorder; }
        }
        /// <summary>
        /// 字段名
        /// </summary>
        public string ColumnName {
            set { _columnName = value; }
            get { return _columnName; }
        }
        /// <summary>
        /// 字段类型
        /// </summary>
        public string TypeName {
            set { _typeName = value; }
            get { return _typeName; }
        }
        /// <summary>
        /// 长度
        /// </summary>
        public string Length {
            set { _length = value; }
            get { return _length; }
        }
        /// <summary>
        /// 精度
        /// </summary>
        public string Preci {
            set { _preci = value; }
            get { return _preci; }
        }
        /// <summary>
        /// 小数位数
        /// </summary>
        public string Scale {
            set { _scale = value; }
            get { return _scale; }
        }
        /// <summary>
        /// 是否是标识列
        /// </summary>
        public bool IsIdentity {
            set { _isIdentity = value; }
            get { return _isIdentity; }
        }
        /// <summary>
        /// 是否是主键
        /// </summary>
        public bool IsPK {
            set { _ispk = value; }
            get { return _ispk; }
        }
        /// <summary>
        /// 是否允许空
        /// </summary>
        public bool cisNull {
            set { _cisNull = value; }
            get { return _cisNull; }
        }
        /// <summary>
        /// 默认值
        /// </summary>
        public string DefaultVal {
            set { _defaultVal = value; }
            get { return _defaultVal; }
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string DeText {
            set { _deText = value; }
            get { return _deText; }
        }

    }
}
