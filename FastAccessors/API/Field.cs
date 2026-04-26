namespace FastAccessors {
    using System;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Provides static methods for fast IL-emitted field access on both reference and value types.
    /// Fields can be private, public, or readonly, and are accessed without security restrictions.
    /// </summary>
    /// <remarks>
    /// Three access patterns are available, in increasing order of speed:
    /// <list type="number">
    ///   <item><description>By-name: no pre-registration required; the accessor is emitted on first call and cached.</description></item>
    ///   <item><description>Key-based: call a <c>Register</c> overload once at startup to obtain an <see cref="int"/> key, then pass the key on every subsequent access.</description></item>
    ///   <item><description>Default-field: designate one field per type as the default; subsequent reads require no name or key lookup at all.</description></item>
    /// </list>
    /// </remarks>
    public static class Field {
        #region Instance-level
        /// <summary>
        /// Registers an instance field by name and returns an integer key that can be cached
        /// and passed to <see cref="GetValue(object,int)"/> for the fastest repeated access.
        /// </summary>
        /// <param name="fieldName">The name of the instance field. The field may be private, public, or readonly.</param>
        /// <param name="type">The <see cref="Type"/> that declares the field.</param>
        /// <returns>
        /// An <see cref="int"/> key that uniquely identifies the field accessor.
        /// Pass this key to <see cref="GetValue(object,int)"/> to read the field value with no name-lookup overhead.
        /// </returns>
        public static int Register(string fieldName, Type type) {
            return FieldAccessor.RegisterField(type, fieldName);
        }
        /// <summary>
        /// Gets the value of an instance field identified by name.
        /// The accessor is emitted once via IL and cached; prefer <see cref="Register(string,Type)"/> +
        /// <see cref="GetValue(object,int)"/> when the same field is read repeatedly.
        /// </summary>
        /// <param name="instance">The object instance whose field value is to be read. May be a boxed value type.</param>
        /// <param name="type">The <see cref="Type"/> that declares the field.</param>
        /// <param name="fieldName">The name of the field to read.</param>
        /// <returns>
        /// The current value of the field, boxed to <see cref="object"/>.
        /// Returns <see langword="null"/> if no field with the given name exists on <paramref name="type"/>.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static object GetValue(object instance, Type type, string fieldName) {
            return FieldAccessor.GetFieldValue(instance, type, fieldName);
        }
        /// <summary>
        /// Gets the value of an instance field using a pre-registered integer key.
        /// This is the fastest non-generic instance-field read path: no name lookup is performed.
        /// </summary>
        /// <param name="instance">The object instance whose field value is to be read. May be a boxed value type.</param>
        /// <param name="key">The integer key returned by <see cref="Register(string,Type)"/>.</param>
        /// <returns>The current value of the field, boxed to <see cref="object"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static object GetValue(object instance, int key) {
            return FieldAccessor.GetFieldValue(instance, key);
        }
        #endregion
        #region Type-level
        /// <summary>
        /// Registers a static field by name and returns an integer key that can be cached
        /// and passed to <see cref="GetStaticValue(int)"/> for the fastest repeated access.
        /// </summary>
        /// <param name="fieldName">The name of the static field. The field may be private or public.</param>
        /// <param name="type">The <see cref="Type"/> that declares the static field.</param>
        /// <returns>
        /// An <see cref="int"/> key that uniquely identifies the static field accessor.
        /// Pass this key to <see cref="GetStaticValue(int)"/> to read the field value with no name-lookup overhead.
        /// </returns>
        public static int RegisterStatic(string fieldName, Type type) {
            return FieldAccessorStatic.RegisterField(type, fieldName, false);
        }
        /// <summary>
        /// Gets the value of a static field identified by name.
        /// The accessor is emitted once via IL and cached; prefer <see cref="RegisterStatic(string,Type)"/> +
        /// <see cref="GetStaticValue(int)"/> when the same field is read repeatedly.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> that declares the static field.</param>
        /// <param name="fieldName">The name of the static field to read.</param>
        /// <returns>
        /// The current value of the static field, boxed to <see cref="object"/>.
        /// Returns <see langword="null"/> if no static field with the given name exists on <paramref name="type"/>.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static object GetStaticValue(Type type, string fieldName) {
            return FieldAccessorStatic.GetFieldValue(type, fieldName);
        }
        /// <summary>
        /// Gets the value of a static field using a pre-registered integer key.
        /// This is the fastest static-field read path: no name lookup is performed.
        /// </summary>
        /// <param name="key">The integer key returned by <see cref="RegisterStatic(string,Type)"/> or <see cref="RegisterDefaultStatic(string,Type)"/>.</param>
        /// <returns>The current value of the static field, boxed to <see cref="object"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static object GetStaticValue(int key) {
            return FieldAccessorStatic.GetFieldValue(key);
        }
        /// <summary>
        /// Registers a static field as the default static field, returning an integer key.
        /// After registration, <see cref="GetDefaultStaticValue()"/> reads this field with no lookup overhead at all.
        /// Calling this method again replaces the previous default.
        /// </summary>
        /// <param name="fieldName">The name of the static field to designate as the default.</param>
        /// <param name="type">The <see cref="Type"/> that declares the static field.</param>
        /// <returns>
        /// An <see cref="int"/> key that also allows access via <see cref="GetStaticValue(int)"/>.
        /// </returns>
        public static int RegisterDefaultStatic(string fieldName, Type type) {
            return FieldAccessorStatic.RegisterField(type, fieldName, true);
        }
        /// <summary>
        /// Gets the value of the default static field registered via <see cref="RegisterDefaultStatic(string,Type)"/>.
        /// This is the absolute fastest read path for static fields: the accessor delegate is stored
        /// in a direct field reference with no lookup of any kind.
        /// </summary>
        /// <returns>
        /// The current value of the default static field, boxed to <see cref="object"/>.
        /// Returns <see langword="null"/> if no default static field has been registered.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static object GetDefaultStaticValue() {
            return FieldAccessorStatic.GetDefaultFieldValue();
        }
        #endregion
        #region Instance-level(GenericWay)
        /// <summary>
        /// Registers an instance field by name for the type <typeparamref name="T"/> and returns an integer key
        /// that can be cached and passed to <see cref="GetValue{T}(T,int)"/> for the fastest repeated access.
        /// Using the generic overload avoids boxing the instance on each read.
        /// </summary>
        /// <typeparam name="T">The type that declares the field. May be a class or a struct.</typeparam>
        /// <param name="fieldName">The name of the instance field. The field may be private, public, or readonly.</param>
        /// <returns>
        /// An <see cref="int"/> key that uniquely identifies the field accessor for <typeparamref name="T"/>.
        /// </returns>
        public static int Register<T>(string fieldName) {
            return FieldAccessor<T>.RegisterField(fieldName, false);
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static object GetValue<T>(this T instance, string fieldName) {
            return FieldAccessor<T>.GetFieldValue(instance, fieldName);
        }
        /// <summary>
        /// Gets the value of an instance field using a pre-registered integer key on a strongly-typed instance.
        /// No name lookup is performed and the instance is not boxed.
        /// </summary>
        /// <typeparam name="T">The type of the instance. May be a class or a struct.</typeparam>
        /// <param name="instance">The instance whose field value is to be read.</param>
        /// <param name="fieldKey">The integer key returned by <see cref="Register{T}(string)"/> or <see cref="RegisterDefault{T}(string)"/>.</param>
        /// <returns>The current value of the field, boxed to <see cref="object"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static object GetValue<T>(this T instance, int fieldKey) {
            return FieldAccessor<T>.GetFieldValue(instance, fieldKey);
        }
        /// <summary>
        /// Registers an instance field as the default field for the type <typeparamref name="T"/>, returning an integer key.
        /// After registration, <see cref="GetDefaultValue{T}(T)"/> reads this field with no lookup overhead at all.
        /// Only one default field per closed generic type <typeparamref name="T"/> is active at a time;
        /// calling this method again replaces the previous default.
        /// </summary>
        /// <typeparam name="T">The type that declares the field. May be a class or a struct.</typeparam>
        /// <param name="fieldName">The name of the field to designate as the default.</param>
        /// <returns>
        /// An <see cref="int"/> key that also allows access via <see cref="GetValue{T}(T,int)"/>.
        /// </returns>
        public static int RegisterDefault<T>(string fieldName) {
            return FieldAccessor<T>.RegisterField(fieldName, true);
        }
        /// <summary>
        /// Gets the value of the default instance field registered via <see cref="RegisterDefault{T}(string)"/> on a strongly-typed instance.
        /// This is the absolute fastest read path for instance fields: the accessor delegate is stored in a
        /// direct static field reference per closed generic type, with no lookup of any kind.
        /// </summary>
        /// <typeparam name="T">The type of the instance. May be a class or a struct.</typeparam>
        /// <param name="instance">The instance whose default field value is to be read.</param>
        /// <returns>
        /// The current value of the default field, boxed to <see cref="object"/>.
        /// Returns <see langword="null"/> if no default field has been registered for <typeparamref name="T"/>.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static object GetDefaultValue<T>(this T instance) {
            return FieldAccessor<T>.GetDefaultFieldValue(instance);
        }
        #endregion
    }
}
