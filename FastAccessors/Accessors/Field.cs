﻿namespace FastAccessors {
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Reflection.Emit;
    using BF = System.Reflection.BindingFlags;
    using MA = System.Reflection.MethodAttributes;

    partial class Accessor {
        static int MakeKey(Type type, string fieldName) {
            unchecked { return type.GetHashCode() * 16777619 + fieldName.GetHashCode(); }
        }
        static class FieldAccessor {
            static IDictionary<string, Func<object, object>> accessors = new Dictionary<string, Func<object, object>>(StringComparer.Ordinal);
            static Func<object, object> defaultAccessor = _ => null;
            internal static object GetFieldValue(object instance, Type type, string fieldName) {
                Func<object, object> accessor;
                string key = type.Name + "." + fieldName;
                if(!accessors.TryGetValue(key, out accessor)) {
                    var field = type.GetField(fieldName, BF.Public | BF.NonPublic | BF.Instance);
                    accessor = (field != null) ? EmitFieldAccesssor(field, type) : defaultAccessor;
                    accessors.Add(key, accessor);
                }
                return accessor(instance);
            }
            static IDictionary<int, Func<object, object>> keyAccessors = new Dictionary<int, Func<object, object>>();
            internal static object GetFieldValue(object instance, int key) {
                return keyAccessors[key](instance);
            }
            internal static int RegisterField(Type type, string fieldName) {
                int key = MakeKey(type, fieldName);
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
                var ilGen = method.GetILGenerator();
                ilGen.Emit(OpCodes.Ldarg_0);
                ilGen.Emit(type.IsValueType ? OpCodes.Unbox : OpCodes.Castclass, type);
                ilGen.Emit(OpCodes.Ldfld, field);
                ilGen.Emit(OpCodes.Ret);
                return method.CreateDelegate(typeof(Func<object, object>)) as Func<object, object>;
            }
        }
        static class FieldAccessor<T> {
            static IDictionary<string, Func<T, object>> accessors = new Dictionary<string, Func<T, object>>(StringComparer.Ordinal);
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
            static IDictionary<int, Func<T, object>> keyAccessors = new Dictionary<int, Func<T, object>>();
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
                var ilGen = method.GetILGenerator();
                ilGen.Emit(OpCodes.Ldarg_0);
                ilGen.Emit(OpCodes.Ldfld, field);
                ilGen.Emit(OpCodes.Ret);
                return method.CreateDelegate(typeof(Func<T, object>)) as Func<T, object>;
            }
        }
        static class FieldAccessorStatic {
            static IDictionary<string, Func<object>> accessors = new Dictionary<string, Func<object>>(StringComparer.Ordinal);
            static Func<object> defaultAccessor = () => null;
            internal static object GetFieldValue(Type type, string fieldName) {
                Func<object> accessor;
                string key = type.Name + "." + fieldName;
                if(!accessors.TryGetValue(key, out accessor)) {
                    var field = type.GetField(fieldName, BF.Public | BF.NonPublic | BF.Static);
                    accessor = (field != null) ? EmitFieldAccesssor(field, type) : defaultAccessor;
                    accessors.Add(key, accessor);
                }
                return accessor();
            }
            static IDictionary<int, Func<object>> keyAccessors = new Dictionary<int, Func<object>>();
            static Func<object> defaultFieldAccessor = defaultAccessor;
            internal static object GetFieldValue(int key) {
                return keyAccessors[key]();
            }
            internal static object GetFieldValue() {
                return defaultFieldAccessor();
            }
            internal static int RegisterField(Type type, string fieldName, bool defaultField = false) {
                int key = MakeKey(type, fieldName);
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
                var ilGen = method.GetILGenerator();
                ilGen.Emit(OpCodes.Ldsfld, field);
                ilGen.Emit(OpCodes.Ret);
                return method.CreateDelegate(typeof(Func<object>)) as Func<object>;
            }
        }
    }
}