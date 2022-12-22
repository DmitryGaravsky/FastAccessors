namespace FastAccessors.Benchmarks {
    using BenchmarkDotNet.Attributes;
    using BF = System.Reflection.BindingFlags;

    [MemoryDiagnoser]
    public partial class Benchmark {
        public static void Main() {
            BenchmarkDotNet.Running.BenchmarkRunner.Run<Benchmark>();
        }
        //
        [GlobalSetup]
        public void SetUp() {
            foo = new Foo("Foo");
            fi_public_Name = typeof(Foo).GetField("public_Name", BF.Public | BF.Instance);
            fi_static_Name = typeof(Foo).GetField("static_Name", BF.NonPublic | BF.Static);
            fi_private_Name = typeof(Foo).GetField("private_Name", BF.NonPublic | BF.Instance);
            fi_readonly_Name = typeof(Foo).GetField("readonly_Name", BF.NonPublic | BF.Instance);
            //
            bar = new Bar("Bar".Length);
            fi_public_Size = typeof(Bar).GetField("public_Size", BF.Public | BF.Instance);
            fi_static_Size = typeof(Bar).GetField("static_Size", BF.NonPublic | BF.Static);
            fi_private_Size = typeof(Bar).GetField("private_Size", BF.NonPublic | BF.Instance);
            fi_readonly_Size = typeof(Bar).GetField("readonly_Size", BF.NonPublic | BF.Instance);
            //
            Field.RegisterDefault<Foo>("readonly_Name");
            fldKey_private_Name_Field = Field.Register("private_Name", typeof(Foo));
            fldKey_static_Name_Field = Field.RegisterStatic("static_Name", typeof(Foo));
            //
            Field.RegisterDefault<Bar>("readonly_Size");
            fldKey_private_Size_Field = Field.Register("private_Size", typeof(Bar));
            fldKey_static_Size_Field = Field.RegisterStatic("static_Size", typeof(Bar));
            //
            Field.RegisterDefaultStatic("DefaultName", typeof(FooBar));
        }
        Foo foo;
        System.Reflection.FieldInfo fi_public_Name;
        System.Reflection.FieldInfo fi_static_Name;
        System.Reflection.FieldInfo fi_private_Name;
        System.Reflection.FieldInfo fi_readonly_Name;
        Bar bar;
        System.Reflection.FieldInfo fi_public_Size;
        System.Reflection.FieldInfo fi_static_Size;
        System.Reflection.FieldInfo fi_private_Size;
        System.Reflection.FieldInfo fi_readonly_Size;
        int fldKey_private_Name_Field;
        int fldKey_static_Name_Field;
        int fldKey_private_Size_Field;
        int fldKey_static_Size_Field;
    }
}