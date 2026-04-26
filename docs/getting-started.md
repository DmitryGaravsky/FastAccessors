# Getting Started

FastAccessors provides three access patterns for reading fields, in increasing order of speed.

Given this class:

```csharp
class Foo {
    string private_Name;
    int    private_Field;
    static int static_Field;
}
```

---

## Pattern 1 — By-name access

No registration required. The IL accessor is generated on first call and cached automatically.

```csharp
using FastAccessors;

var foo = new Foo();

// Non-generic
string name = (string)Field.GetValue(foo, typeof(Foo), "private_Name");

// Generic extension method — no boxing of the instance
string name = (string)foo.GetValue<Foo>("private_Name");

// Monads-style extension method
string name = foo.@ƒ(typeof(Foo), "private_Name") as string;
```

---

## Pattern 2 — Key-based access

Register the field once at startup and cache the returned `int` key. Subsequent reads skip the name lookup entirely.

```csharp
using FastAccessors;

// Registration (do this once, e.g. in a static constructor)
static readonly int _keyPrivateField  = Field.Register("private_Field", typeof(Foo));
static readonly int _keyStaticField   = Field.RegisterStatic("static_Field", typeof(Foo));

// Generic registration
static readonly int _keyGenericField  = Field.Register<Foo>("private_Name");

var foo = new Foo();

int  value  = (int) Field.GetValue(foo, _keyPrivateField);
int  sValue = (int) Field.GetStaticValue(_keyStaticField);
string name = (string) foo.GetValue<Foo>(_keyGenericField);

// Monads style
int value = (int) foo.@ƒ(_keyPrivateField);
int sValue = (int) typeof(Foo).@ƒs(_keyStaticField);
```

---

## Pattern 3 — Default-field access (fastest)

Designate exactly one field per type as the "default". Reads require no name or key lookup — the accessor delegate is held in a direct static reference.

```csharp
using FastAccessors;

// Registration (do this once at startup)
Field.RegisterDefault<Foo>("private_Name");          // default instance field
Field.RegisterDefaultStatic("static_Field", typeof(Foo));  // default static field

var foo = new Foo();

string name   = (string) foo.GetDefaultValue<Foo>();
int    sValue = (int) Field.GetDefaultStaticValue();

// Monads style
string name   = foo.ƒDefault<Foo>() as string;
int    sValue = (int) @Accessor.@ƒsDefault();
```

---

## Choosing a pattern

| Pattern | Relative speed | When to use |
|---------|---------------|-------------|
| By-name | ~90 ns | Occasional reads, dynamic field names |
| Key-based | ~18 ns | Hot paths reading the same field repeatedly |
| Default-field | ~7 ns | Innermost loops where every nanosecond matters |

See [Benchmarks](../README.MD#benchmarks) for measured numbers.

---

## Next steps

- [API Reference: Field](api-field.md)
- [API Reference: Monads (Accessor)](api-monads.md)
