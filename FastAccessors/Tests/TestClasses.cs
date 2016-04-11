#if DEBUGTEST
namespace FastAccessors.Tests {
    class Foo {
        static string static_Name = "Foo(Static)";
        static string GetName() { 
            return static_Name; 
        }
        //
        string private_Name;
        readonly string readonly_Name;
        public string public_Name;
        //
        public Foo(string name) {
            this.readonly_Name = name + "(Readonly)";
            this.private_Name = name + "(Private)";
            this.public_Name = name + "(Public)";
        }
    }
}
#endif