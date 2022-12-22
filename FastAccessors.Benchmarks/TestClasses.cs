using System.Runtime.CompilerServices;

namespace FastAccessors.Benchmarks {
    class Foo {
        static string static_Name = "Foo(Static)";
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string GetName() {
            return static_Name;
        }
        string private_Name;
        readonly string readonly_Name;
        public string public_Name;
        public Foo(string name) {
            this.readonly_Name = name + "(Readonly)";
            this.private_Name = name + "(Private)";
            this.public_Name = name + "(Public)";
        }
    }
    class Bar {
        static int static_Size = "Bar(Static)".Length;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetSize() {
            return static_Size;
        }
        int private_Size;
        readonly int readonly_Size;
        public int public_Size;
        public Bar(int size) {
            this.readonly_Size = size + "(Readonly)".Length;
            this.private_Size = size + "(Private)".Length;
            this.public_Size = size + "(Public)".Length;
        }
    }
    static class FooBar {
        static string DefaultName = nameof(FooBar);
        static string GetDefaultName() {
            return DefaultName;
        }
    }
}