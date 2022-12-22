namespace FastAccessors.Benchmarks {
    using BenchmarkDotNet.Attributes;
    using BF = System.Reflection.BindingFlags;

    partial class Benchmark {
        [Benchmark(Description = "Direct Access", Baseline = true)]
        public int DirectAccess() {
            return foo.public_Name.Length + bar.public_Size;
        }
        [Benchmark(Description = "Direct Access(Static)")]
        public int DirectAccess_Static() {
            return Foo.GetName().Length + Bar.GetSize();
        }
        //
        [Benchmark(Description = "1.1. Reflection(Instance)")]
        public int Reflection_Instance() {
            var fi_pname = typeof(Foo).GetField("private_Name", BF.NonPublic | BF.Instance);
            var name = (string)fi_pname.GetValue(foo);
            var fi_psize = typeof(Foo).GetField("private_Size", BF.NonPublic | BF.Instance);
            var size = (int)fi_psize.GetValue(bar);
            return name.Length + size;
        }
        [Benchmark(Description = "1.2. Reflection(Static)")]
        public int Reflection_Static() {
            var fi_psname = typeof(Foo).GetField("static_Name", BF.NonPublic | BF.Static);
            var name = (string)fi_psname.GetValue(foo);
            var fi_pssize = typeof(Foo).GetField("static_Size", BF.NonPublic | BF.Static);
            var size = (int)fi_pssize.GetValue(bar);
            return name.Length + size;
        }
        [Benchmark(Description = "1.3. Reflection(Public)")]
        public int Reflection_Public() {
            var fi_pname = typeof(Foo).GetField("public_Name", BF.Public | BF.Instance);
            var name = (string)fi_pname.GetValue(foo);
            var fi_psize = typeof(Foo).GetField("public_Size", BF.Public | BF.Instance);
            var size = (int)fi_psize.GetValue(bar);
            return name.Length + size;
        }
        [Benchmark(Description = "1.4. Reflection(Readonly)")]
        public int Reflection_Readonly() {
            var fi_prname = typeof(Foo).GetField("readonly_Name", BF.NonPublic | BF.Instance);
            var name = (string)fi_prname.GetValue(foo);
            var fi_prsize = typeof(Foo).GetField("readonly_Size", BF.NonPublic | BF.Instance);
            var size = (int)fi_prsize.GetValue(bar);
            return name.Length + size;
        }
        //
        [Benchmark(Description = "1.1. Reflection(Instance, Cached)")]
        public int Reflection_Instance_Cached() {
            var name = (string)fi_private_Name.GetValue(foo);
            var size = (int)fi_private_Size.GetValue(bar);
            return name.Length + size;
        }
        [Benchmark(Description = "1.2. Reflection(Static, Cached)")]
        public int Reflection_Static_Cached() {
            var name = (string)fi_static_Name.GetValue(foo);
            var size = (int)fi_static_Size.GetValue(bar);
            return name.Length + size;
        }
        [Benchmark(Description = "1.3. Reflection(Public, Cached)")]
        public int Reflection_Public_Cached() {
            var name = (string)fi_public_Name.GetValue(foo);
            var size = (int)fi_public_Size.GetValue(bar);
            return name.Length + size;
        }
        [Benchmark(Description = "1.4. Reflection(Readonly, Cached)")]
        public int Reflection_Readonly_Cached() {
            var name = (string)fi_readonly_Name.GetValue(foo);
            var size = (int)fi_readonly_Size.GetValue(bar);
            return name.Length + size;
        }
    }
}