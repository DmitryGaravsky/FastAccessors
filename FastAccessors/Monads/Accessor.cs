namespace FastAccessors.Monads {
    using System;
    using System.Diagnostics;

    public static partial class @Accessor {
        /// <summary>@Accessor(object): Get Field Value</summary>
        [DebuggerStepThrough]
        public static object @ƒ(this object instance, Type type, string fieldName) {
            return FieldAccessor.GetFieldValue(instance, type, fieldName);
        }
        /// <summary>@Accessor(object): Get Field Value</summary>
        [DebuggerStepThrough]
        public static object @ƒ(this object instance, int key) {
            return FieldAccessor.GetFieldValue(instance, key);
        }
        /// <summary>@Accessor(Type): Get Static Field Value</summary>
        [DebuggerStepThrough]
        public static object @ƒs(this Type type, string fieldName) {
            return FieldAccessorStatic.GetFieldValue(type, fieldName);
        }
        /// <summary>@Accessor(Type): Get Static Field Value</summary>
        [DebuggerStepThrough]
        public static object @ƒs(int key) {
            return FieldAccessorStatic.GetFieldValue(key);
        }
        /// <summary>@Accessor(T): Get Default Static Field Value</summary>
        [DebuggerStepThrough]
        public static object @ƒsDefault() {
            return FieldAccessorStatic.GetDefaultFieldValue();
        }
        /// <summary>@Accessor(T): Get Field Value</summary>
        [DebuggerStepThrough]
        public static object @ƒ<T>(this T instance, string fieldName) {
            return FieldAccessor<T>.GetFieldValue(instance, fieldName);
        }
        /// <summary>@Accessor(T): Get Field Value</summary>
        [DebuggerStepThrough]
        public static object @ƒ<T>(this T instance, int fieldKey) {
            return FieldAccessor<T>.GetFieldValue(instance, fieldKey);
        }
        /// <summary>@Accessor(T): Get Default Field Value</summary>
        [DebuggerStepThrough]
        public static object ƒDefault<T>(this T instance) {
            return FieldAccessor<T>.GetDefaultFieldValue(instance);
        }
        //
        [DebuggerStepThrough]
        public static int @ƒRegister(this string fieldName, Type type) {
            return FieldAccessor.RegisterField(type, fieldName);
        }
        [DebuggerStepThrough]
        public static int @ƒsRegister(this string fieldName, Type type, bool defaultField = false) {
            return FieldAccessorStatic.RegisterField(type, fieldName, defaultField);
        }
        [DebuggerStepThrough]
        public static int @ƒRegister<T>(this string fieldName, bool defaultField = false) {
            return FieldAccessor<T>.RegisterField(fieldName, defaultField);
        }
    }
}