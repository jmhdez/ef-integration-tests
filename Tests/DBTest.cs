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
			using (var context = CreateContext())
				DbTools.CleanDatabase(context);

			BeforeAll();
		}

		protected virtual void BeforeAll()
		{
		}

		[SetUp]
		protected virtual void BeforeEach()
		{
		}

		protected MovieDbContext CreateContext()
		{
			return new TestDbContext();
		}

		protected void Execute(Action<MovieDbContext> action)
		{
			using (var context = new TestDbContext())
			using (var tx = context.Database.BeginTransaction())
			{
				action(context);

				context.SaveChanges();
				tx.Commit();
			}
		}

		protected T Query<T>(Func<MovieDbContext, T> query)
		{
			T result;

			using (var context = new TestDbContext())
			using (var tx = context.Database.BeginTransaction())
			{
				result = query(context);

				context.SaveChanges();
				tx.Commit();
			}

			return result;
		}
	}
}