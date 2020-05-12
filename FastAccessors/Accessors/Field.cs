namespace FastAccessors {
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Reflection.Emit;
    using BF = System.Reflection.BindingFlags;
    using MA = System.Reflection.MethodAttributes;

    static class FieldAccessor {
        static Dictionary<string, Func<object, object>> accessors = new Dictionary<string, Func<object, object>>(StringComparer.Ordinal);
        static Func<object, object> defaultAccessor = _ => null;
        internal static object GetFieldValue(object instance, Type type, string fieldName) {
            Func<object, object> accessor;
            string key = AccessorKey.GetKey(type, fieldName);
            if(!accessors.TryGetValue(key, out accessor)) {
                var field = type.GetField(fieldName, BF.Public | BF.NonPublic | BF.Instance);
                accessor = (field != null) ? EmitFieldAccesssor(field, type) : defaultAccessor;
                accessors.Add(key, accessor);
            }
            return accessor(instance);
        }
        static Dictionary<int, Func<object, object>> keyAccessors = new Dictionary<int, Func<object, object>>();
        internal static object GetFieldValue(object instance, int key) {
            return keyAccessors[key](instance);
        }
        internal static int RegisterField(Type type, string fieldName) {
            int key = AccessorKey.Make(type, fieldName);
            Func<object, object> accessor;
            if(!keyAccessors.TryGetValue(key, out accessor)) {
                var field = type.GetField(fieldName, BF.Public | BF.NonPublic | BF.Instance);
                accessor = (field != null) ? FieldAccessor.EmitFieldAccesssor(field, type) : defaultAccessor;
                keyAccessors.Add(key, accessor);
            }
            return key;
        }
        //
        readonly static Type[] accessorArgs = new Type[] { typeof(object) };
        static Func<object, object> EmitFieldAccesssor(System.Reflection.FieldInfo field, Type type) {
            var method = new DynamicMethod("__get_" + field.Name, MA.Static | MA.Public, CallingConventions.Standard,
                typeof(object), accessorArgs, typeof(FieldAccessor).Module, true);
            var ILGen = method.GetILGenerator();
            ILGen.Emit(OpCodes.Ldarg_0);
            ILGen.EmitUnboxOrCast(type);
            ILGen.Emit(OpCodes.Ldfld, field);
            ILGen.EmitBoxEndRet(field.FieldType);
            return method.CreateDelegate(typeof(Func<object, object>)) as Func<object, object>;
        }
    }
    static class FieldAccessor<T> {
        static Dictionary<string, Func<T, object>> accessors = new Dictionary<string, Func<T, object>>(StringComparer.Ordinal);
        static Func<T, object> defaultAccessor = _ => null;
        internal static object GetFieldValue(T instance, string fieldName) {
            Func<T, object> accessor;
            if(!accessors.TryGetValue(fieldName, out accessor)) {
                var field = type.GetField(fieldName, BF.Public | BF.NonPublic | BF.Instance);
                accessor = (field != null) ? EmitFieldAccesssor(field) : defaultAccessor;
                accessors.Add(fieldName, accessor);
            }
            return accessor(instance);
        }
        static Func<T, object> defaultFieldAccessor = defaultAccessor;
        static Dictionary<int, Func<T, object>> keyAccessors = new Dictionary<int, Func<T, object>>();
        internal static object GetFieldValue(T instance, int key) {
            return keyAccessors[key](instance);
        }
        internal static object GetFieldValue(T instance) {
            return defaultFieldAccessor(instance);
        }
        internal static int RegisterField(string fieldName, bool defaultField = false) {
            int key = fieldName.GetHashCode();
            Func<T, object> accessor;
            if(!keyAccessors.TryGetValue(key, out accessor)) {
                var field = type.GetField(fieldName, BF.Public | BF.NonPublic | BF.Instance);
                accessor = (field != null) ? EmitFieldAccesssor(field) : defaultAccessor;
                if(defaultField) defaultFieldAccessor = accessor;
                keyAccessors.Add(key, accessor);
            }
            return key;
        }
        //
        readonly static Type type = typeof(T);
        readonly static Type[] accessorArgs = new Type[] { typeof(T) };
        static Func<T, object> EmitFieldAccesssor(System.Reflection.FieldInfo field) {
            var method = new DynamicMethod("__get_" + field.Name, MA.Static | MA.Public, CallingConventions.Standard,
                typeof(object), accessorArgs, type, true);
            var ILGen = method.GetILGenerator();
            ILGen.Emit(OpCodes.Ldarg_0);
            ILGen.Emit(OpCodes.Ldfld, field);
            ILGen.EmitBoxEndRet(field.FieldType);
            return method.CreateDelegate(typeof(Func<T, object>)) as Func<T, object>;
        }
    }
    static class FieldAccessorStatic {
        static Dictionary<string, Func<object>> accessors = new Dictionary<string, Func<object>>(StringComparer.Ordinal);
        static Func<object> defaultAccessor = () => null;
        internal static object GetFieldValue(Type type, string fieldName) {
            Func<object> accessor;
            string key = AccessorKey.GetKey(type, fieldName);
            if(!accessors.TryGetValue(key, out accessor)) {
                var field = type.GetField(fieldName, BF.Public | BF.NonPublic | BF.Static);
                accessor = (field != null) ? EmitFieldAccesssor(field, type) : defaultAccessor;
                accessors.Add(key, accessor);
            }
            return accessor();
        }
        static Dictionary<int, Func<object>> keyAccessors = new Dictionary<int, Func<object>>();
        static Func<object> defaultFieldAccessor = defaultAccessor;
        internal static object GetFieldValue(int key) {
            return keyAccessors[key]();
        }
        internal static object GetFieldValue() {
            return defaultFieldAccessor();
        }
        internal static int RegisterField(Type type, string fieldName, bool defaultField = false) {
            int key = AccessorKey.Make(type, fieldName);
            Func<object> accessor;
            if(!keyAccessors.TryGetValue(key, out accessor)) {
                var field = type.GetField(fieldName, BF.Public | BF.NonPublic | BF.Static);
                accessor = (field != null) ? EmitFieldAccesssor(field, type) : defaultAccessor;
                if(defaultField) defaultFieldAccessor = accessor;
                keyAccessors.Add(key, accessor);
            }
            return key;
        }
        //
        static Func<object> EmitFieldAccesssor(System.Reflection.FieldInfo field, Type type) {
            var method = new DynamicMethod("__get_" + field.Name, MA.Static | MA.Public, CallingConventions.Standard,
                typeof(object), null, type, true);
            var ILGen = method.GetILGenerator();
            ILGen.Emit(OpCodes.Ldsfld, field);
            ILGen.EmitBoxEndRet(field.FieldType);
            return method.CreateDelegate(typeof(Func<object>)) as Func<object>;
        }
    }
    //
    static class BoxExtension {
        internal static void EmitUnboxOrCast(this ILGenerator ILGen, System.Type type) {
            ILGen.Emit(type.IsValueType ? OpCodes.Unbox : OpCodes.Castclass, type);
        }
        internal static void EmitBoxEndRet(this ILGenerator ILGen, System.Type type) {
            if(type.IsValueType) ILGen.Emit(OpCodes.Box, type);
            ILGen.Emit(OpCodes.Ret);
        }
    }
}