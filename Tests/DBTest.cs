using System;
using Koalite.EFSample.DB;
using NUnit.Framework;

namespace Tests
{
	public abstract class DBTest
	{
		[TestFixtureSetUp]
		public void TestFixtureSetup()
		{
			Execute(DbTools.CleanDatabase);
			BeforeAll();
		}

		protected virtual void BeforeAll()
		{
		}

		[SetUp]
		protected virtual void BeforeEach()
		{
		}


		protected void Execute(Action<MovieDbContext> action)
		{
			using (var context = new TestDbContext())
				action(context);
		}

		protected T Execute<T>(Func<MovieDbContext, T> query)
		{
			using (var context = new TestDbContext())
				return query(context);
		}
	}
}