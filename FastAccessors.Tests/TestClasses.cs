namespace FastAccessors.Tests {
    class Foo {
        static string static_Name = "Foo(Static)";
        static string GetName() { 
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
        static int GetSize() {
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
}