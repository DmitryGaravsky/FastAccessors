# API Reference: `FastAccessors.Field`

`Field` is a static class that exposes IL-emitted field accessors for both reference types and value types. Fields can be private, public, or readonly.

The examples below use this type:

```csharp
class Person {
    string name;              // private instance field
    public string tag;        // public instance field
    readonly int id;          // readonly instance field
    static string category;   // static field
}
```

---

## Instance-level methods

### `Register`

```csharp
public static int Register(string fieldName, Type type)
```

Registers an instance field by name. Returns an `int` key to be cached and passed to `GetValue(object, int)`.

| Parameter | Description |
|-----------|-------------|
| `fieldName` | Field name (private, public, or readonly) |
| `type` | The declaring type |

**Returns:** integer key for subsequent key-based reads.

```csharp
// Register once — typically in a static constructor or application startup
static readonly int _nameKey = Field.Register("name", typeof(Person));
static readonly int _idKey   = Field.Register("id",   typeof(Person));
```

---

### `GetValue` (by name)

```csharp
public static object GetValue(object instance, Type type, string fieldName)
```

Reads an instance field by name. The accessor is compiled once and cached.

| Parameter | Description |
|-----------|-------------|
| `instance` | Object instance (may be a boxed value type) |
| `type` | The declaring type |
| `fieldName` | Field name |

**Returns:** field value boxed to `object`, or `null` if the field does not exist.

```csharp
var person = new Person();

// Access private field by name — no key pre-registration needed
string name = (string) Field.GetValue(person, typeof(Person), "name");
int    id   = (int)    Field.GetValue(person, typeof(Person), "id");

// Works on value types (boxed)
object boxed = new MyStruct();
int value = (int) Field.GetValue(boxed, typeof(MyStruct), "count");
```

---

### `GetValue` (by key)

```csharp
public static object GetValue(object instance, int key)
```

Reads an instance field using a pre-registered key. No name lookup is performed.

| Parameter | Description |
|-----------|-------------|
| `instance` | Object instance |
| `key` | Key returned by `Register(string, Type)` |

**Returns:** field value boxed to `object`.

```csharp
static readonly int _nameKey = Field.Register("name", typeof(Person));

var person = new Person();

// Hot path — no dictionary lookup, just a direct delegate call
string name = (string) Field.GetValue(person, _nameKey);
```

---

## Type-level (static field) methods

### `RegisterStatic`

```csharp
public static int RegisterStatic(string fieldName, Type type)
```

Registers a static field. Returns an `int` key for key-based reads.

```csharp
static readonly int _categoryKey = Field.RegisterStatic("category", typeof(Person));
```

---

### `GetStaticValue` (by name)

```csharp
public static object GetStaticValue(Type type, string fieldName)
```

Reads a static field by name.

**Returns:** field value boxed to `object`, or `null` if the field does not exist.

```csharp
string category = (string) Field.GetStaticValue(typeof(Person), "category");
```

---

### `GetStaticValue` (by key)

```csharp
public static object GetStaticValue(int key)
```

Reads a static field using a pre-registered key.

```csharp
static readonly int _categoryKey = Field.RegisterStatic("category", typeof(Person));

// No name lookup — fastest path for static fields after default-field
string category = (string) Field.GetStaticValue(_categoryKey);
```

---

### `RegisterDefaultStatic`

```csharp
public static int RegisterDefaultStatic(string fieldName, Type type)
```

Registers a static field as the **default** static field. Enables `GetDefaultStaticValue()`. A subsequent call replaces the previous default.

**Returns:** `int` key (also usable with `GetStaticValue(int)`).

```csharp
// Mark the field that will be read most often as the default
Field.RegisterDefaultStatic("category", typeof(Person));
```

---

### `GetDefaultStaticValue`

```csharp
public static object GetDefaultStaticValue()
```

Reads the default static field. The accessor delegate is held in a direct field reference — the absolute fastest static read path.

**Returns:** field value boxed to `object`, or `null` if no default has been registered.

```csharp
// After RegisterDefaultStatic — no type, no name, no key — direct delegate call
string category = (string) Field.GetDefaultStaticValue();
```

---

## Generic instance-level methods

### `Register<T>`

```csharp
public static int Register<T>(string fieldName)
```

Registers an instance field for type `T`. Avoids boxing the instance on each read.

**Returns:** `int` key for key-based reads with `GetValue<T>(T, int)`.

```csharp
static readonly int _nameKey = Field.Register<Person>("name");
```

---

### `GetValue<T>` (by name, extension method)

```csharp
public static object GetValue<T>(this T instance, string fieldName)
```

Reads an instance field by name on a strongly-typed instance. Can be called as an extension method.

**Returns:** field value boxed to `object`, or `null` if the field does not exist.

```csharp
var person = new Person();

// Extension method syntax — reads private field without boxing the instance
string name = (string) person.GetValue<Person>("name");

// Works with value types — struct is not boxed
var point = new Point(1, 2);
int x = (int) point.GetValue<Point>("x");
```

---

### `GetValue<T>` (by key, extension method)

```csharp
public static object GetValue<T>(this T instance, int fieldKey)
```

Reads an instance field using a pre-registered key. No name lookup, no boxing of the instance.

```csharp
static readonly int _nameKey = Field.Register<Person>("name");

var person = new Person();

// Fastest generic instance read path with key
string name = (string) person.GetValue<Person>(_nameKey);
```

---

### `RegisterDefault<T>`

```csharp
public static int RegisterDefault<T>(string fieldName)
```

Registers an instance field as the **default** field for type `T`. Enables `GetDefaultValue<T>(T)`. One default per closed generic type; a subsequent call replaces the previous default.

**Returns:** `int` key (also usable with `GetValue<T>(T, int)`).

```csharp
// Designate the most-read field as the default
Field.RegisterDefault<Person>("name");
```

---

### `GetDefaultValue<T>` (extension method)

```csharp
public static object GetDefaultValue<T>(this T instance)
```

Reads the default field for type `T`. The accessor delegate is held in a per-`T` static field — the absolute fastest generic instance read path.

**Returns:** field value boxed to `object`, or `null` if no default has been registered for `T`.

```csharp
// After RegisterDefault<Person> — no name, no key — direct per-T static delegate
var person = new Person();
string name = (string) person.GetDefaultValue<Person>();

// Also works with value types
var point = new Point(1, 2);
int x = (int) point.GetDefaultValue<Point>();
```

---

## Complete startup + hot-path pattern

```csharp
using FastAccessors;

class PersonCache {
    // --- Startup: register fields once ---
    static readonly int _nameKey     = Field.Register<Person>("name");
    static readonly int _categoryKey = Field.RegisterDefaultStatic("category", typeof(Person));

    static PersonCache() {
        Field.RegisterDefault<Person>("name"); // fastest instance path
    }

    // --- Hot path: zero-lookup reads ---
    static string GetName(Person p)     => (string) p.GetDefaultValue<Person>();
    static string GetCategory()         => (string) Field.GetDefaultStaticValue();

    // --- Occasional reads by key ---
    static string GetNameByKey(Person p) => (string) p.GetValue<Person>(_nameKey);
}
```

---

## Access pattern summary

| Method group | Lookup cost | Typical latency |
|---|---|---|
| By-name (`GetValue(obj, type, name)`) | Hash map on every call | ~90 ns |
| Key-based (`GetValue(obj, key)`) | Direct array index | ~18 ns |
| Default-field (`GetDefaultValue<T>`) | None — static delegate reference | ~7 ns |

See [Getting Started](getting-started.md) for a broader introduction and performance comparison.
