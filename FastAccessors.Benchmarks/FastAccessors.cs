namespace FastAccessors.Benchmarks {
    using BenchmarkDotNet.Attributes;

    partial class Benchmark {
        [Benchmark(Description = "2.1 FastAccessors(Instance)")]
        public int FastAccessors_Instance() {
            var name = (string)Field.GetValue(foo, typeof(Foo), "private_Name");
            var size = (int)Field.GetValue(bar, typeof(Bar), "private_Size");
            return name.Length + size;
        }
        [Benchmark(Description = "2.2. FastAccessors(Static)")]
        public int FastAccessors_Static() {
            var name = (string)Field.GetStaticValue(typeof(Foo), "static_Name");
            var size = (int)Field.GetStaticValue(typeof(Bar), "static_Size");
            return name.Length + size;
        }
        [Benchmark(Description = "2.3. FastAccessors(Public)")]
        public int FastAccessors_Public() {
            var name = (string)Field.GetValue(foo, typeof(Foo), "public_Name");
            var size = (int)Field.GetValue(bar, typeof(Bar), "public_Size");
            return name.Length + size;
        }
        [Benchmark(Description = "2.4. FastAccessors(Readonly)")]
        public int FastAccessors_Readonly() {
            var name = (string)Field.GetValue(foo, typeof(Foo), "readonly_Name");
            var size = (int)Field.GetValue(bar, typeof(Bar), "readonly_Size");
            return name.Length + size;
        }
        [Benchmark(Description = "2.5. FastAccessors(Generic)")]
        public int FastAccessors_Generic() {
            var name = (string)Field.GetValue(foo, "private_Name");
            var size = (int)Field.GetValue(bar, "private_Size");
            return name.Length + size;
        }
        //
        [Benchmark(Description = "3.1 FastAccessors(Instance,Key)")]
        public int FastAccessors_Instance_Key() {
            var name = (string)Field.GetValue((object)foo, fldKey_private_Name_Field);
            var size = (int)Field.GetValue((object)bar, fldKey_private_Size_Field);
            return name.Length + size;
        }
        [Benchmark(Description = "3.2 FastAccessors(Static,Key)")]
        public int FastAccessors_Static_Key() {
            var name = (string)Field.GetStaticValue(fldKey_static_Name_Field);
            var size = (int)Field.GetStaticValue(fldKey_static_Size_Field);
            return name.Length + size;
        }
        [Benchmark(Description = "3.3 FastAccessors(Generic,Key)")]
        public int FastAccessors_Generic_Key() {
            var name = (string)Field.GetValue(foo, fldKey_private_Name_Field);
            var size = (int)Field.GetValue(bar, fldKey_private_Size_Field);
            return name.Length + size;
        }
        //
        [Benchmark(Description = "4.1 FastAccessors(Generic,DefaultField)")]
        public int FastAccessors_Generic_DefaultField() {
            var name = (string)Field.GetDefaultValue(foo);
            var size = (int)Field.GetDefaultValue(bar);
            return name.Length + size;
        }
        [Benchmark(Description = "4.2 FastAccessors(Static,DefaultField)")]
        public int FastAccessors_Static_DefaultField() {
            var name1 = (string)Field.GetDefaultStaticValue();
            var name2 = (string)Field.GetDefaultStaticValue();
            return name1.Length + name2.Length;
        }
    }
}