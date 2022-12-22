namespace FastAccessors {
    using System;
    using System.Runtime.CompilerServices;

    public static class Field {
        #region Instance-level
        public static int Register(string fieldName, Type type) {
            return FieldAccessor.RegisterField(type, fieldName);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static object GetValue(object instance, Type type, string fieldName) {
            return FieldAccessor.GetFieldValue(instance, type, fieldName);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static object GetValue(object instance, int key) {
            return FieldAccessor.GetFieldValue(instance, key);
        }
        #endregion
        #region Type-level
        public static int RegisterStatic(string fieldName, Type type) {
            return FieldAccessorStatic.RegisterField(type, fieldName, false);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static object GetStaticValue(Type type, string fieldName) {
            return FieldAccessorStatic.GetFieldValue(type, fieldName);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static object GetStaticValue(int key) {
            return FieldAccessorStatic.GetFieldValue(key);
        }
        public static int RegisterDefaultStatic(string fieldName, Type type) {
            return FieldAccessorStatic.RegisterField(type, fieldName, true);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static object GetDefaultStaticValue() {
            return FieldAccessorStatic.GetDefaultFieldValue();
        }
        #endregion
        #region Instance-level(GenericWay)
        public static int Register<T>(string fieldName) {
            return FieldAccessor<T>.RegisterField(fieldName, false);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static object GetValue<T>(this T instance, string fieldName) {
            return FieldAccessor<T>.GetFieldValue(instance, fieldName);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static object GetValue<T>(this T instance, int fieldKey) {
            return FieldAccessor<T>.GetFieldValue(instance, fieldKey);
        }
        public static int RegisterDefault<T>(string fieldName) {
            return FieldAccessor<T>.RegisterField(fieldName, true);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static object GetDefaultValue<T>(this T instance) {
            return FieldAccessor<T>.GetDefaultFieldValue(instance);
        }
        #endregion
    }
}