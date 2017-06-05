namespace FastAccessors.Tests {
    using System;
    using NUnit.Framework;

    [TestFixture]
    public class FieldAccessor_Tests_Ref {
        #region SetUp
        object obj;
        Foo instance;
        Type instanceType;
        static int fld_private_Name_T, fld_private_Name, fld_static_Name;
        [SetUp]
        public void SetUp() {
            obj = instance = new Foo("Foo");
            instanceType = typeof(Foo);
            fld_private_Name = Field.Register("private_Name", instanceType);
            fld_static_Name = Field.RegisterStatic("static_Name", instanceType, true);
            fld_private_Name_T = Field.Register<Foo>("private_Name", true);
        }
        #endregion

        [Test(Description = "1.1 DynamicMethod(Instance)")]
        public void Accessor01_DynamicMethod() {
            Assert.AreEqual("Foo(Private)", Field.GetValue(obj, instanceType, "private_Name"));
        }
        [Test(Description = "1.2 DynamicMethod(Public)")]
        public void Accessor01_DynamicMethod_Public() {
            Assert.AreEqual("Foo(Public)", Field.GetValue(obj, instanceType, "public_Name"));
        }
        [Test(Description = "1.3 DynamicMethod(Readonly)")]
        public void Accessor01_DynamicMethod_Readonly() {
            Assert.AreEqual("Foo(Readonly)", Field.GetValue(obj, instanceType, "readonly_Name"));
        }
        [Test(Description = "1.4 DynamicMethod(Static)")]
        public void Accessor01_DynamicMethod_Static() {
            Assert.AreEqual("Foo(Static)", Field.GetStaticValue(instanceType, "static_Name"));
        }
        [Test(Description = "1.5 DynamicMethod(Generic)")]
        public void Accessor01_DynamicMethod_Generic() {
            Assert.AreEqual("Foo(Private)", Field.GetValue<Foo>(instance, "private_Name"));
        }
        [Test(Description = "2.1 DynamicMethod(Instance,Key)")]
        public void Accessor02_DynamicMethod_Key() {
            Assert.AreEqual("Foo(Private)", Field.GetValue(obj, fld_private_Name));
        }
        [Test(Description = "2.2 DynamicMethod(Static,Key)")]
        public void Accessor02_DynamicMethod_Static_Key() {
            Assert.AreEqual("Foo(Static)", Field.GetStaticValue(fld_static_Name));
        }
        [Test(Description = "2.3 DynamicMethod(Generic,Key)")]
        public void Accessor02_DynamicMethod_Generic_Key() {
            Assert.AreEqual("Foo(Private)", Field.GetValue<Foo>(instance, fld_private_Name_T));
        }
        [Test(Description = "3.1 DynamicMethod(Static,DefaultField)")]
        public void Accessor02_DynamicMethod_Static_DefaultField() {
            Assert.AreEqual("Foo(Static)", Field.GetDefaultStaticValue());
        }
        [Test(Description = "3.2 DynamicMethod(Generic,DefaultField)")]
        public void Accessor02_DynamicMethod_Generic_DefaultField() {
            Assert.AreEqual("Foo(Private)", Field.GetDefaultValue<Foo>(instance));
        }
    }
    [TestFixture]
    public class FieldAccessor_Tests_Val {
        #region SetUp
        object obj;
        Bar instance;
        Type instanceType;
        static int fld_private_Size_T, fld_private_Size, fld_static_Size;
        [SetUp]
        public void SetUp() {
            obj = instance = new Bar("Bar".Length);
            instanceType = typeof(Bar);
            fld_private_Size = Field.Register("private_Size", instanceType);
            fld_static_Size = Field.RegisterStatic("static_Size", instanceType, true);
            fld_private_Size_T = Field.Register<Bar>("private_Size", true);
        }
        #endregion
        [Test(Description = "1.1 DynamicMethod(Instance)")]
        public void Accessor01_DynamicMethod() {
            Assert.AreEqual("Bar(Private)".Length, Field.GetValue(obj, instanceType, "private_Size"));
        }
        [Test(Description = "1.2 DynamicMethod(Public)")]
        public void Accessor01_DynamicMethod_Public() {
            Assert.AreEqual("Bar(Public)".Length, Field.GetValue(obj, instanceType, "public_Size"));
        }
        [Test(Description = "1.3 DynamicMethod(Readonly)")]
        public void Accessor01_DynamicMethod_Readonly() {
            Assert.AreEqual("Bar(Readonly)".Length, Field.GetValue(obj, instanceType, "readonly_Size"));
        }
        [Test(Description = "1.4 DynamicMethod(Static)")]
        public void Accessor01_DynamicMethod_Static() {
            Assert.AreEqual("Bar(Static)".Length, Field.GetStaticValue(instanceType, "static_Size"));
        }
        [Test(Description = "1.5 DynamicMethod(Generic)")]
        public void Accessor01_DynamicMethod_Generic() {
            Assert.AreEqual("Bar(Private)".Length, Field.GetValue<Bar>(instance, "private_Size"));
        }
        [Test(Description = "2.1 DynamicMethod(Instance,Key)")]
        public void Accessor02_DynamicMethod_Key() {
            Assert.AreEqual("Bar(Private)".Length, Field.GetValue(obj, fld_private_Size));
        }
        [Test(Description = "2.2 DynamicMethod(Static,Key)")]
        public void Accessor02_DynamicMethod_Static_Key() {
            Assert.AreEqual("Bar(Static)".Length, Field.GetStaticValue(fld_static_Size));
        }
        [Test(Description = "2.3 DynamicMethod(Generic,Key)")]
        public void Accessor02_DynamicMethod_Generic_Key() {
            Assert.AreEqual("Bar(Private)".Length, Field.GetValue<Bar>(instance, fld_private_Size_T));
        }
        [Test(Description = "3.1 DynamicMethod(Static,DefaultField)")]
        public void Accessor02_DynamicMethod_Static_DefaultField() {
            Assert.AreEqual("Bar(Static)".Length, Field.GetDefaultStaticValue());
        }
        [Test(Description = "3.2 DynamicMethod(Generic,DefaultField)")]
        public void Accessor02_DynamicMethod_Generic_DefaultField() {
            Assert.AreEqual("Bar(Private)".Length, Field.GetDefaultValue<Bar>(instance));
        }
    }
}