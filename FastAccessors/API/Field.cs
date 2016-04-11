namespace FastAccessors {
    using System;

    public static class Field {
        #region Instance-level
        public static object GetValue(object instance, Type type, string fieldName) {
            return FieldAccessor.GetFieldValue(instance, type, fieldName);
        }
        public static int Register(string fieldName, Type type) {
            return FieldAccessor.RegisterField(type, fieldName);
        }
        public static object GetValue(object instance, int key) {
            return FieldAccessor.GetFieldValue(instance, key);
        }
        #endregion
        #region Type-level
        public static object GetStaticValue(Type type, string fieldName) {
            return FieldAccessorStatic.GetFieldValue(type, fieldName);
        }
        public static int RegisterStatic(string fieldName, Type type, bool defaultField = false) {
            return FieldAccessorStatic.RegisterField(type, fieldName, defaultField);
        }
        public static object GetStaticValue(int key) {
            return FieldAccessorStatic.GetFieldValue(key);
        }
        public static object GetDefaultStaticValue() {
            return FieldAccessorStatic.GetFieldValue();
        }
        #endregion
        #region Instance-level(GenericWay)
        public static object GetValue<T>(this T instance, string fieldName) {
            return FieldAccessor<T>.GetFieldValue(instance, fieldName);
        }
        public static int Register<T>(string fieldName, bool defaultField = false) {
            return FieldAccessor<T>.RegisterField(fieldName, defaultField);
        }
        public static object GetValue<T>(this T instance, int fieldKey) {
            return FieldAccessor<T>.GetFieldValue(instance, fieldKey);
        }
        public static object GetDefaultValue<T>(this T instance) {
            return FieldAccessor<T>.GetFieldValue(instance);
        }
        #endregion
    }
}