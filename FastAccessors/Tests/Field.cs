#if DEBUGTEST
namespace FastAccessors.Tests {
    using System;
    using NUnit.Framework;

    #region Test Classes
    class Foo {
        static string static_Name = "Foo(Static)";
        static string GetName() { return static_Name; }
        string private_Name;
        readonly string readonly_Name;
        public string public_Name;
        public Foo(string name) {
            this.readonly_Name = name + "(Readonly)";
            this.private_Name = name + "(Private)";
            this.public_Name = name + "(Public)";
        }
    }
    #endregion
    [TestFixture]
    public class FieldAccessor_Tests {
        #region SetUp
        object obj;
        Foo instance;
        Type instanceType;
        static int fld_private_Name_T;
        static int fld_private_Name;
        static int fld_static_Name;
        [SetUp]
        public void SetUp() {
            obj = instance = new Foo("Foo");
            instanceType = typeof(Foo);
            fld_private_Name = "private_Name".ƒRegister(instanceType);
            fld_static_Name = "static_Name".ƒsRegister(instanceType, true);
            fld_private_Name_T = "private_Name".ƒRegister<Foo>(true);
        }
        #endregion
        [Test(Description = "1.1 DynamicMethod(Instance)")]
        public void Accessor01_DynamicMethod() {
            Assert.AreEqual("Foo(Private)", obj.@ƒ(instanceType, "private_Name"));
        }
        [Test(Description = "1.2 DynamicMethod(Public)")]
        public void Accessor01_DynamicMethod_Public() {
            Assert.AreEqual("Foo(Public)", obj.@ƒ(instanceType, "public_Name"));
        }
        [Test(Description = "1.3 DynamicMethod(Readonly)")]
        public void Accessor01_DynamicMethod_Readonly() {
            Assert.AreEqual("Foo(Readonly)", obj.@ƒ(instanceType, "readonly_Name"));
        }
        [Test(Description = "1.4 DynamicMethod(Static)")]
        public void Accessor01_DynamicMethod_Static() {
            Assert.AreEqual("Foo(Static)", instanceType.@ƒs("static_Name"));
        }
        [Test(Description = "1.5 DynamicMethod(Generic)")]
        public void Accessor01_DynamicMethod_Generic() {
            Assert.AreEqual("Foo(Private)", instance.@ƒ("private_Name"));
        }
        [Test(Description = "2.1 DynamicMethod(Instance,Key)")]
        public void Accessor02_DynamicMethod_Key() {
            Assert.AreEqual("Foo(Private)", obj.@ƒ(fld_private_Name));
        }
        [Test(Description = "2.2 DynamicMethod(Static,Key)")]
        public void Accessor02_DynamicMethod_Static_Key() {
            Assert.AreEqual("Foo(Static)", Accessor.@ƒs(fld_static_Name));
        }
        [Test(Description = "2.3 DynamicMethod(Generic,Key)")]
        public void Accessor02_DynamicMethod_Generic_Key() {
            Assert.AreEqual("Foo(Private)", instance.@ƒ(fld_private_Name_T));
        }
        [Test(Description = "2.4 DynamicMethod(Static,DefaultField)")]
        public void Accessor02_DynamicMethod_Static_DefaultField() {
            Assert.AreEqual("Foo(Static)", Accessor.@ƒsDefault());
        }
        [Test(Description = "2.5 DynamicMethod(Generic,DefaultField)")]
        public void Accessor02_DynamicMethod_Generic_DefaultField() {
            Assert.AreEqual("Foo(Private)", instance.@ƒDefault());
        }
    }
}
#endif