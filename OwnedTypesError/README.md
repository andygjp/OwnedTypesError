# README

Program that demonstrates error thrown in rc1 ef core.

Stack trace:

```text
Unhandled exception. System.InvalidOperationException: Sequence contains no elements
at System.Linq.ThrowHelper.ThrowNoElementsException()
at System.Linq.Enumerable.Aggregate[TSource](IEnumerable`1 source, Func`3 func)
at Microsoft.EntityFrameworkCore.Query.RelationalSqlTranslatingExpressionVisitor.TryRewriteEntityEquality(ExpressionType nodeType, Expression left, Expression right, Boolean equalsMethod, Expression& result)
at Microsoft.EntityFrameworkCore.Query.RelationalSqlTranslatingExpressionVisitor.VisitBinary(BinaryExpression binaryExpression)
at Microsoft.EntityFrameworkCore.SqlServer.Query.Internal.SqlServerSqlTranslatingExpressionVisitor.VisitBinary(BinaryExpression binaryExpression)
at Microsoft.EntityFrameworkCore.Query.RelationalSqlTranslatingExpressionVisitor.TranslateInternal(Expression expression)
at Microsoft.EntityFrameworkCore.Query.RelationalSqlTranslatingExpressionVisitor.Translate(Expression expression)
at Microsoft.EntityFrameworkCore.Query.RelationalQueryableMethodTranslatingExpressionVisitor.TranslateExpression(Expression expression)
at Microsoft.EntityFrameworkCore.Query.RelationalQueryableMethodTranslatingExpressionVisitor.TranslateLambdaExpression(ShapedQueryExpression shapedQueryExpression, LambdaExpression lambdaExpression)
at Microsoft.EntityFrameworkCore.Query.RelationalQueryableMethodTranslatingExpressionVisitor.TranslateWhere(ShapedQueryExpression source, LambdaExpression predicate)
at Microsoft.EntityFrameworkCore.Query.QueryableMethodTranslatingExpressionVisitor.VisitMethodCall(MethodCallExpression methodCallExpression)
at Microsoft.EntityFrameworkCore.Query.RelationalQueryableMethodTranslatingExpressionVisitor.VisitMethodCall(MethodCallExpression methodCallExpression)
at Microsoft.EntityFrameworkCore.Query.QueryableMethodTranslatingExpressionVisitor.VisitMethodCall(MethodCallExpression methodCallExpression)
at Microsoft.EntityFrameworkCore.Query.RelationalQueryableMethodTranslatingExpressionVisitor.VisitMethodCall(MethodCallExpression methodCallExpression)
at Microsoft.EntityFrameworkCore.Query.QueryCompilationContext.CreateQueryExecutor[TResult](Expression query)
at Microsoft.EntityFrameworkCore.Storage.Database.CompileQuery[TResult](Expression query, Boolean async)
at Microsoft.EntityFrameworkCore.Query.Internal.QueryCompiler.CompileQueryCore[TResult](IDatabase database, Expression query, IModel model, Boolean async)
at Microsoft.EntityFrameworkCore.Query.Internal.QueryCompiler.<>c__DisplayClass9_0`1.<Execute>b__0()
at Microsoft.EntityFrameworkCore.Query.Internal.CompiledQueryCache.GetOrAddQuery[TResult](Object cacheKey, Func`1 compiler)
at Microsoft.EntityFrameworkCore.Query.Internal.QueryCompiler.Execute[TResult](Expression query)
at Microsoft.EntityFrameworkCore.Query.Internal.EntityQueryProvider.Execute[TResult](Expression expression)
at Microsoft.EntityFrameworkCore.Query.Internal.EntityQueryable`1.GetEnumerator()
at System.Collections.Generic.List`1..ctor(IEnumerable`1 collection)
at System.Linq.Enumerable.ToList[TSource](IEnumerable`1 source)
at Program.<<Main>$>g__GetEntity|0_7(DbContextOptions`1 dbContextOptions, Entity existingEntity, Expression`1 predicate)
at Program.<Main>$(String[] args)```