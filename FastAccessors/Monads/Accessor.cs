namespace FastAccessors.Monads {
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Extension methods providing IL-emitted field access via Unicode shorthand identifiers
    /// (<c>@ƒ</c>, <c>@ƒs</c>, <c>@ƒRegister</c>, …).
    /// Mirrors the API of <see cref="FastAccessors.Field"/> in a fluent extension-method style.
    /// </summary>
    public static partial class @Accessor {
        /// <summary>
        /// Gets the value of an instance field identified by name from an untyped object reference.
        /// The accessor is emitted once via IL and cached internally for subsequent calls.
        /// </summary>
        /// <param name="instance">The object instance whose field value is to be read.</param>
        /// <param name="type">The <see cref="Type"/> that declares the field.</param>
        /// <param name="fieldName">The name of the field to read.</param>
        /// <returns>
        /// The current value of the field, boxed to <see cref="object"/>.
        /// Returns <see langword="null"/> if no field with the given name exists on <paramref name="type"/>.
        /// </returns>
        [DebuggerStepThrough]
        public static object @ƒ(this object instance, Type type, string fieldName) {
            return FieldAccessor.GetFieldValue(instance, type, fieldName);
        }
        /// <summary>
        /// Gets the value of an instance field using a pre-registered integer key from an untyped object reference.
        /// No name lookup is performed; this is faster than the by-name overload.
        /// </summary>
        /// <param name="instance">The object instance whose field value is to be read.</param>
        /// <param name="key">The integer key returned by <see cref="ƒRegister(string,Type)"/>.</param>
        /// <returns>The current value of the field, boxed to <see cref="object"/>.</returns>
        [DebuggerStepThrough]
        public static object @ƒ(this object instance, int key) {
            return FieldAccessor.GetFieldValue(instance, key);
        }
        /// <summary>
        /// Gets the value of a static field identified by name, using the <see cref="Type"/> as the receiver.
        /// The accessor is emitted once via IL and cached internally for subsequent calls.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> that declares the static field.</param>
        /// <param name="fieldName">The name of the static field to read.</param>
        /// <returns>
        /// The current value of the static field, boxed to <see cref="object"/>.
        /// Returns <see langword="null"/> if no static field with the given name exists on <paramref name="type"/>.
        /// </returns>
        [DebuggerStepThrough]
        public static object @ƒs(this Type type, string fieldName) {
            return FieldAccessorStatic.GetFieldValue(type, fieldName);
        }
        /// <summary>
        /// Gets the value of a static field using a pre-registered integer key.
        /// No name lookup is performed; this is faster than the by-name overload.
        /// </summary>
        /// <param name="key">The integer key returned by <see cref="ƒsRegister(string,Type,bool)"/>.</param>
        /// <returns>The current value of the static field, boxed to <see cref="object"/>.</returns>
        [DebuggerStepThrough]
        public static object @ƒs(int key) {
            return FieldAccessorStatic.GetFieldValue(key);
        }
        /// <summary>
        /// Gets the value of the default static field registered via
        /// <see cref="ƒsRegister(string,Type,bool)"/> with <c>defaultField: true</c>.
        /// This is the absolute fastest read path for static fields; no lookup of any kind is performed.
        /// </summary>
        /// <returns>
        /// The current value of the default static field, boxed to <see cref="object"/>.
        /// Returns <see langword="null"/> if no default static field has been registered.
        /// </returns>
        [DebuggerStepThrough]
        public static object @ƒsDefault() {
            return FieldAccessorStatic.GetDefaultFieldValue();
        }
        /// <summary>
        /// Gets the value of an instance field identified by name on a strongly-typed instance.
        /// The accessor is emitted once via IL and cached; the instance is not boxed.
        /// </summary>
        /// <typeparam name="T">The type of the instance. May be a class or a struct.</typeparam>
        /// <param name="instance">The instance whose field value is to be read.</param>
        /// <param name="fieldName">The name of the field to read.</param>
        /// <returns>
        /// The current value of the field, boxed to <see cref="object"/>.
        /// Returns <see langword="null"/> if no field with the given name exists on <typeparamref name="T"/>.
        /// </returns>
        [DebuggerStepThrough]
        public static object @ƒ<T>(this T instance, string fieldName) {
            return FieldAccessor<T>.GetFieldValue(instance, fieldName);
        }
        /// <summary>
        /// Gets the value of an instance field using a pre-registered integer key on a strongly-typed instance.
        /// No name lookup is performed and the instance is not boxed.
        /// </summary>
        /// <typeparam name="T">The type of the instance. May be a class or a struct.</typeparam>
        /// <param name="instance">The instance whose field value is to be read.</param>
        /// <param name="fieldKey">The integer key returned by <see cref="ƒRegister{T}(string,bool)"/>.</param>
        /// <returns>The current value of the field, boxed to <see cref="object"/>.</returns>
        [DebuggerStepThrough]
        public static object @ƒ<T>(this T instance, int fieldKey) {
            return FieldAccessor<T>.GetFieldValue(instance, fieldKey);
        }
        /// <summary>
        /// Gets the value of the default instance field registered via
        /// <see cref="ƒRegister{T}(string,bool)"/> with <c>defaultField: true</c> on a strongly-typed instance.
        /// This is the absolute fastest read path for instance fields; no lookup of any kind is performed.
        /// </summary>
        /// <typeparam name="T">The type of the instance. May be a class or a struct.</typeparam>
        /// <param name="instance">The instance whose default field value is to be read.</param>
        /// <returns>
        /// The current value of the default field, boxed to <see cref="object"/>.
        /// Returns <see langword="null"/> if no default field has been registered for <typeparamref name="T"/>.
        /// </returns>
        [DebuggerStepThrough]
        public static object ƒDefault<T>(this T instance) {
            return FieldAccessor<T>.GetDefaultFieldValue(instance);
        }
        /// <summary>
        /// Registers an instance field by name for a given type and returns an integer key
        /// that can be cached and passed to <see cref="ƒ(object,int)"/> for the fastest repeated access.
        /// </summary>
        /// <param name="fieldName">The name of the instance field. The field may be private, public, or readonly.</param>
        /// <param name="type">The <see cref="Type"/> that declares the field.</param>
        /// <returns>An <see cref="int"/> key that uniquely identifies the field accessor.</returns>
        [DebuggerStepThrough]
        public static int @ƒRegister(this string fieldName, Type type) {
            return FieldAccessor.RegisterField(type, fieldName);
        }
        /// <summary>
        /// Registers a static field by name for a given type and returns an integer key.
        /// Optionally designates the field as the default static field, enabling access via <see cref="ƒsDefault"/>.
        /// </summary>
        /// <param name="fieldName">The name of the static field. The field may be private or public.</param>
        /// <param name="type">The <see cref="Type"/> that declares the static field.</param>
        /// <param name="defaultField">
        /// When <see langword="true"/>, designates this field as the default accessible via <see cref="ƒsDefault"/>.
        /// A subsequent call with <see langword="true"/> replaces the previous default.
        /// </param>
        /// <returns>An <see cref="int"/> key that can be passed to <see cref="ƒs(int)"/> for key-based access.</returns>
        [DebuggerStepThrough]
        public static int @ƒsRegister(this string fieldName, Type type, bool defaultField = false) {
            return FieldAccessorStatic.RegisterField(type, fieldName, defaultField);
        }
        /// <summary>
        /// Registers an instance field by name for the type <typeparamref name="T"/> and returns an integer key.
        /// Optionally designates the field as the default field for <typeparamref name="T"/>,
        /// enabling access via <see cref="ƒDefault{T}(T)"/>.
        /// </summary>
        /// <typeparam name="T">The type that declares the field. May be a class or a struct.</typeparam>
        /// <param name="fieldName">The name of the instance field. The field may be private, public, or readonly.</param>
        /// <param name="defaultField">
        /// When <see langword="true"/>, designates this field as the default for <typeparamref name="T"/>.
        /// A subsequent call with <see langword="true"/> replaces the previous default.
        /// </param>
        /// <returns>An <see cref="int"/> key that can be passed to <see cref="ƒ{T}(T,int)"/> for key-based access.</returns>
        [DebuggerStepThrough]
        public static int @ƒRegister<T>(this string fieldName, bool defaultField = false) {
            return FieldAccessor<T>.RegisterField(fieldName, defaultField);
        }
    }
}
