using System;
using System.Collections.Generic;

namespace CodeAngel
{
	/// <summary>
	/// 
	/// </summary>
	public interface ITestInterface
	{
		string TestMethod(int id);
	}

	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="CodeAngel.ITestInterface" />
	public class TestClass : ITestInterface
    {
		/// <summary>
		/// Gets the stuff.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="name">The name.</param>
		/// <returns></returns>
		public List<string> GetStuff(int id, string name)
        {
            return new List<string>();
        }

		/// <summary>
		/// Sets a thing.
		/// </summary>
		private static void SetAThing()
        {
            //Do a thing
        }

		// TODO: Here we should insert <inheritdoc cref="..."/>
		/// <summary>
		/// Tests the method.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		public string TestMethod(int id)
		{
			return "123";
		}
    }

	/// <summary>
	/// 
	/// </summary>
	public class TestProgramClass
    {
		/// <summary>
		/// Tests the main.
		/// </summary>
		/// <param name="args">The arguments.</param>
		static void TestMain(string[] args)
        {
            Console.WriteLine("Hello World!");
        }

		/// <summary>
		/// Parses the text.
		/// </summary>
		/// <returns></returns>
		static string ParseText()
        {
            return ""; 
        }

		/// <summary>
		/// Determines whether [is a thing].
		/// </summary>
		/// <returns>
		///   <c>true</c> if [is a thing]; otherwise, <c>false</c>.
		/// </returns>
		public bool IsAThing()
		{
			return false;
		}

		/// <summary>
		/// Shoulds the do a thing.
		/// </summary>
		/// <returns></returns>
		public bool ShouldDoAThing()
		{
			return true;
		}

		/// <summary>
		/// Gets the identifier list.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <returns></returns>
		public List<string> GetIdList(string name)
		{
			return new List<string>();
		}

		/// <summary>
		/// Gets or sets a value indicating whether [should enable a thing].
		/// </summary>
		/// <value>
		///   <c>true</c> if [should enable a thing]; otherwise, <c>false</c>.
		/// </value>
		public bool ShouldEnableAThing { get; set; }

		/// <summary>
		/// Gets a value indicating whether [should enable feature one].
		/// </summary>
		/// <value>
		///   <c>true</c> if [should enable feature one]; otherwise, <c>false</c>.
		/// </value>
		public bool ShouldEnableFeatureOne { get; }

		/// <summary>
		/// The number field
		/// </summary>
		public readonly int NumberField = 123;
    }
}
