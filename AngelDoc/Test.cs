using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace AngelDoc
{
    public interface ITestInterface
    {
        string TestMethod(int id);
    }

    [ExcludeFromCodeCoverage]
    public class TestClass : IDisposable, ITestInterface
    {
        public TestClass(int id, string name, DateTime dateTime, float stuff, double percentage, Guid externalId)
        {

        }

        public List<string> GetStuff(int id, string name)
        {
            return new List<string>();
        }

        private static void SetAThing()
        {
            //Do a thing
        }

        public string TestMethod(int id)
        {
            return "123";
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }

    [ExcludeFromCodeCoverage]
    public class TestProgramClass<T>
    {
        static void TestMain(string[] args)
        {
            Console.WriteLine("Hello World!");
        }

        static string ParseText()
        {
            return "";
        }

        public bool IsAThing()
        {
            return false;
        }

        public bool ShouldDoAThing()
        {
            return true;
        }

        public TStuff GetStuffAsT<TStuff>(T stuff)
        {
            return default(TStuff);
        }

        public List<string> GetIdList(string name)
        {
            return new List<string>();
        }

        public bool AccessLimit { get; }

        public bool ShouldEnableAThing { get; set; }

        public bool ShouldEnableFeatureOne { get; }

        public readonly int NumberField = 123;
    }

    [ExcludeFromCodeCoverage]
    public class GenericClass<TStuff>
    {
        public TThing GetTStuff<TThing>(int id)
        {
            return default(TThing);
        }
    }

    public interface ITestGenericInterface<T>
    { }
}
