using System;
using System.Collections.Generic;

namespace CodeAngel
{
	public interface ITestInterface
	{
		string TestMethod(int id);
	}

	public class TestClass : ITestInterface
    {
		public TestClass(int id, string name, DateTime dateTime)
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
    }

	public class TestProgramClass
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

		public List<string> GetIdList(string name)
		{
			return new List<string>();
        }

        public bool AccessLimit { get; }

        public bool ShouldEnableAThing { get; set; }

		public bool ShouldEnableFeatureOne { get; }

		public readonly int NumberField = 123;
    }
}
