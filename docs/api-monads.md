# API Reference: `FastAccessors.Monads.Accessor`

`@Accessor` is a static partial class in the `FastAccessors.Monads` namespace that exposes the same field-access functionality as [`FastAccessors.Field`](api-field.md) through extension methods using Unicode identifiers (`@ƒ`, `@ƒs`, `@ƒRegister`, …).

The `@` prefix is a standard C# verbatim identifier — it allows these symbols to be used as method names.

```csharp
using FastAccessors.Monads;
```

The examples below use this type:

```csharp
class Person {
    string name;            // private instance field
    public string tag;      // public instance field
    static string category; // static field
}
```

---

## Instance field access

| Method | Description |
|--------|-------------|
| `obj.@ƒ(Type type, string fieldName)` | Read by name (non-generic) |
| `obj.@ƒ(int key)` | Read by pre-registered key (non-generic) |
| `instance.@ƒ<T>(string fieldName)` | Read by name (generic, no boxing) |
| `instance.@ƒ<T>(int fieldKey)` | Read by key (generic, no boxing) |
| `instance.ƒDefault<T>()` | Read default field for `T` (fastest) |

### Signatures

```csharp
public static object @ƒ(this object instance, Type type, string fieldName)
public static object @ƒ(this object instance, int key)
public static object @ƒ<T>(this T instance, string fieldName)
public static object @ƒ<T>(this T instance, int fieldKey)
public static object ƒDefault<T>(this T instance)
```

All return the field value boxed to `object`, or `null` if the field does not exist / no default has been registered.

### Examples

```csharp
var person = new Person();

// By name — non-generic (any object)
string name = person.@ƒ(typeof(Person), "name") as string;
string tag  = person.@ƒ(typeof(Person), "tag")  as string;

// By name — generic (no boxing, type known at compile time)
string name = person.@ƒ<Person>("name") as string;

// By pre-registered key — non-generic
static readonly int _nameKey = "name".@ƒRegister(typeof(Person));
string name = person.@ƒ(_nameKey) as string;

// By pre-registered key — generic
static readonly int _nameKey = "name".@ƒRegister<Person>();
string name = person.@ƒ<Person>(_nameKey) as string;

// Default field (fastest) — no name, no key
"name".@ƒRegister<Person>(defaultField: true); // register once at startup
string name = person.ƒDefault<Person>() as string;
```

---

## Static field access

| Method | Description |
|--------|-------------|
| `typeof(Foo).@ƒs(string fieldName)` | Read static field by name |
| `@Accessor.@ƒs(int key)` | Read static field by pre-registered key |
| `@Accessor.@ƒsDefault()` | Read default static field (fastest) |

### Signatures

```csharp
public static object @ƒs(this Type type, string fieldName)
public static object @ƒs(int key)
public static object @ƒsDefault()
```

### Examples

```csharp
// By name — type is the receiver
string category = typeof(Person).@ƒs("category") as string;

// By pre-registered key
static readonly int _categoryKey = "category".@ƒsRegister(typeof(Person));
string category = @Accessor.@ƒs(_categoryKey) as string;

// Default static field (fastest)
"category".@ƒsRegister(typeof(Person), defaultField: true); // register once
string category = @Accessor.@ƒsDefault() as string;
```

---

## Registration

| Method | Description |
|--------|-------------|
| `"fieldName".@ƒRegister(Type type)` | Register instance field, returns key |
| `"fieldName".@ƒsRegister(Type type, bool defaultField = false)` | Register static field, optionally as default |
| `"fieldName".@ƒRegister<T>(bool defaultField = false)` | Register instance field for `T`, optionally as default |

### Signatures

```csharp
public static int @ƒRegister(this string fieldName, Type type)
public static int @ƒsRegister(this string fieldName, Type type, bool defaultField = false)
public static int @ƒRegister<T>(this string fieldName, bool defaultField = false)
```

All return an `int` key that can be cached and passed to the corresponding key-based read method.

### Examples

```csharp
// Instance field — non-generic
int nameKey = "name".@ƒRegister(typeof(Person));

// Instance field — generic (preferred, avoids boxing on reads)
int nameKey = "name".@ƒRegister<Person>();

// Instance field — generic, also mark as default
int nameKey = "name".@ƒRegister<Person>(defaultField: true);

// Static field
int categoryKey = "category".@ƒsRegister(typeof(Person));

// Static field — also mark as default
int categoryKey = "category".@ƒsRegister(typeof(Person), defaultField: true);
```

---

## Complete startup + hot-path pattern

```csharp
using FastAccessors.Monads;

static class PersonAccessor {
    // --- Startup registration (static constructor or app init) ---
    static readonly int NameKey     = "name".@ƒRegister<Person>(defaultField: true);
    static readonly int CategoryKey = "category".@ƒsRegister(typeof(Person), defaultField: true);

    // --- Hot path: reads ---

    // Fastest instance read — no name, no key
    public static string GetName(Person p) => p.ƒDefault<Person>() as string;

    // Fastest static read — no name, no key
    public static string GetCategory() => @Accessor.@ƒsDefault() as string;

    // Key-based reads — one level slower, still very fast
    public static string GetNameByKey(Person p)  => p.@ƒ<Person>(NameKey) as string;
    public static string GetCategoryByKey()      => @Accessor.@ƒs(CategoryKey) as string;

    // By-name reads — most flexible, slowest
    public static string GetNameByLabel(Person p, string field) =>
        p.@ƒ(typeof(Person), field) as string;
}
```

---

See [Getting Started](getting-started.md) for a broader introduction and performance comparison.
